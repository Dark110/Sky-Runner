using UnityEngine;

public class ResetScoresData : MonoBehaviour
{
    /// <summary>
    /// Borra solo los scores y high scores guardados, sin afectar otros PlayerPrefs.
    /// </summary>
    public void ResetScores()
    {
        // Resetea los highscores global y por escena
        PlayerPrefs.DeleteKey("HighScore_Global");

        // Resetea todas las keys de highscore por escena
        foreach (var scene in UnityEngine.SceneManagement.SceneManager.GetAllScenes())
        {
            string key = "HighScore_" + scene.name;
            PlayerPrefs.DeleteKey(key);
        }

        // Reinicia score actual en SaveDataManager
        if (SaveDataManager.Instance != null)
        {
            SaveDataManager.Instance.scoreInGame = 0;
        }

        // Reinicia UI del ScoreManager
        if (ScoreManager.Instance != null)
        {
            int current = ScoreManager.Instance.GetScore();
            ScoreManager.Instance.AddScore(-current);
        }

        PlayerPrefs.Save();
        Debug.Log("Scores y HighScore reiniciados correctamente.");
    }
}
