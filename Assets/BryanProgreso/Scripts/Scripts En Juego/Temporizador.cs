using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Temporizador : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float MinutoTiempo = 60f; // Duración en segundos
    public bool modoPrueba = false;  // Activar modo de prueba
    public float velocidadTest = 5f; // Factor de aceleración en modo prueba

    [Header("UI")]
    public TMP_Text UI_Tiempo;

    private float contador;
    private bool juegoActivo = true;
    private int minutos;
    private int segundos;

    void Start()
    {
        contador = MinutoTiempo;
    }

    void Update()
    {
        if (!juegoActivo) return;

        // --- Ajuste para modo prueba ---
        float delta = Time.deltaTime;
        if (modoPrueba)
            delta *= velocidadTest;

        // Restar tiempo
        contador -= delta;
        contador = Mathf.Max(0, contador);

        // Calcular minutos y segundos
        minutos = Mathf.FloorToInt(contador / 60f);
        segundos = Mathf.FloorToInt(contador % 60f);

        // Mostrar en UI
        UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Verificar si terminó el tiempo
        if (contador <= 0f)
        {
            juegoActivo = false;
            Victoria();
        }
    }

    void Victoria()
    {
        Debug.Log("¡Tiempo completado! Victoria");
        UI_Tiempo.text = "00:00";
        SceneManager.LoadScene("Victoria"); // Cambia "Victoria" por tu escena
    }

    public void DetenerTemporizador()
    {
        juegoActivo = false;
    }
}
