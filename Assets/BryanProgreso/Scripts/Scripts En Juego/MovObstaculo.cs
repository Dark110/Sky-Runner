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
    }

    void Update()
    {
        float direccion = moverAdelante ? 1f : -1f;
        transform.Translate(Vector3.forward * direccion * VelObstaculo * Time.deltaTime, Space.World);

        transform.Rotate(velocidadRotacion * Time.deltaTime, Space.Self);

        if (transform.position.z > 60f || transform.position.z < -60f)
        {
            // Antes de destruir, damos puntos
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(puntosAlEsquivar);
            }

            Destroy(gameObject);
        }
    }
}
