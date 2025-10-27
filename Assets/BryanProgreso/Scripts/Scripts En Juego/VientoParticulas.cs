using UnityEngine;

/// <summary>
/// Este script controla un sistema de partículas para simular la caída de agua.
/// Se puede ajustar la emisión, la velocidad, la forma del emisor, entre otros parámetros.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class VientoParticulas : MonoBehaviour
{
    // Referencia al sistema de partículas
    private ParticleSystem waterParticleSystem;

    // Variables públicas para ajustar en el editor de Unity
    [Header("Ajustes de Emisión")]
    [Tooltip("Tasa de emisión de partículas por segundo")]
    public float emissionRate = 100f;

    [Header("Ajustes de Velocidad")]
    [Tooltip("Velocidad inicial de las partículas")]
    public float startSpeed = 5f;
    [Tooltip("Gravedad aplicada a las partículas")]
    public float gravityModifier = 0.5f;

    [Header("Ajustes de Forma")]
    [Tooltip("Ángulo del cono de emisión")]
    public float shapeAngle = 0f;
    [Tooltip("Radio de la forma de emisión")]
    public float shapeRadius = 0.1f;

    [Header("Ajustes de Tamaño")]
    [Tooltip("Tamaño inicial de las partículas")]
    public float startSize = 0.1f;

    private void Start()
    {
        // Obtener el componente ParticleSystem adjunto al mismo GameObject
        waterParticleSystem = GetComponent<ParticleSystem>();

        // Configurar el sistema de partículas con los valores iniciales
        ConfigureParticleSystem();
    }

    /// <summary>
    /// Configura el sistema de partículas con los valores establecidos en las variables públicas.
    /// </summary>
    private void ConfigureParticleSystem()
    {
        var mainModule = waterParticleSystem.main;
        var emissionModule = waterParticleSystem.emission;
        var shapeModule = waterParticleSystem.shape;
        var velocityOverLifetimeModule = waterParticleSystem.velocityOverLifetime;

        // Configurar módulo principal
        mainModule.startSpeed = startSpeed;
        mainModule.startSize = startSize;
        mainModule.gravityModifier = gravityModifier;

        // Configurar emisión
        emissionModule.rateOverTime = emissionRate;

        // Configurar la forma (por defecto es cono, pero podemos cambiarla si se quiere)
        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Cone; // Forma de cono para la emisión
        shapeModule.angle = shapeAngle;
        shapeModule.radius = shapeRadius;

        // Si queremos que las partículas tengan una ligera variación en su velocidad a lo largo de la vida, podemos usar velocityOverLifetime.
        // En este caso, no lo usamos, pero lo dejamos como ejemplo de cómo podríamos acceder a otros módulos.
        // velocityOverLifetimeModule.enabled = false; // Por defecto desactivado
    }

    // Si se cambian los valores en el inspector durante el juego, actualizamos el sistema
    private void OnValidate()
    {
        // Si el sistema de partículas ya está asignado (en el editor, durante el juego no se ejecuta OnValidate)
        if (waterParticleSystem != null)
        {
            ConfigureParticleSystem();
        }
    }
}
