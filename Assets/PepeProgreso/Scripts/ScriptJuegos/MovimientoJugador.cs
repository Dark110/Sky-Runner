using UnityEngine;
using System.Collections; // Necesario para IEnumerator

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;
    public float NivelSuavidad = 2f;
    public float IzqDer = 0f;
    public float ArribaAbajo = 0f;
    private float VelActual; // Esta variable te va permitir cambiar la velocidad al tocar un power up de velocidad

    private Vector3 JugadorMov;

    private bool ladoIzquierda = false;
    private bool ladoDerecho = false;
    private bool arriba = false;
    private bool abajo = false;

    private void Start()
    {
        VelActual = Vel;
    }

    private void Update()
    {
#if UNITY_EDITOR
        // Movimiento con teclado (A-D o flechas izquierda/derecha)
        IzqDer = Input.GetAxis("Horizontal");
        ArribaAbajo = Input.GetAxis("Vertical");
#else
        // Movimiento con acelerómetro en móvil
        IzqDer = Input.acceleration.x;
        ArribaAbajo = Input.acceleration.y; // minúscula
#endif

        // Guardar el movimiento en vector (solo en X)
        JugadorMov = new Vector3(IzqDer, ArribaAbajo, 0);

        // Aplicar el movimiento multiplicado por VelActual
        transform.Translate(JugadorMov * VelActual * Time.deltaTime);
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

    public void ActPowerUP(float aumento_vel, float duracion_Powerup)
    {
        StartCoroutine(PowerVelocidad(aumento_vel, duracion_Powerup)); // corregido StartCoroutine
    }

    private IEnumerator PowerVelocidad(float MulVel, float TiempoPower)
    {
        VelActual = Vel * MulVel; // mayor velocidad al jugador
        yield return new WaitForSeconds(TiempoPower);
        VelActual = Vel; // se desactiva el power up
    }
}