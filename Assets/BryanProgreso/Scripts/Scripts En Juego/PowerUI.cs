using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpUI : MonoBehaviour
{
    [Header("Referencias UI (Tipo Image: Filled Radial)")]
    [Tooltip("La imagen para el timer de Velocidad.")]
    public Image imagenVelocidad;
    [Tooltip("La imagen para el timer de Invulnerabilidad.")]
    public Image imagenInvulnerabilidad;

    // Variables para las corrutinas activas
    private Coroutine velocidadRoutine;
    private Coroutine invulnerabilidadRoutine;

    private void Start()
    {
        // Inicializa los timers ocultos
        if (imagenVelocidad != null)
            imagenVelocidad.fillAmount = 0f;
        if (imagenInvulnerabilidad != null)
            imagenInvulnerabilidad.fillAmount = 0f;
    }

    // Método llamado por MovimientoParacaidista.cs
    public void IniciarTimer(TipoPowerUp tipo, float duracion)
    {
        // Detiene la rutina anterior si existe (para reiniciar el timer)
        if (tipo == TipoPowerUp.Velocidad && velocidadRoutine != null)
            StopCoroutine(velocidadRoutine);
        if (tipo == TipoPowerUp.Invulnerabilidad && invulnerabilidadRoutine != null)
            StopCoroutine(invulnerabilidadRoutine);

        // Inicia la nueva rutina del timer
        if (tipo == TipoPowerUp.Velocidad)
            velocidadRoutine = StartCoroutine(RutinaTimerVisual(imagenVelocidad, duracion));
        else if (tipo == TipoPowerUp.Invulnerabilidad)
            invulnerabilidadRoutine = StartCoroutine(RutinaTimerVisual(imagenInvulnerabilidad, duracion));
    }

    private IEnumerator RutinaTimerVisual(Image imagen, float duracionTotal)
    {
        if (imagen == null) yield break;

        float tiempoTranscurrido = 0f;
        imagen.fillAmount = 1f; // Inicia lleno

        while (tiempoTranscurrido < duracionTotal)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Calcula el porcentaje restante: 1.0 (lleno) a 0.0 (vacío)
            float porcentaje = 1f - (tiempoTranscurrido / duracionTotal);
            imagen.fillAmount = porcentaje;

            yield return null;
        }

        // Asegura que al terminar quede completamente vacío
        imagen.fillAmount = 0f;
    }
}