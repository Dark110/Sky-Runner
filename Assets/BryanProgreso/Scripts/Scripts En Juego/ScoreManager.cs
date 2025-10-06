using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        currentScore += value;

        if (scoreText != null)
            scoreText.text = "Puntos: " + currentScore;
    }

    public int GetScore()
    {
        return currentScore;
    }
}
