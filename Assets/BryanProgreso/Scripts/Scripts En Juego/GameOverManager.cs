using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Esta clase estática guarda información que sobrevive al cambio de escenas
public static class GameStateTracker
{
    // Por defecto lo dejamos vacío para obligar a llenarlo dinámicamente
    public static string LastLevel = "";
}

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string nombreEscenaDerrota = "Gameover";
    private float retrasoGameOver = 2.5f;

    private bool gameOver = false;
    private Image fadeOverlay;
    private GameObject fadeCanvasObj;

    private void Start()
    {
        CrearOverlayNegro();
    }

    // ... (Tu código de CrearOverlayNegro se queda igual, lo omito para ahorrar espacio) ...
    private void CrearOverlayNegro()
    {
        GameObject canvasExistente = GameObject.Find("FadeCanvas");
        if (canvasExistente != null) Destroy(canvasExistente);

        fadeCanvasObj = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        fadeCanvasObj.AddComponent<CanvasGroup>();
        DontDestroyOnLoad(fadeCanvasObj);

        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(fadeCanvasObj.transform, false);
        fadeOverlay = imgObj.AddComponent<Image>();
        fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
        fadeOverlay.raycastTarget = false;

        RectTransform rt = fadeOverlay.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstaculo")) TriggerGameOver();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstaculo")) TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        // Guardamos el nombre de la escena ACTUAL ("Nivel 1", "Nivel 2", etc.)
     
        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;

        Debug.Log($"Jugador perdió en el nivel: {GameStateTracker.LastLevel}");

        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        //  Manager Global para pausar lógica
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();
        else
        {
            var jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        Time.timeScale = 0f;
        AudioListener.pause = true;

        StartCoroutine(SecuenciaMuerte());
    }

    private IEnumerator SecuenciaMuerte()
    {
        // Fade
        float t = 0f;
        while (t < retrasoGameOver)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / retrasoGameOver;
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        if (fadeOverlay != null) fadeOverlay.color = Color.black;

        // Restaurar tiempo para la escena de UI
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(nombreEscenaDerrota);

        // Aclarar en la nueva escena
        t = 0f;
        while (t < 1.5f)
        {
            t += Time.unscaledDeltaTime;
            float alpha = 1f - (t / 1.5f);
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        if (fadeCanvasObj != null) Destroy(fadeCanvasObj);
    }

    //Boton reiniciar nivel
    public void JugarDeNuevo()
    {
        Time.timeScale = 1f;

        // Verifica si hay un nivel guardado
        if (!string.IsNullOrEmpty(GameStateTracker.LastLevel))
        {
            // Cargamos ("Nivel 1", etc.)
            SceneManager.LoadScene(GameStateTracker.LastLevel);
        }
        else
        {
            // volvemos al menú principal para evitar errores.
            Debug.LogWarning("No se encontró nivel anterior, volviendo al menú.");
            SceneManager.LoadScene("MenuInicio");
        }
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}