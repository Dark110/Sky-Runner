using UnityEngine;

public class MovObstaculo : MonoBehaviour
{
    [Header("Rango de Velocidad (Movimiento)")]
    public float VelMin = 3f;
    public float VelMax = 8f;
    private float VelObstaculo;
    public bool moverAdelante = true;

    [Header("Rango de Velocidad de Rotación")]
    public float RotMin = 30f;
    public float RotMax = 120f;
    private Vector3 velocidadRotacion;

    [Header("Puntaje")]
    public int puntosAlEsquivar = 10;

    private Transform jugador;

    void Start()
    {
        VelObstaculo = Random.Range(VelMin, VelMax);

        velocidadRotacion = new Vector3(
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax)
        );

        if (velocidadRotacion == Vector3.zero)
        {
            velocidadRotacion = Vector3.up * Random.Range(RotMin, RotMax);
        }

        jugador = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        float direccion = moverAdelante ? 1f : -1f;
        transform.Translate(Vector3.forward * direccion * VelObstaculo * Time.deltaTime, Space.World);

        transform.Rotate(velocidadRotacion * Time.deltaTime, Space.Self);

        if (JugadorFueraDeRango())
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(puntosAlEsquivar);

            Destroy(gameObject);
        }
    }

    // Verifica si el obstáculo pasó al jugador
    bool JugadorFueraDeRango()
    {
        if (jugador == null) return false;

        // Ajusta la distancia según necesites
        float margen = 5f;

        if (moverAdelante)
            return transform.position.z > jugador.position.z + margen;
        else
            return transform.position.z < jugador.position.z - margen;
    }
}
