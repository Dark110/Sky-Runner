using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefabs de PowerUps (2 tipos)")]
    public GameObject[] powerUpPrefabs; // EXACTAMENTE 2 prefabs

    [Header("Rango de spawn")]
    public Vector3 rango = new Vector3(20f, 0f, 20f);

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Esperar entre 5 y 10 segundos
            float espera = Random.Range(5f, 10f);
            yield return new WaitForSeconds(espera);

            if (powerUpPrefabs.Length != 2)
            {
                Debug.LogError("Debes asignar EXACTAMENTE 2 prefabs de PowerUps!");
                yield break;
            }

            // Elegir aleatoriamente el orden
            int indexA = Random.Range(0, 2);
            int indexB = 1 - indexA; // El otro

            // Spawn del primer power up
            SpawnOne(powerUpPrefabs[indexA]);

            // Spawn del segundo power up
            SpawnOne(powerUpPrefabs[indexB]);

            Debug.Log("Spawn de los 2 power ups (orden aleatorio)");
        }
    }

    void SpawnOne(GameObject prefab)
    {
        Vector3 posicion = transform.position + new Vector3(
            Random.Range(-rango.x, rango.x),
            Random.Range(-rango.y, rango.y),
            Random.Range(-rango.z, rango.z)
        );

        Instantiate(prefab, posicion, Quaternion.identity);
    }
}