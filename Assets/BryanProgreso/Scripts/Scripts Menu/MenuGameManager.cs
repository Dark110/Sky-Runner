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
        if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // SOLO actualizar referencias si estamos en escena de gameplay
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Gameplay" && (needsRefresh || jugador == null))
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
        Debug.Log($"Escena cargada: {scene.name}");

        // SOLO buscar referencias en la escena de gameplay
        if (scene.name == "Gameplay")
        {
            needsRefresh = true;
            Debug.Log("Forzando estado PLAY en escena Gameplay");
            currentState = GameState.PLAY; // Primero cambiar el estado
            ApplyGameState(currentState);  // Luego aplicarlo
        }
        else
        {
            // En otras escenas (menús, game over), resetear referencia al jugador
            jugador = null;
            Debug.Log($"Escena no-Gameplay detectada: {scene.name}. Jugador resetado.");
        }
    }

    private void RefreshReferences()
    {
        // Solo buscar en escena de gameplay
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Gameplay")
        {
            Debug.Log($"No buscar referencias en escena: {currentScene}");
            return;
        }

        // Buscar jugador
        if (jugador == null)
        {
            jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null)
                Debug.Log("Jugador encontrado: " + jugador.gameObject.name);
            else
                Debug.LogWarning("No se encontró el jugador en la escena Gameplay");
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
        // Solo aplicar si estamos en escena de gameplay o es GAMEOVER
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Gameplay" && state != GameState.GAMEOVER)
        {
            Debug.Log($"No aplicar estado {state} en escena: {currentScene}");
            return;
        }

        // Asegurarse de que las referencias estén actualizadas solo en gameplay
        if (currentScene == "Gameplay" && (needsRefresh || jugador == null))
        {
            RefreshReferences();
            needsRefresh = false;
        }

        // NUEVO: Debug del estado anterior vs nuevo
        Debug.Log($"Aplicando estado: {state} (anterior: {currentState})");

        currentState = state;

        switch (state)
        {
            case GameState.PLAY:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                if (jugador)
                {
                    jugador.enabled = true;
                    Debug.Log("Jugador habilitado");
                }
                SetEnabledForAnimators(animators, true);
                SetEnabledForScripts(scriptsExtras, true);
                SetEnabledForScripts(spawners.ToArray(), true);
                break;

            case GameState.PAUSE:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                if (jugador)
                {
                    jugador.enabled = false;
                    Debug.Log("Jugador deshabilitado");
                }
                SetEnabledForAnimators(animators, false);
                SetEnabledForScripts(scriptsExtras, false);
                SetEnabledForScripts(spawners.ToArray(), false);
                break;

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

    // Funciones Botones (UI)
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

    public void OnGameOver()
    {
        Debug.Log("Game Over llamado");
        ApplyGameState(GameState.GAMEOVER);
    }

    public void OnResetPressed()
    {
        Time.timeScale = 1f;
        currentState = GameState.PLAY;
        needsRefresh = true;
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