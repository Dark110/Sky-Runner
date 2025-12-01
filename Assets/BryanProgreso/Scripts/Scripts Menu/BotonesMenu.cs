using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario si vas a usar CheckNivel2

public class MenuManager : MonoBehaviour
{
    // Nombre de la clave para PlayerPrefs
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";
    private const string LAST_LEVEL_KEY = "LastCompletedLevel";

    [Header("Referencias UI (Opcional)")]
    // Asigna el botón del Nivel 2 aquí en el Inspector (si es necesario)
    public Button nivel2Button;

    void Start()
    {
        if (nivel2Button != null)
        {
            CheckNivel2();
        }
    }
    // Lógica del Botón Jugar (Jugar por Primera Vez / Nivel 1)

    public void Jugar()
    {
        // 1. Verificar si el tutorial ya fue completado (0 = no visto, 1 = visto)
        int tutorialVisto = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0);

        if (tutorialVisto == 0)
        {
            // PRIMERA VEZ: Ir al tutorial
            GameStateTracker.LastLevel = "SkyTutorial";
            SceneManager.LoadScene("SkyTutorial");
        }
        else
        {
            GameStateTracker.LastLevel = "SkyRunnerM";
            SceneManager.LoadScene("SkyRunnerM");
        }
    }

    // Llamado desde el FINAL del Tutorial
    public void CompletarTutorial()
    {
        // Guarda 1 para indicar que el tutorial ya se completó
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
        PlayerPrefs.Save(); // Asegura que se escriba en el disco

        // Carga la escena de selección de niveles o el Nivel 1
        GameStateTracker.LastLevel = "Lvl1";
        SceneManager.LoadScene("Lvl1");
    }


    // Botón de Reinicio (Ver Tutorial de Nuevo)
    public void ReiniciarTutorial()
    {
        GameStateTracker.LastLevel = "SkyTutorial";
        SceneManager.LoadScene("SkyTutorial");
    }
    // Control de Niveles Completados
    public static void MarcarNivelCompletado(int nivel)
    {
        // El nivel completado se guarda en PlayerPrefs.
        // Ejemplo: Si completas Nivel 1, guarda el valor 1.
        int nivelActualCompletado = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 0);

        // Solo guarda el nivel si es mayor que el nivel previamente guardado
        if (nivel > nivelActualCompletado)
        {
            PlayerPrefs.SetInt(LAST_LEVEL_KEY, nivel);
            PlayerPrefs.Save();
            Debug.Log($"Nivel {nivel} completado y guardado.");
        }
    }

    // Deshabilita el botón del Nivel 2 si el Nivel 1 no se ha completado
    public void CheckNivel2()
    {
        int nivelCompletado = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 0);

        if (nivelCompletado < 2)
        {
            if (nivel2Button != null)
            {
                nivel2Button.interactable = false;
            }
        }
        else
        {
            if (nivel2Button != null)
            {
                nivel2Button.interactable = true;
            }
        }
    }

    public void Nivel1()
    {
        GameStateTracker.LastLevel = "Lvl1";
        SceneManager.LoadScene("Lvl1");
    }

    public void Nivel2()
    {
        // Doble verificación por si el jugador intenta hacer trampa
        int nivelCompletado = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 0);
        if (nivelCompletado < 1) return;

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

    // Función para el botón del menú: Ver Tutorial
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