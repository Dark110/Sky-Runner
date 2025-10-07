using UnityEngine;

public class MovObstaculo : MonoBehaviour
{
    [Header("Movimiento hacia adelante")]
    public float VelMin = 3f;
    public float VelMax = 8f;
    private float VelObstaculo;
    public bool moverAdelante = true;

    [Header("Rotación aleatoria")]
    public float RotMin = 30f;
    public float RotMax = 120f;
    private Vector3 velocidadRotacion;

    [Header("Homing (seguir al jugador)")]
    public bool usarHoming = false;
    [Range(0f, 5f)] public float fuerzaHoming = 1f;

    [Header("Predicción de movimiento del jugador (CharacterController)")]
    public bool usarPrediccion = false;
    public float tiempoPrediccion = 0.5f; // segundos hacia adelante para "predecir"

    [Header("Puntaje")]
    public int puntosAlEsquivar = 10;

    private Transform jugador;
    private Vector3 ultimaPosJugador;
    private Vector3 velJugador; // velocidad calculada

    void Start()
    {
        VelObstaculo = Random.Range(VelMin, VelMax);

        velocidadRotacion = new Vector3(
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax)
        );

        if (velocidadRotacion == Vector3.zero)
            velocidadRotacion = Vector3.up * Random.Range(RotMin, RotMax);

        jugador = GameObject.FindWithTag("Player")?.transform;

        if (jugador != null)
            ultimaPosJugador = jugador.position;
    }

    void Update()
    {
        // 🔹 Calcular velocidad aproximada del jugador
        if (jugador != null)
        {
            velJugador = (jugador.position - ultimaPosJugador) / Time.deltaTime;
            ultimaPosJugador = jugador.position;
        }

        // Movimiento básico hacia adelante
        float direccion = moverAdelante ? 1f : -1f;
        Vector3 avance = Vector3.forward * direccion * VelObstaculo * Time.deltaTime;
        transform.Translate(avance, Space.World);

        // Rotación aleatoria
        transform.Rotate(velocidadRotacion * Time.deltaTime, Space.Self);

        // Aplicar homing o predicción si está activado
        if (jugador != null)
        {
            Vector3 targetPos = jugador.position;

            if (usarPrediccion)
            {
                targetPos += velJugador * tiempoPrediccion; // usa velocidad estimada
            }

            if (usarHoming)
            {
                Vector3 direccionHoming = (targetPos - transform.position).normalized;
                // Suaviza el movimiento con Lerp
                transform.position = Vector3.Lerp(
                    transform.position,
                    transform.position + direccionHoming,
                    fuerzaHoming * Time.deltaTime
                );
            }
        }

        // Verifica si ya pasó al jugador
        if (JugadorFueraDeRango())
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(puntosAlEsquivar);

            Destroy(gameObject);
        }
    }

    bool JugadorFueraDeRango()
    {
        if (jugador == null) return false;

        float margen = 5f;

        if (moverAdelante)
            return transform.position.z > jugador.position.z + margen;
        else
            return transform.position.z < jugador.position.z - margen;
    }
}
