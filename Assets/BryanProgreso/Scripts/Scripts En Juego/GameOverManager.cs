using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public string nombreEscena = "Derrota"; // Escena que se carga al perder
    private bool gameOver = false;

    // Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;
        if (other.CompareTag("Obstaculo"))
        {
            gameOver = true;

            // Guardar score antes de cambiar de escena
            if (SaveDataManager.Instance != null)
                SaveDataManager.Instance.EndGame();

            SceneManager.LoadScene(nombreEscena);
        }
    }

    // Colisión directa
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameOver) return;
        if (hit.gameObject.CompareTag("Obstaculo"))
        {
            gameOver = true;

            if (SaveDataManager.Instance != null)
                SaveDataManager.Instance.EndGame();

            SceneManager.LoadScene(nombreEscena);
        }
    }
}
