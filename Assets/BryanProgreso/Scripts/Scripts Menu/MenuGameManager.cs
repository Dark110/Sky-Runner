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

    // Bandera para saber si la cuenta regresiva terminó
    private bool gameplayActivo = false;

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

    private void OnEnable()
    {
        // Suscribirse al evento del otro GameManager al activarse
        GameManager.OnGameStarted += HandleGameStarted;
    }

    private void OnDisable()
    {
        // Desuscribirse al desactivarse
        GameManager.OnGameStarted -= HandleGameStarted;
    }

    // Método llamado cuando el Countdown finaliza
    private void HandleGameStarted()
    {
        gameplayActivo = true;
    }

    private void Start()
    {
        if (autoRefreshReferences) RefreshReferences();

        if (panelPausa != null) panelPausa.SetActive(false);

        // El juego comienza EN ESTADO PLAY, pero gameplayActivo es FALSE hasta que la cuenta regresiva termine
        currentState = initialState;
        ApplyGameState(currentState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Solo se permite pausar/reanudar si el gameplay ya está activo
            if (gameplayActivo || currentState == GameState.PAUSE)
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        // La Pausa SÓLO se permite si el juego está en PLAY Y gameplayActivo es TRUE
        if (currentState == GameState.PLAY && gameplayActivo)
        {
            OnPausePressed();
        }
        else if (currentState == GameState.PAUSE)
        {
            OnResumePressed();
        }
        // Si no se cumple ninguna de las dos condiciones (ej. durante el countdown), no hace nada.
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

        // 1. Control del Tiempo
        if (state == GameState.PAUSE)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        // 2. CONTROL DEL PANEL DE PAUSA
        if (panelPausa != null)
        {
            panelPausa.SetActive(state == GameState.PAUSE);
        }

        // 3. Control de Jugador y Scripts
        // Nota: Los Spawners solo se activan si el GameManager les da la orden final.
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