using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    [Header("Panel Menú de Pausa")]
    public GameObject panelMenu;

    private void Start()
    {
        // Menu Oculto al iniciar
        if (panelMenu != null)
            panelMenu.SetActive(false);

        // Solo suscribirse si estamos en Gameplay
        if (SceneManager.GetActiveScene().name == "Gameplay" && MenuGameManager.Instance != null)
        {
            MenuGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            UpdateUIWithCurrentState();
        }
    }

    private void OnDestroy()
    {
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        UpdateUIState(newState);
    }

    private void UpdateUIState(GameState state)
    {
        if (panelMenu == null) return;

        switch (state)
        {
            case GameState.PAUSE:
                // Mostrar menú de pausa solo en Gameplay
                if (SceneManager.GetActiveScene().name == "Gameplay")
                    panelMenu.SetActive(true);
                break;

            case GameState.PLAY:
                panelMenu.SetActive(false);
                break;

            case GameState.GAMEOVER:
                // No tocar UI en GameOver
                panelMenu.SetActive(false);
                break;
        }
    }

    private void UpdateUIWithCurrentState()
    {
        if (MenuGameManager.Instance != null)
            UpdateUIState(MenuGameManager.Instance.CurrentState);
    }
}
