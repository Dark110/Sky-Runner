using UnityEngine;
using System.Collections.Generic;

public class ObstaculoSpawner : MonoBehaviour
{
    [Header("Prefab del obstáculo")]
    public GameObject PrefabObstaculo;

    [Header("Configuración de aparición")]
    public float AparicionObstaculos = 1f;
    public float[] PosicionesX = { -2f, 0f, 2f }; // Posiciones relativas en X
    public float DistanciaZ = 15f;                // Qué tan lejos enfrente del jugador aparecen
    public int MaximoObs = 10;

    [Header("Configuración de limpieza")]
    public float DistanciaDetrasJugador = 5f; // Si el obstáculo queda detrás de esta distancia → destruir

    private float temporizador = 0f;
    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        temporizador -= Time.deltaTime;

        // Limpiar obstáculos destruidos o demasiado lejos
        for (int i = obstaculosActivos.Count - 1; i >= 0; i--)
        {
            if (obstaculosActivos[i] == null)
            {
                obstaculosActivos.RemoveAt(i);
            }
            else
            {
                // Si el obstáculo quedó muy detrás del jugador → destruirlo
                if (obstaculosActivos[i].transform.position.z < transform.position.z - DistanciaDetrasJugador)
                {
                    Destroy(obstaculosActivos[i]);
                    obstaculosActivos.RemoveAt(i);
                }
            }
        }

        // Generar nuevo obstáculo
        if (temporizador <= 0f && obstaculosActivos.Count < MaximoObs)
        {
            GenerarObstaculo();
            temporizador = AparicionObstaculos;
        }
    }

    void GenerarObstaculo()
    {
        if (PrefabObstaculo != null && PosicionesX.Length > 0)
        {
            // Posición relativa al jugador
            float x = PosicionesX[Random.Range(0, PosicionesX.Length)];
            Vector3 spawnPos = transform.position + new Vector3(x, 0f, DistanciaZ);

            GameObject nuevoObs = Instantiate(PrefabObstaculo, spawnPos, Quaternion.identity);
            nuevoObs.tag = "Obstaculo";

            obstaculosActivos.Add(nuevoObs);
        }
        else
        {
            Debug.LogWarning("PrefabObstaculo no está asignado");
        }
    }
}
