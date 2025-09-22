using UnityEngine;
using TMPro; // Necesario si usas TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceso fácil
    private int score = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }
}
