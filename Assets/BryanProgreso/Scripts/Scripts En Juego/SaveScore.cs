using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance;

    [Header("Datos de juego")]
    public int scoreInGame;
    public int highScore;

    private void Awake()
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

    private void Start()
    {
        // Cargar HighScore global al iniciar
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Fin de partida: actualizar HighScore global
    public void EndGame()
    {
        if (ScoreManager.Instance != null)
            scoreInGame = ScoreManager.Instance.GetScore();
        else
            scoreInGame = 0;

        // Actualiza HighScore global si corresponde
        if (scoreInGame > highScore)
        {
            highScore = scoreInGame;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        PlayerPrefs.Save();
        Debug.Log($"[SaveDataManager] Score: {scoreInGame}, HighScore: {highScore}");
    }

    // Resetear HighScore global
    public void ResetHighScore()
    {
        scoreInGame = 0;
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        Debug.Log("[SaveDataManager] HighScore reiniciado a 0");
    }
}
