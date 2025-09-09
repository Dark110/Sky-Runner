using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;            
    public float NivelSuavidad = 2f;  
    public float IzqDer = 0f;
    public float ArribaAbajo = 0f;

    private Vector3 JugadorMov;

    private bool ladoIzquierda = false;
    private bool ladoDerecho = false;
    private bool arriba = false;
    private bool abajo = false;

    private void Update()
    {
#if UNITY_EDITOR
        // Movimiento con teclado (A-D o flechas izquierda/derecha)
        IzqDer = Input.GetAxis("Horizontal");
        ArribaAbajo = Input.GetAxis("Vertical");
#else
        // Movimiento con acelerómetro en móvil
        IzqDer = Input.acceleration.x;
        ArribaAbajo = Input.acceleration.Y;
#endif

        // Guardar el movimiento en vector (solo en X)
        JugadorMov = new Vector3(IzqDer, ArribaAbajo, 0);

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

    public void Arriba(bool SubeBaja)
    {
        arriba = SubeBaja;
    }

    public void Abajo(bool SubeBaja)
    {
        abajo = SubeBaja;
    }
}