using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;            // Velocidad de movimiento general
    public float NivelSuavidad = 2f;   // (Opcional) Suavidad en el control
    public float IzqDer = 0f;          // Valor que recoge del input

    private Vector3 JugadorMov;

    private bool ladoIzquierda = false;
    private bool ladoDerecho = false;

    private void Update()
    {
#if UNITY_EDITOR
        // Movimiento con teclado (A-D o flechas izquierda/derecha)
        IzqDer = Input.GetAxis("Horizontal");
#else
        // Movimiento con acelerómetro en móvil
        IzqDer = Input.acceleration.x;
#endif

        // Guardar el movimiento en vector (solo en X)
        JugadorMov = new Vector3(IzqDer, 0, 0);

        // Aplicar el movimiento multiplicado por Vel
        transform.Translate(JugadorMov * Vel * Time.deltaTime);
    }

    public void izquierda(bool lado)
    {
        ladoIzquierda = lado;
    }

    public void derecha(bool lado)
    {
        ladoDerecho = lado;
    }
}