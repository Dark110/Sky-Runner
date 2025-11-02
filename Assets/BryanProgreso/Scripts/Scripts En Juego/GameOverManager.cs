using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public string nombreEscena = "Derrota"; // Escena que se carga al perder
    public float retrasoGameOver = 2f;
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

            StartCoroutine(CargarEscenaRetraso());
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

            StartCoroutine(CargarEscenaRetraso());
        }
    }

    private IEnumerator CargarEscenaRetraso()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(retrasoGameOver);
        Time.timeScale = 1f;

        SceneManager.LoadScene(nombreEscena);
    }
}