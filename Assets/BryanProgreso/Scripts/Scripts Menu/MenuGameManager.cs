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

    [Header("Objetos a pausar")]
    public MovimientoParacaidista jugador;
    public Animator[] animators;
    public MonoBehaviour[] scriptsExtras;

    private GameState currentState;
    public GameState CurrentState => currentState;

    private List<MonoBehaviour> spawners = new List<MonoBehaviour>();
    private bool needsRefresh = true;

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
        RefreshReferences();
        ApplyGameState(currentState);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Gameplay" && (needsRefresh || jugador == null))
        {
            RefreshReferences();
            needsRefresh = false;
        }

        // Control manual de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.PLAY) OnPausePressed();
            else if (currentState == GameState.PAUSE) OnResumePressed();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Siempre reactivar tiempo y audio al cambiar escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

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
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Gameplay") return;

        jugador = FindFirstObjectByType<MovimientoParacaidista>();
        animators = FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        spawners.Clear();
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var mb in allObjects)
        {
            if (mb != null && mb.gameObject.CompareTag("Spawner"))
                spawners.Add(mb);
        }
    }

    private void ApplyGameStateInternal(GameState state, bool notify)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Si no estamos en Gameplay y no es GameOver, no aplicar lógicas de pausa/jugador
        if (currentScene != "Gameplay" && state != GameState.GAMEOVER)
        {
            if (notify) OnGameStateChanged?.Invoke(state);
            return;
        }

        if (currentScene == "Gameplay" && (needsRefresh || jugador == null))
        {
            RefreshReferences();
            needsRefresh = false;
        }

        currentState = state;
        bool enableGameplay = (state == GameState.PLAY);

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
                // 🔸 Muy importante: NO pausar el tiempo ni audio
                Time.timeScale = 1f;
                AudioListener.pause = false;
                enableGameplay = false; // Desactiva control del jugador
                break;
        }

        // Habilitar/deshabilitar jugabilidad
        if (jugador) jugador.enabled = enableGameplay;
        SetEnabledForAnimators(animators, enableGameplay);
        SetEnabledForScripts(scriptsExtras, enableGameplay);
        SetEnabledForScripts(spawners.ToArray(), enableGameplay);

        if (notify)
            OnGameStateChanged?.Invoke(state);
    }

    private void ApplyGameState(GameState state) => ApplyGameStateInternal(state, true);

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

    // ==============================
    // FUNCIONES DE ESTADO Y UI
    // ==============================

    public void OnPausePressed() => ApplyGameState(GameState.PAUSE);
    public void OnResumePressed() => ApplyGameState(GameState.PLAY);
    public void OnGameOver() => ApplyGameState(GameState.GAMEOVER);

    // 🔸 Pausa SIN afectar UI (antes usada por GameOverManager)
    public void PauseWithoutMenu()
    {
        // En lugar de pausar el tiempo, solo desactiva control del jugador
        currentState = GameState.PAUSE;

        if (jugador) jugador.enabled = false;
        SetEnabledForAnimators(animators, false);
        SetEnabledForScripts(scriptsExtras, false);
        SetEnabledForScripts(spawners.ToArray(), false);
    }

    public void OnResetPressed()
    {
        Time.timeScale = 1f;
        currentState = GameState.PLAY;
        needsRefresh = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitPressed() => SceneManager.LoadScene("MenuInicio");

    public void ForceRefreshReferences() => needsRefresh = true;
}
