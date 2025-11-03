using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string nombreEscena = "Derrota"; // Escena que se carga al perder
    public float retrasoGameOver = 2f;      // Tiempo antes de cambiar de escena

    private bool gameOver = false;

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

        // Guardar score si hay SaveDataManager
        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        // Pausar tiempo y audio
        Time.timeScale = 0f;
        AudioListener.pause = true;

        // Desactivar control del jugador
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.PauseWithoutMenu();
        else
        {
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // Iniciar cambio con retraso
        StartCoroutine(CambiarEscenaConRetraso());
    }

    private IEnumerator CambiarEscenaConRetraso()
    {
        // Espera en tiempo real sin afectar Time.timeScale
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        // Restaurar tiempo y audio antes de cambiar de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Notificar GameOver al MenuGameManager
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        // Cargar escena de Game Over
        SceneManager.LoadScene(nombreEscena);

        // Esperar un frame para asegurar EventSystem funcional
        yield return null;
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null)
            eventSystem.enabled = true;
    }
}
