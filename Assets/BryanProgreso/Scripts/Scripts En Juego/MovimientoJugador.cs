using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;            // Velocidad de movimiento general
    public float NivelSuavidad = 2f;   // (Opcional) Suavidad en el control
    public float IzqDer = 0f;          // Valor que recoge del input

    private Vector3 JugadorMov;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Bloquea el movimiento en Y y Z, y todas las rotaciones
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        IzqDer = -Input.GetAxis("Horizontal"); // Invertir el eje
#else
        IzqDer = Input.acceleration.x;
#endif
        JugadorMov = new Vector3(IzqDer, 0, 0);
    }

    private void FixedUpdate()
    {
        // Solo mueve en X, mantiene Y y Z fijos
        Vector3 nuevaPos = rb.position + new Vector3(JugadorMov.x, 0, 0) * Vel * Time.fixedDeltaTime;
        rb.MovePosition(nuevaPos);
    }

    public void izquierda(bool lado)
    {
        // Para controles táctiles si los usas
    }

    public void derecha(bool lado)
    {
        // Para controles táctiles si los usas
    }
}