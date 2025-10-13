using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float variacionLateral = 0.4f;
    public float variacionVertical = 0.2f;

    [HideInInspector] public float objetivoZ; // hacia dónde deben avanzar (pasan al jugador)
    private Vector3 direccion;

    void Start()
    {
        // Pequeña variación de dirección inicial
        direccion = new Vector3(
            Random.Range(-variacionLateral, variacionLateral),
            Random.Range(-variacionVertical, variacionVertical),
            1f // siempre avanzan hacia adelante en +Z (pasan al jugador)
        ).normalized;
    }

    void Update()
    {
        // Movimiento constante
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        // Si ya pasó mucho el objetivoZ, la nube dejará de renderizarse
        if (transform.position.z > objetivoZ)
        {
            // No la destruimos aquí (el spawner lo hace), solo opcionalmente la volvemos invisible si quieres
        }
    }
}
