using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string nombreEscena = "Derrota"; // Escena que se carga al perder
    public float retrasoGameOver = 5f;      // Tiempo antes de cambiar de escena

    [Header("Configuración de transición de iluminación")]
    [Tooltip("Duración de la transición de oscurecimiento antes del Game Over.")]
    public float duracionTransicion = 2f;

    [Tooltip("Intensidad mínima a la que llegará la luz al final del fade.")]
    public float intensidadFinalLuz = 0.05f;

    private bool gameOver = false;
    private Light luzPrincipal;
    private float intensidadOriginalLuz;
    private float intensidadAmbientalOriginal;

    private void Start()
    {
        // Buscar la luz direccional principal
        luzPrincipal = RenderSettings.sun;
        if (luzPrincipal != null)
            intensidadOriginalLuz = luzPrincipal.intensity;

        // Guardar la intensidad ambiental inicial
        intensidadAmbientalOriginal = RenderSettings.ambientIntensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstaculo"))
            TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        if (gameOver) return; // Evita triggers dobles
        gameOver = true;

        // Guardar score
        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        // Pausar jugabilidad sin tocar Time.timeScale
        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.PauseWithoutMenu();
        else
        {
            MovimientoParacaidista jugador = FindFirstObjectByType<MovimientoParacaidista>();
            if (jugador != null) jugador.enabled = false;
        }

        // Inicia la secuencia de transición y cambio de escena
        StartCoroutine(TransicionOscurecerYLuegoGameOver());
    }

    private IEnumerator TransicionOscurecerYLuegoGameOver()
    {
        float tiempo = 0f;

        float inicioLuz = luzPrincipal != null ? intensidadOriginalLuz : 0f;
        float inicioAmbiental = intensidadAmbientalOriginal;

        // Oscurecer gradualmente
        while (tiempo < duracionTransicion)
        {
            tiempo += Time.unscaledDeltaTime; // no se ve afectado por Time.timeScale
            float t = Mathf.Clamp01(tiempo / duracionTransicion);

            if (luzPrincipal != null)
                luzPrincipal.intensity = Mathf.Lerp(inicioLuz, intensidadFinalLuz, t);

            RenderSettings.ambientIntensity = Mathf.Lerp(inicioAmbiental, 0f, t);

            yield return null;
        }

        // Esperar el retraso configurado antes del cambio de escena
        yield return new WaitForSecondsRealtime(retrasoGameOver);

        if (MenuGameManager.Instance != null)
            MenuGameManager.Instance.OnGameOver();

        SceneManager.LoadScene(nombreEscena);
    }
}