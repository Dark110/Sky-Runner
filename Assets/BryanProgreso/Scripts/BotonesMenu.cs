using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
     
        SceneManager.LoadScene("SkyRunner");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }
    
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}
