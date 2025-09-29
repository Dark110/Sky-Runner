using UnityEngine;

public class VictoriaJuego : MonoBehaviour
{
    public MenuVictoria menuVictoria; // Hacemos la referencia de menu de victoria
    public Temporizador contador;     // Hacemos la referencia al script del Temporizador

    void Update()
    {
        // Si el tiempo llegó a 0 y el jugador sigue vivo
        if (contador.tiempoActual <= 0 && contador.jugadorVivo)
        {
            VictoriaPartida();
        }
    }

    void VictoriaPartida()
    {
        menuVictoria.menuvictoria(); // Función para llamar la victoria
        this.enabled = false;
    }
}