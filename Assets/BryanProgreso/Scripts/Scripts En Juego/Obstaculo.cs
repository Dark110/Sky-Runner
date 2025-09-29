using UnityEngine;
using System.Collections.Generic;

public class ObstaculoSpawnerCaotico : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabObstaculo;

    [Header("Spawn")]
    public float distanciaZ = 15f;       // distancia adelante del jugador
    public float offsetX = 5f;           // rango lateral de spawn
    public float offsetY = 3f;           // rango vertical de spawn
    public float altura = 1f;            // base de altura

    [Header("Frecuencia")]
    public float tiempoEntreSpawn = 1f;
    private float temporizador = 0f;

    [Header("Límite de obstáculos")]
    public int maxObstaculos = 15;
    public float distanciaDetras = 10f;

    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        if (jugador == null || prefabObstaculo == null) return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f && obstaculosActivos.Count < maxObstaculos)
        {
            GenerarObstaculo();
            temporizador = tiempoEntreSpawn;
        }

        LimpiarObstaculos();
    }

    void GenerarObstaculo()
    {
        // Generar posición aleatoria en X/Y delante del jugador
        float x = jugador.position.x + Random.Range(-offsetX, offsetX);
        float y = jugador.position.y + altura + Random.Range(-offsetY, offsetY);
        float z = jugador.position.z - distanciaZ;

        Vector3 spawnPos = new Vector3(x, y, z);

        // Instanciar el obstáculo con tu MovObstaculo intacto
        GameObject nuevo = Instantiate(prefabObstaculo, spawnPos, Quaternion.identity);
        obstaculosActivos.Add(nuevo);

        // Visual debug
        Debug.DrawLine(jugador.position, spawnPos, Color.red, 1f);
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

            // Destruir si queda demasiado atrás del jugador
            if (obstaculosActivos[i].transform.position.z > jugador.position.z + distanciaDetras)
            {
                Destroy(obstaculosActivos[i]);
                obstaculosActivos.RemoveAt(i);
            }
        }
    }
}
