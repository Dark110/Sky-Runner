using UnityEngine;

public class SaveData : MonoBehaviour

{
   public int highScore;    
   public int scoreinGame;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScore = PlayerPrefs.GetInt("hScore", 0);
    }


    public void SaveHighScore(int score)
    {
        if (score > highScore)
        {
        highScore = score;
        PlayerPrefs.SetInt("hScore", highScore);
        Debug.Log ("New High Score: " + highScore);
        return;
    }
        Debug.Log ("score not higher than high score" +score); 
}   
// Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.H))
       {
        SaveHighScore(scoreinGame);
        }
    }
}
