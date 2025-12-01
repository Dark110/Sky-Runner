using UnityEngine;
using UnityEngine.UI;

public class SistemaResistencia : MonoBehaviour
{
    [Header("Configuración de Resistencia")]
    [Tooltip("Tiempo total en segundos que dura la resistencia sin recoger anillos.")]
    public float resistenciaMax = 20f;

    [Tooltip("Arrastra aquí la imagen de la barra (debe ser tipo Filled).")]
    public Image barraUI;
    private GameOverManager gameOverManager;

    private float resistenciaActual;

    private void Start()
    {
        // Iniciamos con la barra llena
        resistenciaActual = resistenciaMax;

        if (gameOverManager == null)
        {
            gameOverManager = FindFirstObjectByType<GameOverManager>();
        }
    }

    private void Update()
    {
        // Si el juego está pausado o ya perdimos, no drenamos
        if (Time.timeScale == 0f) return;

        // 1. Drenar resistencia constantemente
        resistenciaActual -= Time.deltaTime;

        // 2. Actualizar UI
        if (barraUI != null)
        {
            // Convertimos el valor a escala 0 a 1 para el FillAmount
            barraUI.fillAmount = resistenciaActual / resistenciaMax;

            // Opcional: Cambiar color a rojo si queda poco
            if (barraUI.fillAmount < 0.25f) barraUI.color = Color.red;
            else barraUI.color = Color.white; // O tu color original
        }

        // 3. Verificar derrota
        if (resistenciaActual <= 0f)
        {
            resistenciaActual = 0f;
            Debug.Log("<color=red>¡Te quedaste sin resistencia!</color>");

            if (gameOverManager != null)
            {
                gameOverManager.TriggerGameOver();
                // Desactivamos este script para que no llame al game over mil veces
                this.enabled = false;
            }
        }
    }

    // ⭐ Método para llamar desde los anillos
    public void RecargarResistencia(float cantidad)
    {
        resistenciaActual += cantidad;

        // No pasarnos del máximo
        if (resistenciaActual > resistenciaMax)
            resistenciaActual = resistenciaMax;

        // Restaurar color si estaba en rojo
        if (barraUI != null) barraUI.color = Color.white; // O tu color base
    }
}