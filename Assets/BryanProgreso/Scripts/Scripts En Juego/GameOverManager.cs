using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text gameOverText;
    public string nombreMenu = "MenuInicio";

    private bool gameOver = false;

    private void Start()
    {
        if (gameOverText != null)
            gameOverText.enabled = false; // Oculta el texto al iniciar
    }

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
