using UnityEngine;

public class DianaPuntos : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadZMin = 12f;    // mínimo hacia adelante
    public float velocidadZMax = 18f;    // máximo hacia adelante
    public float variacionLateral = 0.5f;
    public float variacionVertical = 0.3f;

    [Header("Puntaje")]
    public int puntos = 20;

    [Header("Despawn")]
    public float margenDespawn = 5f;

    private Transform jugador;
    private Vector3 direccion; // desviación X/Y
    public float velocidadZ;  // velocidad Z aleatoria por instancia
    private float objetivoZ;

    void Start()
    {
        jugador = GameObject.FindWithTag("Player")?.transform;

        // Pequeña variación lateral y vertical para diagonales
        direccion = new Vector3(
            Random.Range(-variacionLateral, variacionLateral),
            Random.Range(-variacionVertical, variacionVertical),
            0f // Z se controla aparte
        );

        // Asignar velocidad Z aleatoria dentro del rango
        velocidadZ = Random.Range(velocidadZMin, velocidadZMax);

        // El objetivo será pasar un poco al jugador
        if (jugador != null)
            objetivoZ = jugador.position.z + 10f;
    }

    void Update()
    {
        // Movimiento combinado: Z rápido + desviaciones X/Y
        Vector3 movimiento = new Vector3(direccion.x, direccion.y, 1f) * velocidadZ * Time.deltaTime;
        transform.Translate(movimiento, Space.World);

        // Destruir si ya pasó del jugador
        if (jugador != null && transform.position.z > objetivoZ + margenDespawn)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(puntos);

            // Puedes añadir efectos visuales o sonido
            Destroy(gameObject);
        }
    }
}
