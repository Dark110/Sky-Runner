using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuReinicioSiguienteNivel_RegresoMenuPrincipal : MonoBehaviour
{
    [Header("UI niveles")]
    public GameObject Derrota;
    public GameObject Victoria;

    [Header("Botones")]
    public Button botonReiniciar;   // botón dentro del panel Derrota
    public Button botonSiguiente;   // botón dentro del panel Victoria
    public Button botonMenu;        // botón para regresar al menú principal

    [Header("Obstaculos")]
    public Obstaculo sistemaObstaculo;

    void Start()
    {
        // Asignar listeners a los botones
        if (botonReiniciar != null)
            botonReiniciar.onClick.AddListener(ReiniciarNivel);

        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(SiguienteNivel);

        if (botonMenu != null)
            botonMenu.onClick.AddListener(RegresarMenu);
    }

    public void Perdiste()
    {
        if (Derrota != null) Derrota.SetActive(true);
        Time.timeScale = 0f;

        if (sistemaObstaculo != null) sistemaObstaculo.enabled = false;
    }

    public void Ganaste()
    {
        if (Victoria != null) Victoria.SetActive(true);
        Time.timeScale = 0f;
    }

    void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SiguienteNivel()
    {
        Time.timeScale = 1f;
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nivelActual + 1);
    }

    void RegresarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // suposición: escena 0 = menú principal
    }
}
