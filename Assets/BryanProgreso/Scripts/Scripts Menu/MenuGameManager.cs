using UnityEngine;
using System;

public enum GameState
{
    PLAY,
    PAUSE,
    GAMEOVER
}

public class MenuGameManager : MonoBehaviour
{
    private static MenuGameManager _instance;

    public static MenuGameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<MenuGameManager>();

            if (_instance == null)
            {
                GameObject go = new GameObject("MenuGameManager");
                _instance = go.AddComponent<MenuGameManager>();
            }
        }

        return _instance;
    }

    public event Action<GameState> OnGameStateChanged;

    [Header("Estado inicial")]
    public GameState initialState = GameState.PLAY;

    private GameState currentState;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        currentState = initialState;
        ApplyGameState(currentState);
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void GameStateChange(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        ApplyGameState(currentState);
        OnGameStateChanged?.Invoke(currentState);
    }

    private void ApplyGameState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAY:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                break;

            case GameState.PAUSE:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;

            case GameState.GAMEOVER:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
        }
    }
}
