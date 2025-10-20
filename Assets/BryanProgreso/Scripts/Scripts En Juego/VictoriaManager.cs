using UnityEngine;
using TMPro;

public class FinalManager : MonoBehaviour
{
    public TextMeshProUGUI resultadoText;

    void Start()
    {
        if (SaveDataManager.Instance != null)
        {
            int finalScore = SaveDataManager.Instance.scoreInGame;
            int highScore = SaveDataManager.Instance.highScore;

            resultadoText.text =
                "Tu puntaje: " + finalScore + "\n" +
                "Mejor puntaje: " + highScore;

            Debug.Log($"[FinalManager] Mostrando Score: {finalScore}, HighScore: {highScore}");
        }
        else
        {
            resultadoText.text = "Score no disponible";
        }
    }
}
