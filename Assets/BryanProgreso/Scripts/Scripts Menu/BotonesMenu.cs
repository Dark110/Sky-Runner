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
}
