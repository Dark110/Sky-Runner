using UnityEngine;
using UnityEngine.SceneManagement;
using PepeProgreso;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject pausePanel;
    public GameObject GUI;

    private void OnEnable()
    {
        // Intentamos suscribirnos de forma segura al GameManager.
        // Si aún no existe, se intenta buscar una instancia en la escena.
        if (GameManager.GetInstance() == null)
        {
            var gmInScene = FindObjectOfType<PepeProgreso.GameManager>();
            if (gmInScene != null)
            {
                // Si se encontró, asignarlo a la instancia única si es necesario (si tu GameManager lo hace).
                // aquí asumimos que GameManager.SetInstance lo hace en Awake; si no, la instancia seguirá siendo null.
            }
        }

        var gm = GameManager.GetInstance();
        if (gm != null)
            gm.OnGameStateChanged += OnGameStateChangedCallback;
        else
            Debug.LogWarning("[PauseMenu] No se encontró GameManager al habilitar PauseMenu. Asegúrate de que exista en la escena.");
    }

    private void OnDisable()
    {
        // Siempre desuscribirse para evitar referencias colgantes
        if (GameManager.GetInstance() != null)
            GameManager.GetInstance().OnGameStateChanged -= OnGameStateChangedCallback;
    }

    private void Start()
    {
        // Inicializar visibilidad según el estado actual (por si el evento ya fue enviado antes)
        var gm = GameManager.GetInstance();
        if (gm != null)
            OnGameStateChangedCallback(gm.CurrentState);
    }

    private void Update()
    {
        // Tecla ESC para alternar pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var gm = GameManager.GetInstance();
            if (gm == null) return;

            if (gm.CurrentState == GameState.Playing)
                gm.ChangeState(GameState.Paused);
            else if (gm.CurrentState == GameState.Paused)
                gm.ChangeState(GameState.Playing);
        }
    }

    private void OnGameStateChangedCallback(GameState state)
    {
        if (pausePanel != null) pausePanel.SetActive(state == GameState.Paused);
        if (GUI != null) GUI.SetActive(state != GameState.Paused);
    }

    public void OnResumePressed()
    {
        var gm = GameManager.GetInstance();
        if (gm != null) gm.ChangeState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public void OnResetPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        var gm = GameManager.GetInstance();
        if (gm != null) gm.ChangeState(GameState.Playing);
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1f;
        // Cambia "MainMenu" por el nombre de tu escena de menú si hace falta
        SceneManager.LoadScene("MainMenu");
        var gm = GameManager.GetInstance();
        if (gm != null) gm.ChangeState(GameState.Menu);
    }

    public void OnPauseButtonPressed()
    {
        var gm = GameManager.GetInstance();
        if (gm != null) gm.ChangeState(GameState.Paused);
    }
}