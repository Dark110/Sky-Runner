using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoria : MonoBehaviour
{
    public GameObject UI_Victoria; // Panel del menú de victoria

    void Start()
    {
        UI_Victoria.SetActive(false); //Se oculta el inicio
    }

    //Esta funcion se llama cuando el jugador gana
    public void menuvictoria()
    {
        UI_Victoria.SetActive(true);
        Time.timeScale = 0f; // Pausa
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

    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;
        int siguiente = nivelActual + 1;

        if (siguiente < totalEscenas)
        {
            // Cargar siguiente nivel
            SceneManager.LoadScene(siguiente);
        }
        else
        {
            // Si ya no hay más niveles, volver al menú (índice 0)
            Debug.Log("No hay más niveles, regresando al menú principal.");
            SceneManager.LoadScene(0);
        }
    }
}