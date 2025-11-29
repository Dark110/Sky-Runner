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

    [Header("Configuración Automática")]
    [Tooltip("Si es true, busca referencias automáticamente al iniciar")]
    public bool autoRefreshReferences = true;

    // Referencias internas
    private MovimientoParacaidista jugador;
    private List<Animator> animators = new List<Animator>();
    private List<MonoBehaviour> scriptsExtras = new List<MonoBehaviour>(); // Spawners y otros

    private GameState currentState;
    public GameState CurrentState => currentState;

    private void Awake()
    {
        // Singleton Básico (Solo para esta escena, se destruye al recargar)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Aseguramos que el tiempo corra al nacer el script
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void Start()
    {
        if (autoRefreshReferences)
        {
            RefreshReferences();
        }

        // Aplicar estado inicial
        currentState = initialState;
        ApplyGameState(currentState);
    }

    private void Update()
    {
        // Control manual de pausa con Teclado (PC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (currentState == GameState.PLAY) OnPausePressed();
        else if (currentState == GameState.PAUSE) OnResumePressed();
    }

    // -----------------------------
    // Refrescar referencias
    // -----------------------------
    public void RefreshReferences()
    {
        // 1. Buscar Jugador
        jugador = FindFirstObjectByType<MovimientoParacaidista>();

        // 2. Buscar Animators (Incluyendo inactivos por si acaso)
        animators.Clear();
        animators.AddRange(FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        // 3. Buscar Spawners y scripts extras
        scriptsExtras.Clear();
        var allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var mb in allScripts)
        {
            // Agrega aquí cualquier otro tag o tipo de script que necesites pausar
            if (mb.CompareTag("Spawner"))
            {
                scriptsExtras.Add(mb);
            }
        }
    }

    // -----------------------------
    // Lógica de Estado
    // -----------------------------
    private void ApplyGameState(GameState state)
    {
        currentState = state;
        bool isGameplayActive = (state == GameState.PLAY);

        // 1. Control del Tiempo y Audio
        if (state == GameState.PAUSE)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false; // En GameOver también queremos escuchar sonidos (ej. música triste)
        }

        // 2. Control del Jugador
        if (jugador != null)
            jugador.enabled = (state == GameState.PLAY); // Solo activo en PLAY

        // 3. Control de Animaciones
        foreach (var anim in animators)
        {
            if (anim != null) anim.enabled = isGameplayActive;
        }

        // 4. Control de Spawners y Extras
        foreach (var script in scriptsExtras)
        {
            if (script != null) script.enabled = isGameplayActive;
        }

        // Notificar a la UI (Opcional, si tienes un UIManager escuchando)
        OnGameStateChanged?.Invoke(state);
    }

    // -----------------------------
    // Funciones Públicas para Botones UI
    // -----------------------------
    public void OnPausePressed() => ApplyGameState(GameState.PAUSE);

    public void OnResumePressed() => ApplyGameState(GameState.PLAY);

    public void OnGameOver() => ApplyGameState(GameState.GAMEOVER);

    public void OnResetPressed()
    {
        // Importante: Antes de recargar, aseguramos timeScale 1 para que la carga no se congele si es asíncrona
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}