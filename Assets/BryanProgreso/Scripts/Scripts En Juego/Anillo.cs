using UnityEngine;
using System.Collections;

public class DianaPuntos : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadZMin = 12f;    // mínimo hacia adelante
    public float velocidadZMax = 18f;    // máximo hacia adelante
    public float variacionLateral = 0.5f;
    public float variacionVertical = 0.3f;

    [Header("Puntaje")]
    public int puntos = 20;

    [Header("Despawn")]
    public float margenDespawn = 5f;

    [Header("Fade & Efectos")]
    public GameObject particulasPrefab; // Prefab de partículas
    public float fadeDuration = 0.8f;   // duración del fade in/out

    private Transform jugador;
    private Vector3 direccion;
    public float velocidadZ;
    private float objetivoZ;

    private Renderer rend;
    private Material mat;
    private Color colorOriginal;

    private Vector3 scaleOriginal; // Mantener escala original del prefab
    private bool desactivando = false;

    // -------------------------------
    // Inicialización desde el spawner/pool
    // -------------------------------
    public void Initialize()
    {
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        // Guardamos la escala original si no está seteada
        if (scaleOriginal == Vector3.zero)
            scaleOriginal = transform.localScale;

        transform.localScale = scaleOriginal;

        jugador = GameObject.FindWithTag("Player")?.transform;

        // Variación aleatoria en X e Y
        direccion = new Vector3(
            Random.Range(-variacionLateral, variacionLateral),
            Random.Range(-variacionVertical, variacionVertical),
            0f
        );

        velocidadZ = Random.Range(velocidadZMin, velocidadZMax);

        if (jugador != null)
            objetivoZ = jugador.position.z + 10f;

        // Preparar fade-in
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
            colorOriginal = mat.color;
            Color c = colorOriginal;
            c.a = 0f;
            mat.color = c;
            StartCoroutine(FadeInCoroutine(fadeDuration));
        }

        desactivando = false;
    }

    // -------------------------------
    // Update: movimiento hacia adelante + desviación
    // -------------------------------
    void Update()
    {
        if (desactivando) return;

        Vector3 movimiento = new Vector3(direccion.x, direccion.y, 1f) * velocidadZ * Time.deltaTime;
        transform.Translate(movimiento, Space.World);

        if (jugador != null && transform.position.z > objetivoZ + margenDespawn)
        {
            StartCoroutine(FadeOutAndReturn());
        }
    }

    // -------------------------------
    // Colisión con el jugador
    // -------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (desactivando) return;

        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(puntos);

            // Instanciar partículas
            if (particulasPrefab != null)
            {
                GameObject ps = Instantiate(particulasPrefab, transform.position, Quaternion.identity);
                ParticleSystem system = ps.GetComponent<ParticleSystem>();
                if (system != null)
                    Destroy(ps, system.main.duration + system.main.startLifetime.constantMax);
            }

            StartCoroutine(FadeOutAndReturn());
        }
    }

    // -------------------------------
    // Coroutines: Fade-in
    // -------------------------------
    private IEnumerator FadeInCoroutine(float duracion)
    {
        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            Color c = colorOriginal;
            c.a = Mathf.Lerp(0f, 1f, t / duracion);
            if (mat != null) mat.color = c;
            yield return null;
        }
        if (mat != null) mat.color = colorOriginal;
    }

    // -------------------------------
    // Coroutines: Fade-out, giro y reducción antes de volver al pool
    // -------------------------------
    private IEnumerator FadeOutAndReturn()
    {
        desactivando = true;

        float t = 0f;
        Vector3 initialScale = transform.localScale;
        Quaternion initialRotation = transform.rotation;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float ratio = t / fadeDuration;

            // Fade alpha
            if (mat != null)
            {
                Color c = colorOriginal;
                c.a = Mathf.Lerp(1f, 0f, ratio);
                mat.color = c;
            }

            // Escala hacia 0
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, ratio);

            // Rotación 360°
            transform.rotation = initialRotation * Quaternion.Euler(0f, 360f * ratio, 0f);

            yield return null;
        }

        // Reset para el pool: escala original + rotación original + color original
        transform.localScale = scaleOriginal;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        if (mat != null) mat.color = colorOriginal;

        gameObject.SetActive(false); // devolver al pool
    }
}
