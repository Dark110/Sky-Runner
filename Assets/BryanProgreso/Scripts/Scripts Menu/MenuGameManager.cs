using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    [Header("Objetos a pausar")]
    public MovimientoParacaidista jugador;      // tu script de movimiento
    public GameObject[] spawners;               // spawners de dianas u otros
    public Animator[] animators;                // animators a pausar si aplica
    public MonoBehaviour[] scriptsExtras;       // otros scripts que quieras pausar

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

    private void Update()
    {
        // Permite pausar y reanudar con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.PLAY)
                OnPausePressed();
            else if (currentState == GameState.PAUSE)
                OnResumePressed();
        }
    }

    private void ApplyGameState(GameState state)
    {
        currentState = state;

        switch (state)
        {
            case GameState.PLAY:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                if (jugador) jugador.enabled = true;
                SetActiveForObjects(spawners, true);
                SetEnabledForAnimators(animators, true);
                SetEnabledForScripts(scriptsExtras, true);
                break;

            case GameState.PAUSE:
            case GameState.GAMEOVER:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                if (jugador) jugador.enabled = false;
                SetActiveForObjects(spawners, false);
                SetEnabledForAnimators(animators, false);
                SetEnabledForScripts(scriptsExtras, false);
                break;
        }

        OnGameStateChanged?.Invoke(state);
    }

    private void SetActiveForObjects(GameObject[] objects, bool active)
    {
        if (objects == null) return;
        foreach (var obj in objects)
            if (obj) obj.SetActive(active);
    }

    private void SetEnabledForAnimators(Animator[] anims, bool enabled)
    {
        if (anims == null) return;
        foreach (var a in anims)
            if (a) a.enabled = enabled;
    }

    private void SetEnabledForScripts(MonoBehaviour[] scripts, bool enabled)
    {
        if (scripts == null) return;
        foreach (var s in scripts)
            if (s) s.enabled = enabled;
    }

    // 🔹 Funciones de botones (UI)
    public void OnPausePressed()
    {
        ApplyGameState(GameState.PAUSE);
    }

    public void OnResumePressed()
    {
        ApplyGameState(GameState.PLAY);
    }

    public void OnResetPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
