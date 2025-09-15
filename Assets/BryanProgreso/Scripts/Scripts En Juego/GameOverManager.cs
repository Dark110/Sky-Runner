using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public string nombreEscena = "MenuInicio";
    private bool gameOver = false;

    // Trigger (recomendado para obst�culos en movimiento y triggers)
    private void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;
        if (other.CompareTag("Obstaculo"))
        {
            gameOver = true;
            Debug.Log("Trigger detectado con: " + other.name);
            SceneManager.LoadScene(nombreEscena);
        }
    }

    // Colisi�n directa (por compatibilidad si usas colliders s�lidos)
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameOver) return;
        if (hit.gameObject.CompareTag("Obstaculo"))
        {
            gameOver = true;
            Debug.Log("Controller hit con: " + hit.gameObject.name);
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
