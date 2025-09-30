using UnityEngine;

public class GuardarDatos : MonoBehaviour
{
    public int coins;
    public int score;

    public int highScore;
    public int GameCoins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = PlayerPrefs.GetInt("coins", 0);
        highScore = PlayerPrefs.GetInt("hs", 0);
    }

    void ModifyCoins(int toAdd)
    {
        coins += toAdd;
        PlayerPrefs.SetInt("coins", coins);
    }

    public void SetHighScore(int Hiscore)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("hs", highScore);
            Debug.Log("new higscore reached: " + highScore);
            return;
        }
        Debug.Log("Score was not enough to be higscore");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ModifyCoins(GameCoins);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SetHighScore(highScore);
        }
    }
}
