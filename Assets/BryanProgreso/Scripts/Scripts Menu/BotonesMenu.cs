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
        GameStateTracker.LastLevel = "Lvl1";
        SceneManager.LoadScene("Lvl1");
    }

    public void Nivel2()
    {
        GameStateTracker.LastLevel = "Lvl2";
        SceneManager.LoadScene("Lvl2");
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
