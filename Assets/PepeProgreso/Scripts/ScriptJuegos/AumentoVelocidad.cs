using UnityEngine;

public class AumentoVelocidad : MonoBehaviour
{
    public float AumentoVel = 2f;
    public float DuracionPowerUP = 5f;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Colisión detectada con: " + other.name); //Para verficar si funciona en consola
        // se activa al power up cuando el jugador lo toca
        MovimientoJugador Jugador = other.GetComponent<MovimientoJugador>();
        if (Jugador != null)
        {
            // con esta lineas de codigo se activa el aumento de velocidad 
            Jugador.ActPowerUP(AumentoVel, DuracionPowerUP);
            Destroy(gameObject); // se desactiva el powerup
        }
    }
}