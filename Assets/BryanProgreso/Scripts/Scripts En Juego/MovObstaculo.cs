using UnityEngine;

public class MovObstaculo : MonoBehaviour
{
    [Header("Rango de Velocidad (Movimiento)")]
    public float VelMin = 3f; // mínima
    public float VelMax = 8f; // máxima

    private float VelObstaculo;
    public bool moverAdelante = true; // true = hacia adelante (Z+), false = hacia atrás (Z-)

    [Header("Rango de Velocidad de Rotación")]
    public float RotMin = 30f; // grados por segundo mínimo
    public float RotMax = 120f; // grados por segundo máximo

    private Vector3 velocidadRotacion;

    void Start()
    {
        // Velocidad lineal aleatoria
        VelObstaculo = Random.Range(VelMin, VelMax);

        // Rotación aleatoria en cada eje
        velocidadRotacion = new Vector3(
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax),
            Random.Range(-RotMax, RotMax)
        );

        // Normalizar si quieres que no queden "quietos" en un solo eje
        if (velocidadRotacion == Vector3.zero)
        {
            velocidadRotacion = Vector3.up * Random.Range(RotMin, RotMax);
        }
    }

    void Update()
    {
        // Movimiento lineal
        float direccion = moverAdelante ? 1f : -1f;
        transform.Translate(Vector3.forward * direccion * VelObstaculo * Time.deltaTime, Space.World);

        // Rotación en su propio eje
        transform.Rotate(velocidadRotacion * Time.deltaTime, Space.Self);

        // Destruir el objeto si sale del rango visible
        if (transform.position.z > 60f || transform.position.z < -60f)
        {
            Destroy(gameObject);
        }
    }
}
