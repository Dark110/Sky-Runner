using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public bool moverHaciaJugador = false;

    [Header("Variación")]
    public float variacionVelocidad = 0.5f;
    public float variacionMovimientoLateral = 0.3f;

    [HideInInspector] public Transform jugador;
    private float movimientoLateral;
    private float tiempoCambioDireccion = 0f;
    private float intervaloCambioDireccion = 2f;

    void Start()
    {
        // Añadir variación a la velocidad
        velocidad += Random.Range(-variacionVelocidad, variacionVelocidad);

        // Inicializar movimiento lateral aleatorio
        CambiarDireccionLateral();
    }

    void Update()
    {
        if (moverHaciaJugador)
        {
            // Movimiento hacia el jugador en X/Y (ya que Z no cambia)
            Vector3 direccion = (jugador.position - transform.position).normalized;
            direccion.z = 0; // Ignorar Z
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        }
        else
        {
            // Movimiento lateral suave con variación
            tiempoCambioDireccion -= Time.deltaTime;
            if (tiempoCambioDireccion <= 0f)
            {
                CambiarDireccionLateral();
                tiempoCambioDireccion = intervaloCambioDireccion;
            }

            // Movimiento aleatorio en X/Y
            Vector3 direccion = new Vector3(
                movimientoLateral * variacionMovimientoLateral,
                Random.Range(-0.1f, 0.1f),
                0
            );

            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        }
    }

    void CambiarDireccionLateral()
    {
        movimientoLateral = Random.Range(-1f, 1f);
        intervaloCambioDireccion = Random.Range(1f, 3f);
    }
}