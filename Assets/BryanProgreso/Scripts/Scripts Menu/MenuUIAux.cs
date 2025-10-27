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
    }

    private void OnDestroy()
    {
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (panelMenu == null) return;

        switch (newState)
        {
            case GameState.PAUSE:
                panelMenu.SetActive(true); //Mostrar menú
                break;

            case GameState.PLAY:
            case GameState.GAMEOVER:
                panelMenu.SetActive(false); //Ocultar menú
                break;
        }
    }
}
