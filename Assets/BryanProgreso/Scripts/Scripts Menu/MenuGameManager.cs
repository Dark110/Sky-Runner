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
    public static MenuGameManager Instance { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    [Header("Estado inicial")]
    public GameState initialState = GameState.PLAY;

    [Header("Referencias UI ()")]
    public GameObject panelPausa;

    [Header("Configuración Automática")]
    public bool autoRefreshReferences = true;

    // Referencias internas
    private MovimientoParacaidista jugador;
    private List<Animator> animators = new List<Animator>();
    private List<MonoBehaviour> scriptsExtras = new List<MonoBehaviour>();

    private GameState currentState;
    public GameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void Start()
    {
        if (autoRefreshReferences) RefreshReferences();

        if (panelPausa != null) panelPausa.SetActive(false);

        currentState = initialState;
        ApplyGameState(currentState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (currentState == GameState.PLAY)
        {
            OnPausePressed();
        }
        else if (currentState == GameState.PAUSE)
        {
            OnResumePressed();
        }
    }

    public void RefreshReferences()
    {
        jugador = FindFirstObjectByType<MovimientoParacaidista>();
        animators.Clear();
        animators.AddRange(FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        scriptsExtras.Clear();
        var allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var mb in allScripts)
        {
            if (mb.CompareTag("Spawner")) scriptsExtras.Add(mb);
        }
    }

    private void ApplyGameState(GameState state)
    {
        currentState = state;
        bool isGameplayActive = (state == GameState.PLAY);

        // 1. Control del Tiempo (Solo Time.timeScale)
        if (state == GameState.PAUSE)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (AudioManager.Instance != null)
        {
            
            AudioManager.Instance.sfxSource.enabled = isGameplayActive;
        }

      
        if (panelPausa != null)
        {
            panelPausa.SetActive(state == GameState.PAUSE);
        }

        // Control de Jugador y Scripts (Tu código actual)
        if (jugador != null) jugador.enabled = isGameplayActive;

        foreach (var anim in animators)
            if (anim != null) anim.enabled = isGameplayActive;

        foreach (var script in scriptsExtras)
            if (script != null) script.enabled = isGameplayActive;

        OnGameStateChanged?.Invoke(state);
    }

    // Funciones para botones
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
        SceneManager.LoadScene("MenuInicio");
    }
}