using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaJuego : MonoBehaviour
{
    public Temporizador contador;
    private bool victoriaMostrada = false;

    void Update()
    {
        if (!victoriaMostrada && contador.tiempoActual <= 0 && contador.jugadorVivo)
        {
            VictoriaPartida();
        }
    }

    void VictoriaPartida()
    {
        victoriaMostrada = true;
        Time.timeScale = 1f; // Asegúrate de que el tiempo está normal antes de cambiar de escena
        SceneManager.LoadScene("MenuVictoria");
    }
}
