using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
        GameStateTracker.LastLevel = "SkyRunnerM";
        SceneManager.LoadScene("SkyRunnerM");
    }

    public void Nivel1()
    {
        GameStateTracker.LastLevel = "Gameplay";
        SceneManager.LoadScene("Gameplay");
    }

    public void Nivel2()
    {
        GameStateTracker.LastLevel = "Gameplay Lvl 2";
        SceneManager.LoadScene("Gameplay Lvl 2");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuInicio");
    }

    public void Tutorial()
    {
        GameStateTracker.LastLevel = "SkyTutorial";
        SceneManager.LoadScene("SkyTutorial");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}
