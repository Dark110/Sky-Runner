using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
        CrearOverlayNegro();
    }

    private void CrearOverlayNegro()
    {
        GameObject canvasExistente = GameObject.Find("FadeCanvas");
        if (canvasExistente != null)
        {
            Destroy(canvasExistente);
        }

        fadeCanvasObj = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // Capa superior

        fadeCanvasObj.AddComponent<CanvasGroup>(); // Ayuda a manejar la transparencia

        // Mantener este objeto al cambiar de escena para que el fade se vea continuo
        DontDestroyOnLoad(fadeCanvasObj);

        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(fadeCanvasObj.transform, false);

        fadeOverlay = imgObj.AddComponent<Image>();
        // Empieza totalmente transparente
        fadeOverlay.color = new Color(0f, 0f, 0f, 0f);

        // Configurar para que ocupe toda la pantalla
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

        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;

        AudioListener.pause = true;
        Time.timeScale = 0f; // Congelar juego

        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.PauseWithoutMenu();
        else
        {
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        StartCoroutine(SecuenciaMuerte());
    }

    // Unifiqué las corrutinas para tener mejor control del flujo
    private IEnumerator SecuenciaMuerte()
    {
        // fade
        float t = 0f;
        while (t < retrasoGameOver)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / retrasoGameOver;
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Negro total
        if (fadeOverlay != null) fadeOverlay.color = Color.black;

        // Restaurar tiempo y lógica antes de cambiar escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        // 3. Cambiar Escena
        SceneManager.LoadScene(nombreEscena);

        t = 0f;
        while (t < 1.5f) // Duración del aclarado (1.5 seg)
        {
            t += Time.unscaledDeltaTime;
            float alpha = 1f - (t / 1.5f); // Inverso: de 1 a 0
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Destruir el Canvas ya que terminamos
        if (fadeCanvasObj != null)
            Destroy(fadeCanvasObj);

        // Restaurar EventSystem si es necesario para que funcionen los botones
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null) eventSystem.enabled = true;
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