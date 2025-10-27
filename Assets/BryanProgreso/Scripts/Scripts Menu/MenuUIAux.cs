using UnityEngine;

public class MenuUIHandler : MonoBehaviour
{
    [Header("Panel Menú de Pausa")]
    public GameObject panelMenu;

    private void Start()
    {
        // Menu Oculto al iniciar
        if (panelMenu != null)
            panelMenu.SetActive(false);

        // Se suscribe al evento de cambio de estado del GameManager
        MenuGameManager.Instance.OnGameStateChanged += OnGameStateChanged;

        // NUEVO: Actualizar UI con el estado actual inmediatamente
        UpdateUIWithCurrentState();
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

    // NUEVO: Método para actualizar UI
    private void UpdateUIState(GameState state)
    {
        if (panelMenu == null) return;

        switch (state)
        {
            case GameState.PAUSE:
                panelMenu.SetActive(true); //Mostrar menú
                Debug.Log("UI: Mostrando panel de pausa");
                break;

            case GameState.PLAY:
            case GameState.GAMEOVER:
                panelMenu.SetActive(false); //Ocultar menú
                Debug.Log("UI: Ocultando panel de pausa");
                break;
        }
    }

    // NUEVO: Obtener el estado actual y actualizar UI
    private void UpdateUIWithCurrentState()
    {
        if (MenuGameManager.Instance != null)
        {
            UpdateUIState(MenuGameManager.Instance.CurrentState);
        }
    }
}