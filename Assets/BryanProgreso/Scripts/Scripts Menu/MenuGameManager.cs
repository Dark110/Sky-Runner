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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Update()
    {
        // Control manual de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.PLAY) OnPausePressed();
            else if (currentState == GameState.PAUSE) OnResumePressed();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reactivar tiempo y audio siempre al cambiar escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Solo refrescar referencias si es un nivel que contenga GUI / gameplay
        if (scene.name.Contains("Gameplay") || scene.name.Contains("Lvl"))
        {
            RefreshReferences();
            currentState = GameState.PLAY;
            ApplyGameState(currentState);
        }
        else
        {
            jugador = null;
            animators = null;
            spawners.Clear();
        }
    }

    // -----------------------------
    // Refrescar referencias automáticamente
    // -----------------------------
    private void RefreshReferences()
    {
        // Buscamos jugador automáticamente
        jugador = FindFirstObjectByType<MovimientoParacaidista>();

        // Animadores activos o inactivos
        animators = FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // Limpiar y detectar spawners
        spawners.Clear();
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var mb in allObjects)
        {
            if (mb != null && mb.gameObject.CompareTag("Spawner"))
                spawners.Add(mb);
        }
    }

    // -----------------------------
    // Aplicar estado de juego
    // -----------------------------
    private void ApplyGameStateInternal(GameState state, bool notify)
    {
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
                Time.timeScale = 1f; // no pausamos
                AudioListener.pause = false;
                enableGameplay = false;
                break;
        }

        // Aplicar a jugador, animadores y scripts
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

    // -----------------------------
    // Funciones de estado y UI
    // -----------------------------
    public void OnPausePressed() => ApplyGameState(GameState.PAUSE);
    public void OnResumePressed() => ApplyGameState(GameState.PLAY);
    public void OnGameOver() => ApplyGameState(GameState.GAMEOVER);

    // Pausa sin afectar UI (usado en GameOverManager)
    public void PauseWithoutMenu()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitPressed() => SceneManager.LoadScene("MenuInicio");

    public void ForceRefreshReferences() => RefreshReferences();
}
