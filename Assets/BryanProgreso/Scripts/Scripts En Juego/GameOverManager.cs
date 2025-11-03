using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public string nombreEscena = "Derrota";
    public float retrasoGameOver = 2f;
    public float duracionFade = 1f;
    public Image fadeOverlay;
    private bool gameOver = false;

    private void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;
        if (other.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameOver) return;
        if (hit.gameObject.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        gameOver = true;

        // Guardar el score antes de cambiar de escena
        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        // Iniciar el fade (paralelo)
        if (fadeOverlay != null)
            StartCoroutine(FadeOut());

        // Iniciar el temporizador de escena
        StartCoroutine(CargarEscenaRetraso());
    }

    private IEnumerator CargarEscenaRetraso()
    {
        // Pausa el tiempo del juego
        Time.timeScale = 0f;

        // Fade en paralelo
        if (fadeOverlay != null)
            StartCoroutine(FadeOut());

        // Espera el retraso del Game Over (en tiempo real)
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        // Restaura antes del cambio de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Informa al MenuGameManager (si existe)
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        // Carga la escena
        SceneManager.LoadScene(nombreEscena);
    }

    private IEnumerator FadeOut()
    {
        if (fadeOverlay == null)
            yield break;

        Color color = fadeOverlay.color;
        float t = 0f;

        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime / duracionFade;
            color.a = Mathf.Lerp(0f, 0.6f, t);
            fadeOverlay.color = color;
            yield return null;
        }
    }
}
