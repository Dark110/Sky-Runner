using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador; // Arrastra al Player aquí
    [Tooltip("Debes asignar EXACTAMENTE 2 prefabs")]
    public GameObject[] powerUpPrefabs;

    // --- VARIABLES MODIFICADAS ---
    [Header("Configuración de Spawn")]
    public float spawnDistanceZ = 40f;
    public float rangoX = 10f; // Ancho del carril (Rango +/- del centro)
    [Tooltip("Rango de variación de altura (+/- del centro de spawn)")]
    public float rangoY = 5f; // NUEVA VARIABLE PARA LA ALTURA
    [Tooltip("Altura base respecto al jugador (ej: altura del suelo)")]
    public float alturaBaseY = 2f;
    // --- FIN VARIABLES MODIFICADAS ---

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

            if (powerUpPrefabs.Length != 2)
            {
                Debug.LogError("Error: Necesitas 2 prefabs en el array powerUpPrefabs");
                yield break;
            }

            // 1. Calcular posición Z (adelante en Z Negativo)
            float spawnZ = jugador.position.z - spawnDistanceZ;

            // 2. Decidir posiciones X aleatorias
            float x1 = Random.Range(-rangoX, rangoX);
            float x2 = Random.Range(-rangoX, rangoX);

            while (Mathf.Abs(x1 - x2) < 2.0f)
            {
                x2 = Random.Range(-rangoX, rangoX);
            }

            // 3. Elegir orden aleatorio
            int indexA = Random.Range(0, 2);
            int indexB = 1 - indexA;

            // 4. Spawnear
            // El rango Y se calcula dentro de SpawnOne
            SpawnOne(powerUpPrefabs[indexA], x1, spawnZ);

            // Spawn PowerUp B
            float offsetZ = Random.Range(5f, 10f);
            float spawnZB = jugador.position.z - (spawnDistanceZ + offsetZ);
            SpawnOne(powerUpPrefabs[indexB], x2, spawnZB);
        }
    }

    void SpawnOne(GameObject prefab, float xPos, float zPos)
    {
        // ⭐ CAMBIO CLAVE: Calcular la posición Y aleatoria
        // La Y se basa en la altura del jugador + alturaBaseY + un rango aleatorio
        float yRandom = Random.Range(-rangoY, rangoY);
        float yPos = jugador.position.y + alturaBaseY + yRandom;

        Vector3 posicionSpawn = new Vector3(
            xPos,
            yPos, // Usamos la Y aleatoria
            zPos
        );

        Instantiate(prefab, posicionSpawn, Quaternion.identity);
    }
}