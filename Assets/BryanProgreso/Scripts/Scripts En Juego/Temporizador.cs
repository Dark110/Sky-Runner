using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Temporizador : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    [Tooltip("Duración del temporizador de juego en segundos.")]
    public float MinutoTiempo = 60f;

    [Header("Configuración del Contador Inicial")]
    [Tooltip("Duración de la cuenta regresiva inicial (en segundos).")]
    public float tiempoCuentaInicio = 3f;
    public TMP_Text UI_CuentaInicio; // Texto de 3, 2, 1, ¡YA!

    [Header("Modo de prueba")]
    public bool modoPrueba = false;
    public float velocidadTest = 5f;

    [Header("UI del Temporizador de juego")]
    public TMP_Text UI_Tiempo;

    private float contador;
    private float cuentaInicio;
    private bool juegoActivo = false;
    private bool cuentaTerminada = false;
    private int minutos;
    private int segundos;

    void Start()
    {
        // Inicializa ambos contadores
        contador = MinutoTiempo;
        cuentaInicio = tiempoCuentaInicio;

        // Si faltan referencias, avisar al desarrollador (pero no crashear)
        if (UI_Tiempo == null)
            Debug.LogWarning("[Temporizador] UI_Tiempo no está asignado en el inspector. La UI no mostrará el tiempo.");
        if (UI_CuentaInicio == null)
            Debug.LogWarning("[Temporizador] UI_CuentaInicio no está asignado en el inspector. No se mostrará la cuenta 3,2,1.");

        // Mostrar tiempo inicial si existe la referencia
        ActualizarUI();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        if (modoPrueba) delta *= velocidadTest;

        // Primero manejamos la cuenta de inicio (3,2,1,¡YA!)
        if (!cuentaTerminada)
        {
            cuentaInicio -= delta;

            if (cuentaInicio > 0f)
            {
                int numero = Mathf.CeilToInt(cuentaInicio);
                if (UI_CuentaInicio != null)
                    UI_CuentaInicio.text = numero.ToString();
            }
            else
            {
                if (UI_CuentaInicio != null)
                    UI_CuentaInicio.text = "¡YA!";
                else
                    Debug.Log("[Temporizador] Cuenta inicio terminó (no hay UI_CuentaInicio).");

                cuentaTerminada = true;
                juegoActivo = true;

                // Oculta el texto "¡YA!" después de 1 segundo, solo si existe
                if (UI_CuentaInicio != null)
                    Invoke(nameof(DesactivarCuenta), 1f);
            }

            return; // No comienza el temporizador del juego hasta que acabe la cuenta
        }

        // Si la cuenta inicial ya terminó, empieza el temporizador de juego
        if (juegoActivo)
        {
            contador -= delta;
            contador = Mathf.Max(0, contador);

            minutos = Mathf.FloorToInt(contador / 60f);
            segundos = Mathf.FloorToInt(contador % 60f);

            if (UI_Tiempo != null)
                UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            if (contador <= 0f)
            {
                juegoActivo = false;
                Victoria();
            }
        }
    }

    void ActualizarUI()
    {
        minutos = Mathf.FloorToInt(contador / 60f);
        segundos = Mathf.FloorToInt(contador % 60f);
        if (UI_Tiempo != null)
            UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void DesactivarCuenta()
    {
        if (UI_CuentaInicio != null)
            UI_CuentaInicio.gameObject.SetActive(false);
    }

    void Victoria()
    {
        Debug.Log("¡Tiempo completado! Victoria");
        if (UI_Tiempo != null)
            UI_Tiempo.text = "00:00";

        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();
        else
            Debug.LogWarning("[Temporizador] SaveDataManager.Instance es null al terminar el juego.");

        // Ten en cuenta que cargar escena fallará si el nombre no existe; asegurarse de que la escena "Victoria" esté incluida en Build Settings
        SceneManager.LoadScene("Victoria");
    }

    public void DetenerTemporizador()
    {
        juegoActivo = false;
    }
}