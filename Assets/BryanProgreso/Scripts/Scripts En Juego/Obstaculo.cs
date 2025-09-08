using UnityEngine;
using System.Collections.Generic;

public class Obstaculo : MonoBehaviour
{
    public GameObject PrefabObstaculo;
    public float AparicionObstaculos = 1f;
    public float[] PosicionesX = { -2f, 0f, 2f }; // Posibles posiciones en X
    public float PosicionY = 0f;
    public int MaximoObs = 10;

    private float temporizador = 0f;
    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        temporizador -= Time.deltaTime;

        for (int i = obstaculosActivos.Count - 1; i >= 0; i--)
        {
            if (obstaculosActivos[i] == null)
            {
                obstaculosActivos.RemoveAt(i);
            }
        }

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
            float x = PosicionesX[Random.Range(0, PosicionesX.Length)];
            float z = transform.position.z; // Usar la posición Z del spawner
            Vector3 spawnPos = new Vector3(x, PosicionY, z);
            GameObject nuevoObs = Instantiate(PrefabObstaculo, spawnPos, Quaternion.identity);
            nuevoObs.tag = "Obstaculo";
            obstaculosActivos.Add(nuevoObs);
        }
        else
        {
            Debug.LogWarning("PrefabObstaculo no está asignado o no hay posiciones X!");
        }
    }
}