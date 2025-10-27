using System;
using System.Collections;
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

    public static MenuGameManager GetInstance() => Instance;

    // Lista dinámica de spawners
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
        RefreshReferences(); // Actualizar referencias al inicio
        ApplyGameState(currentState);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Actualizar referencias si es necesario
        if (needsRefresh || jugador == null)
        {
            RefreshReferences();
            needsRefresh = false;
        }

        // Solo para pruebas en PC
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
        needsRefresh = true; // Marcar que necesita actualizar referencias
    }

    // NUEVO MÉTODO: Actualizar todas las referencias
    private void RefreshReferences()
    {
        // Buscar jugador
        if (jugador == null)
        {
            jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null)
                Debug.Log("Jugador encontrado: " + jugador.gameObject.name);
            else
                Debug.LogWarning("No se encontró el jugador en la escena");
        }

        // Buscar animators
        animators = FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // Buscar spawners
        spawners.Clear();
        MonoBehaviour[] allSpawners = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var mb in allSpawners)
        {
            if (mb != null && mb.gameObject.CompareTag("Spawner") && !spawners.Contains(mb))
                spawners.Add(mb);
        }

        Debug.Log($"Referencias actualizadas - Jugador: {jugador != null}, Animators: {animators.Length}, Spawners: {spawners.Count}");
    }

    private void ApplyGameState(GameState state)
    {
        // Asegurarse de que las referencias estén actualizadas
        if (needsRefresh || jugador == null)
            RefreshReferences();

        currentState = state;

        switch (state)
        {
            case GameState.PLAY:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                if (jugador) jugador.enabled = true;
                SetEnabledForAnimators(animators, true);
                SetEnabledForScripts(scriptsExtras, true);
                SetEnabledForScripts(spawners.ToArray(), true);
                break;

            case GameState.PAUSE:
            case GameState.GAMEOVER:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                if (jugador) jugador.enabled = false;
                SetEnabledForAnimators(animators, false);
                SetEnabledForScripts(scriptsExtras, false);
                SetEnabledForScripts(spawners.ToArray(), false);
                break;
        }

        OnGameStateChanged?.Invoke(state);
        Debug.Log($"Estado del juego cambiado a: {state}");
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

    // Funcion Botones (UI)
    public void OnPausePressed()
    {
        Debug.Log("Botón Pausa presionado");
        ApplyGameState(GameState.PAUSE);
    }

    public void OnResumePressed()
    {
        Debug.Log("Botón Reanudar presionado");
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

    public void ForceRefreshReferences()
    {
        needsRefresh = true;
    }
}