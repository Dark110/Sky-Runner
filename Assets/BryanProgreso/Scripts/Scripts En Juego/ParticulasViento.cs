using UnityEngine;

/// <summary>
/// Controla un sistema de partículas para simular la caída de agua.
/// Requiere un ParticleSystem en el mismo GameObject.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class WaterFallSimulation : MonoBehaviour
{
    [Header("Configuración de Partículas")]
    [Tooltip("Intensidad de emisión (partículas/segundo)")]
    public float emissionRate = 100f;

    [Tooltip("Gravedad aplicada a las partículas (valores negativos = hacia abajo)")]
    public float gravity = -9.81f;

    [Tooltip("Tamaño inicial de las partículas")]
    public float startSize = 0.1f;

    [Tooltip("Velocidad inicial de las partículas")]
    public float startSpeed = 5f;

    [Tooltip("Duración de vida de las partículas (segundos)")]
    public float lifetime = 2f;

    private ParticleSystem _particleSystem;

    void Start()
    {
        // Obtener referencia al sistema de partículas
        _particleSystem = GetComponent<ParticleSystem>();

        // Configurar propiedades iniciales
        ConfigureParticles();
    }

    /// <summary>
    /// Configura las propiedades del sistema de partículas
    /// </summary>
    private void ConfigureParticles()
    {
        var mainModule = _particleSystem.main;
        var emissionModule = _particleSystem.emission;

        // Configurar los parámetros principales
        mainModule.startSize = startSize;
        mainModule.startSpeed = startSpeed;
        mainModule.startLifetime = lifetime;
        mainModule.gravityModifier = Mathf.Abs(gravity) / 9.81f; // Relativo a la gravedad
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

        // Configurar la emisión
        emissionModule.rateOverTime = emissionRate;
    }

    void Update()
    {
        // Actualizar dinámicamente si los valores cambian en tiempo real
        ConfigureParticles();
    }
}
