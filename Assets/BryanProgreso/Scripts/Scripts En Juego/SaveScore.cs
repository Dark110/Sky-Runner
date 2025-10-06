using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance;

    public int highScore;
    public int scoreInGame;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void EndGame()
    {
        if (ScoreManager.Instance != null)
            scoreInGame = ScoreManager.Instance.GetScore();
        else
            scoreInGame = 0;

        // Actualiza highscore si corresponde
        if (scoreInGame > highScore)
        {
            highScore = scoreInGame;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        PlayerPrefs.Save(); // Asegura que se guarde
        Debug.Log($"[SaveDataManager] Score guardado: {scoreInGame}, HighScore: {highScore}");
    }
}
