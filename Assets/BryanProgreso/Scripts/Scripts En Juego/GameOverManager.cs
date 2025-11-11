using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class GameStateTracker
{
    public static string LastLevel = "Gameplay"; // Valor por defecto
}

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

        // 🟢 Guardar puntaje global (HighScore)
        if (SaveDataManager.Instance != null)
        {
            SaveDataManager.Instance.EndGame(); // guarda current y actualiza HighScore global
            Debug.Log("[GameOverManager] Puntaje guardado y actualizado correctamente.");
        }

        // 🟢 Guardar la escena actual para "Jugar de nuevo"
        GameStateTracker.LastLevel = SceneManager.GetActiveScene().name;

        // 🟠 Pausar tiempo y audio
        Time.timeScale = 0f;
        AudioListener.pause = true;

        // 🔴 Desactivar control del jugador
        if (MenuGameManager.Instance != null)
        {
            MenuGameManager.Instance.PauseWithoutMenu();
        }
        else
        {
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // 🟣 Iniciar transición con retraso
        StartCoroutine(CambiarEscenaConRetraso());
    }

    private IEnumerator CambiarEscenaConRetraso()
    {
        // Espera en tiempo real (sin afectar Time.timeScale)
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        // Restaurar tiempo y audio antes de cambiar de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Notificar al menú si existe
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        // Cargar escena de Game Over
        SceneManager.LoadScene(nombreEscena);

        // Esperar un frame para asegurar el EventSystem funcional
        yield return null;
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null)
            eventSystem.enabled = true;
    }

    // 🟢 Botón "Jugar de nuevo" — se asigna en el botón del menú Game Over
    public void JugarDeNuevo()
    {
        string nivel = GameStateTracker.LastLevel;
        if (!string.IsNullOrEmpty(nivel))
        {
            Debug.Log($"[GameOverManager] Reiniciando nivel: {nivel}");
            SceneManager.LoadScene(nivel);
        }
        else
        {
            Debug.LogWarning("[GameOverManager] Nivel anterior no encontrado, cargando 'Gameplay' por defecto.");
            SceneManager.LoadScene("Gameplay");
        }
    }
}
