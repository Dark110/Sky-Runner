using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoria : MonoBehaviour
{
    public GameObject UI_Victoria; // Panel del menú de victoria

    //Esta funcion se llama cuando el jugador gana
    public void menuvictoria()
    {
        if (UI_Victoria != null)
        {
            UI_Victoria.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
        else
        {
            Debug.LogWarning("UI_Victoria no está asignado en el inspector.");
        }
    }

    // Botón: Reiniciar partida
    public void ReinicioPartida()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botón: Ir al menú principal
    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    // Botón: Ir al menú de niveles
    public void MenuNVLS()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuNiveles");
    }

    // Botón: Ir al siguiente nivel
    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nivelActual + 1);
    }
}