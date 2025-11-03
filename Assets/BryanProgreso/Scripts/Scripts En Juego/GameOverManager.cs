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
        if (gameOver) return; // Evita triggers dobles
        gameOver = true;

        // Guardar score
        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        // Pausar jugabilidad sin tocar Time.timeScale
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.PauseWithoutMenu();
        else
        {
            // Si no hay manager, desactiva jugador manualmente
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // Inicia el retraso antes de cambiar de escena
        StartCoroutine(CambiarEscenaConRetraso());
    }

    private IEnumerator CambiarEscenaConRetraso()
    {
        // Espera en tiempo real sin afectar Time.timeScale
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        // Notificar al MenuGameManager del GameOver
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        // Cargar escena de GameOver
        SceneManager.LoadScene(nombreEscena);
    }
}
