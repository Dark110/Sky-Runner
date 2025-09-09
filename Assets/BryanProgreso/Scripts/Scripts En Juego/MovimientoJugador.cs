using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;            // Velocidad de movimiento general
    public float NivelSuavidad = 2f;   // (Opcional) Suavidad en el control
    public float IzqDer = 0f;          // Valor que recoge del input

    private Vector3 JugadorMov;
    private CharacterController movimiento;

    private void Awake()
    {
        movimiento = GetComponent<CharacterController>();
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        IzqDer = -Input.GetAxis("Horizontal"); // Invertir el eje si lo necesitas
#else
        IzqDer = Input.acceleration.x;
#endif
        JugadorMov = new Vector3(IzqDer, 0, 0);
    }

    private void FixedUpdate()
    {
        // Mueve solo en X, ajusta la velocidad
        movimiento.Move(JugadorMov * Vel * Time.fixedDeltaTime);
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