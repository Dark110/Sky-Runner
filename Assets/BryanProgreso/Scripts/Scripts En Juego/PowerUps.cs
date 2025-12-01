// PowerUp.cs
using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [Header("Configuración del Efecto")]
    public TipoPowerUp tipo = TipoPowerUp.Velocidad; // Usa el enum global
    public float duracion = 5f;
    public float multiplicadorVelocidad = 1.5f;

    [Header("Movimiento y Despawn")]
    [Tooltip("Velocidad con la que el PowerUp avanza hacia Z Positivo.")]
    public float velocidadMovimiento = 5f;
    [Tooltip("El PowerUp se destruye si su Z es mayor que este valor (ej: 10f).")]
    public float limiteDespawnZ = 10f;

    // --- Lógica de Apariencia (Fade In) ---
    [Header("Apariencia")]
    public float duracionFade = 0.5f;
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    // -------------------------------------

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    private void Start()
    {
        // Iniciar Fade In visual
        if (spriteRenderer != null)
        {
            Color c = colorOriginal;
            c.a = 0f; // Alpha 0 (invisible)
            spriteRenderer.color = c;
            StartCoroutine(FadeIn());
        }
    }

    private void Update()
    {
        // 1. Movimiento hacia adelante (+Z Global)
        transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime, Space.World);

        // 2. Despawn por Límite Z Positivo
        if (transform.position.z > limiteDespawnZ)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
       
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("PowerUp"); // ⬅️ ¡Aquí está!
        }

        if (other.CompareTag("Player"))
        {
            var jugadorScript = other.GetComponent<MovimientoParacaidista>();



            if (jugadorScript != null)
            {
                Debug.Log($"<color=green>[PowerUp] Colisión detectada con objeto: {tipo}. Aplicando efecto...</color>");

                switch (tipo)
                {
                    case TipoPowerUp.Velocidad:
                        jugadorScript.ActivarVelocidad(multiplicadorVelocidad, duracion);
                        break;

                    case TipoPowerUp.Invulnerabilidad:
                        jugadorScript.ActivarInvulnerabilidad(duracion);
                        break;
                }

                // Destruir el objeto una vez que el efecto se ha transferido al jugador
                Destroy(gameObject);
            }
        }
    }

    // Coroutine para el Fade In
    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < duracionFade && spriteRenderer != null)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / duracionFade);

            Color c = colorOriginal;
            c.a = alpha;
            spriteRenderer.color = c;

            yield return null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOriginal;
        }
    }
}