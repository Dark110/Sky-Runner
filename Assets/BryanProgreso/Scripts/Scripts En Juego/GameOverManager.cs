using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public static class GameStateTracker
{
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

    private MovimientoParacaidista jugadorScript;

    private void Start()
    {
        CrearOverlayNegro();

        jugadorScript = GetComponent<MovimientoParacaidista>();
        if (jugadorScript == null)
        {
            jugadorScript = FindFirstObjectByType<MovimientoParacaidista>();
        }
    }

    // ... (CrearOverlayNegro se mantiene igual) ...
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
        if (other.CompareTag("Obstaculo"))
        {
            if (jugadorScript != null && jugadorScript.invulnerable)
            {
                Debug.Log("<color=lime>Colisión ignorada por invulnerabilidad.</color>");
                Destroy(other.gameObject);
                return;
            }
            TriggerGameOver();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstaculo"))
        {
            if (jugadorScript != null && jugadorScript.invulnerable)
            {
                Debug.Log("<color=lime>Colisión ignorada por invulnerabilidad.</color>");
                Destroy(hit.gameObject);
                return;
            }
            TriggerGameOver();
        }
    }

    // ⭐ Este método público maneja TODO el proceso de Game Over, incluyendo el fade.
    public void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;
        Debug.Log($"Game Over activado. Nivel: {GameStateTracker.LastLevel}");

        if (SaveDataManager.Instance != null) SaveDataManager.Instance.EndGame();

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
        float t = 0f;
        while (t < retrasoGameOver)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / retrasoGameOver;
            if (fadeOverlay != null) fadeOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        if (fadeOverlay != null) fadeOverlay.color = Color.black;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(nombreEscenaDerrota);
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

    public void JugarDeNuevo()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(GameStateTracker.LastLevel)) SceneManager.LoadScene(GameStateTracker.LastLevel);
        else SceneManager.LoadScene("MenuInicio");
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}