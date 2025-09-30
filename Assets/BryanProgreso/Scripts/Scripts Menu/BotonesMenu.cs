using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
     public void Jugar()
     {

        SceneManager.LoadScene("SkyRunnerM");
     }



    public void Gameplay()
    {

        SceneManager.LoadScene("Gameplay");
    }

    public void Nivel2()
    {

        SceneManager.LoadScene("GameplayLvL2");
    }


    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }

   
     public void Menu()
     {
        SceneManager.LoadScene("MenuInicio");
     }

     public void Salir()
     {
        Application.Quit();
        Debug.Log("Salir del juego");
     }

    public void PuntajeRecordEscena()
    {
        SceneManager.LoadScene("RecordPuntaje");
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