using UnityEngine;
using System.Collections.Generic;

public class ObstaculoSpawnerCaoticoFinal : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabObstaculo;

    [Header("Spawn Caótico")]
    public float distanciaZMin = 8f;        // distancia mínima delante del jugador
    public float distanciaZMax = 15f;       // distancia máxima delante del jugador
    public float offsetX = 5f;              // rango lateral X
    public float offsetY = 3f;              // rango vertical Y
    public float alturaBase = 1f;           // altura base

    [Header("Frecuencia")]
    public float tiempoEntreSpawn = 0.6f;
    private float temporizador = 0f;

    [Header("Límite de obstáculos")]
    public int maxObstaculos = 15;
    public float distanciaDetras = 10f;

    [Header("Homing")]
    public float homingMin = 1f;            // homing mínimo hacia jugador
    public float homingMax = 3f;            // homing máximo hacia jugador

    [Header("Velocidad de Z")]
    public float velZMin = 3f;
    public float velZMax = 7f;

    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        if (jugador == null || prefabObstaculo == null) return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f && obstaculosActivos.Count < maxObstaculos)
        {
            GenerarObstaculoCaotico();
            temporizador = tiempoEntreSpawn;
        }

        LimpiarObstaculos();
        AplicarHoming();
    }

    void GenerarObstaculoCaotico()
    {
        float x = jugador.position.x + Random.Range(-offsetX, offsetX);
        float y = jugador.position.y + alturaBase + Random.Range(-offsetY, offsetY);
        float z = jugador.position.z - Random.Range(distanciaZMin, distanciaZMax);

        Vector3 spawnPos = new Vector3(x, y, z);

        GameObject nuevo = Instantiate(prefabObstaculo, spawnPos, Quaternion.identity);
        obstaculosActivos.Add(nuevo);

        // Guardamos velocidad Z y homing en un script auxiliar si quieres
        MovObstaculoAux aux = nuevo.AddComponent<MovObstaculoAux>();
        aux.velZ = Random.Range(velZMin, velZMax);
        aux.homing = Random.Range(homingMin, homingMax);
    }

    void LimpiarObstaculos()
    {
        for (int i = obstaculosActivos.Count - 1; i >= 0; i--)
        {
            if (obstaculosActivos[i] == null)
            {
                obstaculosActivos.RemoveAt(i);
                continue;
            }

            if (obstaculosActivos[i].transform.position.z > jugador.position.z + distanciaDetras)
            {
                Destroy(obstaculosActivos[i]);
                obstaculosActivos.RemoveAt(i);
            }
        }
    }

    void AplicarHoming()
    {
        foreach (var obs in obstaculosActivos)
        {
            if (obs == null) continue;

            // Tomamos el auxiliar si existe
            MovObstaculoAux aux = obs.GetComponent<MovObstaculoAux>();
            if (aux == null) continue;

            // Movimiento en Z
            obs.transform.Translate(Vector3.forward * -aux.velZ * Time.deltaTime, Space.World);

            // Homing leve en X/Y
            Vector3 dir = new Vector3(
                jugador.position.x - obs.transform.position.x,
                jugador.position.y - obs.transform.position.y,
                0
            );
            obs.transform.position += dir.normalized * aux.homing * Time.deltaTime;
        }
    }
}

// Script auxiliar para almacenar homing y velocidad Z
public class MovObstaculoAux : MonoBehaviour
{
    [HideInInspector] public float velZ;
    [HideInInspector] public float homing;
}
