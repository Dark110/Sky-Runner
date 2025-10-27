using System;
using System.Collections.Generic;
using UnityEngine;
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
    public static MenuGameManager Instance
    {
        get
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
    }

    public event Action<GameState> OnGameStateChanged;

    [Header("Estado inicial")]
    public GameState initialState = GameState.PLAY;

    [Header("Referencias a pausar")]
    public MovimientoParacaidista jugador;
    public Animator[] animators;
    public MonoBehaviour[] scriptsExtras;

    private GameState currentState;
    public GameState CurrentState => currentState;

    private List<MonoBehaviour> spawners = new List<MonoBehaviour>();
    private bool needsRefresh = true;

    private void Awake()
    {
        // Evita duplicados, pero NO persiste entre escenas
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        currentState = initialState;
        RefreshReferences();
        ApplyGameState(currentState);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // Refresca referencias si es necesario
        if (SceneManager.GetActiveScene().name == "Gameplay" && (needsRefresh || jugador == null))
        {
            RefreshReferences();
            needsRefresh = false;
        }

        // Pausa manual (solo para pruebas)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.PLAY)
                OnPausePressed();
            else if (currentState == GameState.PAUSE)
                OnResumePressed();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            needsRefresh = true;
            currentState = GameState.PLAY;
            ApplyGameState(currentState);
        }
        else
        {
            jugador = null;
        }
    }

    private void RefreshReferences()
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
            return;

        // Jugador
        jugador = FindFirstObjectByType<MovimientoParacaidista>();

        // Animators
        animators = FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // Spawners
        spawners.Clear();
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var mb in allObjects)
        {
            if (mb != null && mb.gameObject.CompareTag("Spawner"))
                spawners.Add(mb);
        }
    }

    private void ApplyGameState(GameState state)
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene != "Gameplay" && state != GameState.GAMEOVER)
            return;

        if (scene == "Gameplay" && (needsRefresh || jugador == null))
        {
            RefreshReferences();
            needsRefresh = false;
        }

        currentState = state;

        bool enable = state == GameState.PLAY;
        Time.timeScale = (state == GameState.PLAY) ? 1f : 0f;
        AudioListener.pause = !enable;

        if (jugador) jugador.enabled = enable;
        SetEnabledForAnimators(animators, enable);
        SetEnabledForScripts(scriptsExtras, enable);
        SetEnabledForScripts(spawners.ToArray(), enable);

        OnGameStateChanged?.Invoke(state);
    }

    private void SetEnabledForScripts(MonoBehaviour[] scripts, bool enabled)
    {
        if (scripts == null) return;
        foreach (var s in scripts)
            if (s) s.enabled = enabled;
    }

    private void SetEnabledForAnimators(Animator[] anims, bool enabled)
    {
        if (anims == null) return;
        foreach (var a in anims)
            if (a) a.enabled = enabled;
    }

    // 
    public void OnPausePressed() => ApplyGameState(GameState.PAUSE);
    public void OnResumePressed() => ApplyGameState(GameState.PLAY);
    public void OnGameOver() => ApplyGameState(GameState.GAMEOVER);

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

    public void ForceRefreshReferences() => needsRefresh = true;
}
