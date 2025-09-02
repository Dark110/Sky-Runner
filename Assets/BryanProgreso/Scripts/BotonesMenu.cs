using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    // Inicio del juego
    public void OnPlayButton()
    {
     
        SceneManager.LoadScene("SkyRunner");
    }

    // Abre el menú de opciones
    public void OnOptionsButton()
    {
       
        // Ejemplo: optionsPanel.SetActive(true);
        Debug.Log("Opciones abiertas");
    }

    // Sale de la aplicación
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}
