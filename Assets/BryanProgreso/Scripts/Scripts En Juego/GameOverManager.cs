using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public static class GameStateTracker
{
    public static string LastLevel = "Gameplay"; // Valor por defecto
}

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string nombreEscena = "Derrota";
    private float retrasoGameOver = 5f;

    private bool gameOver = false;

    // ⭐ Overlay para oscurecer la pantalla
    private Image fadeOverlay;

    private void Start()
    {
        CrearOverlayNegro();
    }

    private void CrearOverlayNegro()
    {
        // Crear un Canvas si no existe
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        canvasObj.AddComponent<CanvasGroup>();
        DontDestroyOnLoad(canvasObj);

        // Crear la imagen negra
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform, false);

        fadeOverlay = imgObj.AddComponent<Image>();
        fadeOverlay.color = new Color(0f, 0f, 0f, 0f); // transparente al inicio

        // Ajustar tamaño
        RectTransform rt = fadeOverlay.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        // Guarda puntaje
        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;

        // Pausar juego
        Time.timeScale = 0f;
        AudioListener.pause = true;

        // Desactivar control
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.PauseWithoutMenu();
        else
        {
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // ⭐ Iniciar oscurecimiento
        StartCoroutine(FadeToBlack());

        StartCoroutine(CambiarEscenaConRetraso());
    }

    // ⭐ Transición visual a negro (modo noche)
    private IEnumerator FadeToBlack()
    {
        float t = 0f;

        while (t < retrasoGameOver)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / retrasoGameOver;

            fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    private IEnumerator CambiarEscenaConRetraso()
    {
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        SceneManager.LoadScene(nombreEscena);

        yield return null;
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null)
            eventSystem.enabled = true;
    }

    public void JugarDeNuevo()
    {
        string nivel = GameStateTracker.LastLevel;

        if (!string.IsNullOrEmpty(nivel))
            SceneManager.LoadScene(nivel);
        else
            SceneManager.LoadScene("Gameplay");
    }
}