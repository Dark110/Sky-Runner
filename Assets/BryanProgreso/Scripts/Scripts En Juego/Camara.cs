    using UnityEngine;

    public class Camara : MonoBehaviour
    {
        [Header("Jugador a seguir")]
        public Transform jugadorCam; // El objeto (jugador) a seguir

        [Header("Configuración de la cámara")]
        public Vector3 offset = new Vector3(0, 0, -4);
        [Range(0.1f, 10f)]
        public float movimientoCam = 2.5f;
        public bool verJugador = true;

        private Rigidbody jugadorRb;
        private float zCongelado; // Valor fijo de Z

        private void Start()
        {
            if (jugadorCam != null)
                jugadorRb = jugadorCam.GetComponent<Rigidbody>();

            // Guardar la posición Z inicial de la cámara
            zCongelado = transform.position.z;
        }

        void FixedUpdate()
        {
            if (jugadorCam == null) return;

            Vector3 objetivo = jugadorRb != null ? jugadorRb.position : jugadorCam.position;
            Vector3 posicionDeseada = objetivo + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(
                transform.position,
                posicionDeseada,
                movimientoCam * Time.fixedDeltaTime
            );

            // Congelar Z
            posicionSuavizada.z = zCongelado;
            transform.position = posicionSuavizada;

            if (verJugador)
                transform.LookAt(objetivo);
        }
    }