using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Clase auxiliar para guardar datos entre escenas sin crear un Singleton complejo
public static class GameStateTracker
{
    public static string LastLevel = "Gameplay";
}

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string nombreEscena = "Derrota";
    private float retrasoGameOver = 2.5f;

    private bool gameOver = false;
    private Image fadeOverlay;
    private GameObject fadeCanvasObj;

    private void Start()
    {
        // Creamos el fade negro programáticamente para no depender de prefabs en cada nivel
        CrearOverlayNegro();
    }

    private void CrearOverlayNegro()
    {
        // Limpieza preventiva por si quedó uno de la escena anterior
        GameObject canvasExistente = GameObject.Find("FadeCanvas");
        if (canvasExistente != null)
        {
            Destroy(canvasExistente);
        }

        fadeCanvasObj = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // Capa superior para tapar todo

        fadeCanvasObj.AddComponent<CanvasGroup>();
        fadeCanvasObj.AddComponent<GraphicRaycaster>(); // Importante si hubiera botones, aunque aquí es solo visual

        // Importante: No destruir al cargar para poder hacer el efecto de "Fade In" en la siguiente escena
        DontDestroyOnLoad(fadeCanvasObj);

        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(fadeCanvasObj.transform, false);

        fadeOverlay = imgObj.AddComponent<Image>();
        fadeOverlay.color = new Color(0f, 0f, 0f, 0f); // Invisible al inicio
        fadeOverlay.raycastTarget = false; // Permitir clicks a través mientras es transparente

        // Estirar imagen a toda la pantalla
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

        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        // Guardamos el nombre de la escena actual para el botón "Reintentar"
        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;

        // 1. Notificar al Manager Global
        // Esto desactiva inputs, spawners y animaciones automáticamente
        if (MenuGameManager.Instance != null)
        {
            MenuGameManager.Instance.OnGameOver();
        }
        else
        {
            // Fallback por si acaso no hay manager en la escena
            var jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // 2. Efecto de congelar el juego (opcional, pero da buen feedback de impacto)
        Time.timeScale = 0f;
        AudioListener.pause = true;

        StartCoroutine(SecuenciaMuerte());
    }

    private IEnumerator SecuenciaMuerte()
    {
        // --- FASE 1: Fade Out (Pantalla a Negro) ---
        // Usamos unscaledDeltaTime porque el TimeScale está en 0
        float t = 0f;
        while (t < retrasoGameOver)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / retrasoGameOver;
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Asegurar negro total
        if (fadeOverlay != null) fadeOverlay.color = Color.black;

        // --- FASE 2: Cambio de Escena ---

        // Restaurar tiempo antes de cambiar de escena para evitar bugs en la UI de Derrota
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Cargamos la escena de derrota
        SceneManager.LoadScene(nombreEscena);

        // --- FASE 3: Fade In (Aclarar pantalla en la nueva escena) ---
        t = 0f;
        while (t < 1.5f)
        {
            t += Time.unscaledDeltaTime;
            float alpha = 1f - (t / 1.5f); // De 1 a 0
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // --- FASE 4: Limpieza ---
        if (fadeCanvasObj != null)
            Destroy(fadeCanvasObj);
    }

    public void JugarDeNuevo()
    {
        Time.timeScale = 1f; // Seguridad extra
        string nivel = GameStateTracker.LastLevel;

        if (!string.IsNullOrEmpty(nivel))
            SceneManager.LoadScene(nivel);
        else
            SceneManager.LoadScene("Gameplay");
    }
}