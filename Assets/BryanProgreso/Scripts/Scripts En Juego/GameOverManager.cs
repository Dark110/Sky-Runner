using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text gameOverText; // Asigna el objeto TMP_Text desde el Inspector
    public string nombreMenu = "MenuInicio"; // Cambia por el nombre de tu escena de menú

    private bool gameOver = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameOver && collision.gameObject.CompareTag("Obstaculo"))
        {
            gameOver = true;
            StartCoroutine(GameOverRoutine());
        }
    }

    IEnumerator GameOverRoutine()
    {
        if (gameOverText != null)
            gameOverText.enabled = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(nombreMenu);
    }
}
