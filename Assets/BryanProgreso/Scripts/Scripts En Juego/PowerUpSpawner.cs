using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    [Tooltip("Debes asignar EXACTAMENTE 2 prefabs")]
    public GameObject[] powerUpPrefabs;

    [Header("Configuración de Posición (Giroscopio Friendly)")]
    public float spawnDistanceZ = 40f;

    // REDUCIDO: Antes 10. Para giroscopio, 4 o 5 suele ser mejor para no forzar la inclinación.
    [Tooltip("Qué tan a los lados pueden aparecer. Para giroscopio mantén esto bajo (ej: 4 o 5).")]
    public float rangoX = 4.5f;

    // REDUCIDO: Antes 5. Demasiada altura hace difícil interceptarlos.
    [Tooltip("Variación de altura. Mantenlo bajo para que sea alcanzable.")]
    public float rangoY = 1.5f;

    public float alturaBaseY = 2f;

    [Header("Lógica de Doble Spawn")]
    [Range(0f, 1f)]
    [Tooltip("0 = Nunca salen 2. 1 = Siempre salen 2. 0.3 = 30% de probabilidad.")]
    public float probabilidadDobleSpawn = 0.3f; // 30% de chance de que salgan dos

    [Tooltip("Distancia mínima lateral entre los dos powerups para que no se encimen")]
    public float separacionMinimaX = 3.0f;

    [Header("Tiempos")]
    public float tiempoMin = 5f;
    public float tiempoMax = 10f;

    private void Start()
    {
        if (jugador == null)
        {
            Debug.LogError("FATAL: No has asignado al Jugador en el PowerUpSpawner.");
            return;
        }
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float espera = Random.Range(tiempoMin, tiempoMax);
            yield return new WaitForSeconds(espera);

            if (powerUpPrefabs.Length != 2) yield break;

            // --- Lógica de Spawn ---

            // 1. Calcular posición base Z (adelante en Z Negativo)
            float spawnZ = jugador.position.z - spawnDistanceZ;

            // 2. Spawnear el PRIMER PowerUp (Siempre ocurre)
            int indexA = Random.Range(0, 2); // Elige uno al azar
            float x1 = Random.Range(-rangoX, rangoX);

            SpawnOne(powerUpPrefabs[indexA], x1, spawnZ);

            // 3. Decidir si spawneamos el SEGUNDO (Probabilidad)
            // Random.value devuelve un número entre 0.0 y 1.0
            if (Random.value < probabilidadDobleSpawn)
            {
                // Elige el prefab contrario al primero
                int indexB = 1 - indexA;

                // Calcular X2 asegurando separación
                float x2 = Random.Range(-rangoX, rangoX);

                // Re-calcular x2 mientras esté demasiado cerca de x1
                // Agregamos un contador de seguridad para evitar bucles infinitos si el rango es muy chico
                int intentos = 0;
                while (Mathf.Abs(x1 - x2) < separacionMinimaX && intentos < 10)
                {
                    x2 = Random.Range(-rangoX, rangoX);
                    intentos++;
                }

                // Calcular Z para el segundo (un poco más adelante o atrás para dar tiempo)
                // Le damos un offset extra en Z para que no salgan alineados perfectamente
                float offsetZ = Random.Range(8f, 15f);
                float spawnZB = spawnZ - offsetZ; // Más lejos en la dirección de avance (hacia -Z)

                SpawnOne(powerUpPrefabs[indexB], x2, spawnZB);

                Debug.Log("¡Doble Spawn Activado!");
            }
        }
    }

    void SpawnOne(GameObject prefab, float xPos, float zPos)
    {
        // Calcular Y aleatoria controlada
        float yRandom = Random.Range(-rangoY, rangoY);
        float yPos = jugador.position.y + alturaBaseY + yRandom;

        Vector3 posicionSpawn = new Vector3(
            xPos,
            yPos,
            zPos
        );

        Instantiate(prefab, posicionSpawn, Quaternion.identity);
    }
}