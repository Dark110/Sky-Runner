using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnPlayButton()
    {
     
        SceneManager.LoadScene("SkyRunner");
    }

    public void OnOptionsButton()
    {
        SceneManager.LoadScene("Opciones");
    }
    
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}
