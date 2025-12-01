using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScoresData : MonoBehaviour
{
    public void ResetScores()
    {
        //  Borra el highscore global (usado por SaveDataManager)
        PlayerPrefs.DeleteKey("HighScore");
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            PlayerPrefs.DeleteKey("HighScore_" + sceneName);
        }
        if (SaveDataManager.Instance != null)
        {
            SaveDataManager.Instance.scoreInGame = 0;
            SaveDataManager.Instance.highScore = 0;
            SaveDataManager.Instance.ResetHighScore(); 
        }
        if (ScoreManager.Instance != null)
        {
            int current = ScoreManager.Instance.GetScore();
            ScoreManager.Instance.AddScore(-current);
        }
        PlayerPrefs.Save();
        Debug.Log("Scores y HighScores reiniciados correctamente.");

    }
}
