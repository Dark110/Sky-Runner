using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject MenuPausaUI;
    private bool Pausa = false;

    // Update is called once per frame
    void Update()
    {
        // La pausa que vamos hacer cuando el jugador precione la pausa en juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pausa)
            {
                Reaunudar();
            }
            else
            {
                PausarGame();
            }
        }
    }

    public void Reaunudar()
    {
        MenuPausaUI.SetActive(false);
        Time.timeScale = 1f; // Se raunuda el juego o partida en proceso
        Pausa = false;
    }

    public void PausarGame()
    {
        MenuPausaUI.SetActive(true);
        Time.timeScale = 0f; // Se detiene el tiempo
        Pausa = true; // aquí estaba el error, antes lo tenías en false
    }

    // Se Reinica el tiempo sin cambiar la escena
    public void ReinicioJuego()
    {
        Time.timeScale = 1f; // corregido
        Pausa = false;
        MenuPausaUI.SetActive(false); // corregido
    }

    // El cierre de juego si esta
    public void SalirJuego()
    {
        Application.Quit();
    }
}