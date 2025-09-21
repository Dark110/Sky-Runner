using UnityEngine;

public class Invuneralidad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MovimientoJugador Player = other.GetComponent<MovimientoJugador>();

        // El if cuando se activa el power up
        if (Player != null)
        {
            Player.ActivarPowerUP_Inv();
            Destroy(gameObject); // Se destruye el Power UP
        }
    }
}