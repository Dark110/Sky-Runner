using UnityEngine;

/// <summary>
/// Este script controla un sistema de partículas que simula viento alrededor del jugador.
/// Ahora sigue al jugador mientras este se mueve, manteniendo un offset configurable.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class VientoParticulas : MonoBehaviour
{
    // Referencia al sistema de partículas
    private ParticleSystem waterParticleSystem;

    [Header("Referencia del Jugador")]
    [Tooltip("Objeto del jugador que el sistema de partículas debe seguir.")]
    public Transform jugador; // 🔹 Asigna aquí el transform del jugador en el inspector.

    [Tooltip("Offset de posición respecto al jugador.")]
    public Vector3 offset = new Vector3(0, 0, 0); // 🔹 Puedes ajustar este valor en el inspector.

    [Header("Ajustes de Emisión")]
    public float emissionRate = 100f;

    [Header("Ajustes de Velocidad")]
    public float startSpeed = 5f;
    public float gravityModifier = 0.5f;

    [Header("Ajustes de Forma")]
    public float shapeAngle = 0f;
    public float shapeRadius = 0.1f;

    [Header("Ajustes de Tamaño")]
    public float startSize = 0.1f;

    private void Start()
    {
        waterParticleSystem = GetComponent<ParticleSystem>();
        ConfigureParticleSystem();

        // Si no se asignó el jugador en el inspector, intentar buscarlo automáticamente
        if (jugador == null)
        {
            GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player"); // 🔹 Asegúrate de que el jugador tenga el tag "Player"
            if (jugadorGO != null)
                jugador = jugadorGO.transform;
            else
                Debug.LogWarning("⚠️ No se encontró el objeto del jugador. Asigna el Transform del jugador en el inspector o usa el tag 'Player'.");
        }
    }

    private void Update()
    {
        // 🔹 Si el jugador existe, seguirlo con un offset
        if (jugador != null)
        {
            transform.position = jugador.position + offset;
        }
    }

    /// <summary>
    /// Configura el sistema de partículas con los valores establecidos en las variables públicas.
    /// </summary>
    private void ConfigureParticleSystem()
    {
        var mainModule = waterParticleSystem.main;
        var emissionModule = waterParticleSystem.emission;
        var shapeModule = waterParticleSystem.shape;

        mainModule.startSpeed = startSpeed;
        mainModule.startSize = startSize;
        mainModule.gravityModifier = gravityModifier;

        emissionModule.rateOverTime = emissionRate;

        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Cone;
        shapeModule.angle = shapeAngle;
        shapeModule.radius = shapeRadius;
    }

    private void OnValidate()
    {
        if (waterParticleSystem != null)
        {
            ConfigureParticleSystem();
        }
    }
}
