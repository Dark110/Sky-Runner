using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoSpawnerCaoticoFinal : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Prefabs")]
    public GameObject prefabObstaculo;

    [Header("Control")]
    public bool spawnerActivo = true;

    [Header("Spawn Normal Z")]
    public float distanciaZMin = 8f;
    public float distanciaZMax = 15f;

    [Header("Frecuencia")]
    public float tiempoEntreSpawn = 0.7f;
    private float temporizador = 0f;

    [Header("Límite de obstáculos")]
    public int maxObstaculos = 12;
    public float distanciaDetras = 10f;

    [Header("Spawn múltiple")]
    public bool habilitarSpawnMultiple = true;
    [Range(0f, 1f)] public float probabilidadMultiple = 0.3f;
    public int minObstaculosMultiples = 2;
    public int maxObstaculosMultiples = 5;
    public float separacionMultiple = 2f;

    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        if (jugador == null || !spawnerActivo) return;

        temporizador -= Time.deltaTime;
        if (temporizador <= 0f && obstaculosActivos.Count < maxObstaculos)
        {
            GenerarObstaculoCaotico();
            temporizador = tiempoEntreSpawn;
        }

        LimpiarObstaculos();
    }

    void GenerarObstaculoCaotico()
    {
        if (habilitarSpawnMultiple && Random.value <= probabilidadMultiple)
        {
            int cantidadMultiple = Random.Range(minObstaculosMultiples, maxObstaculosMultiples + 1);

            bool vertical = Random.value > 0.5f;
            float z = jugador.position.z - Random.Range(distanciaZMin, distanciaZMax);

            if (vertical)
            {
                float x = jugador.position.x + Random.Range(-2f, 2f);
                float yBase = jugador.position.y - ((cantidadMultiple - 1) * separacionMultiple) / 2f;

                for (int i = 0; i < cantidadMultiple; i++)
                {
                    float y = yBase + i * separacionMultiple;
                    CrearObstaculo(new Vector3(x, y, z));
                }
            }
            else
            {
                float y = jugador.position.y + Random.Range(-1f, 1f);
                float xBase = jugador.position.x - ((cantidadMultiple - 1) * separacionMultiple) / 2f;

                for (int i = 0; i < cantidadMultiple; i++)
                {
                    float x = xBase + i * separacionMultiple;
                    CrearObstaculo(new Vector3(x, y, z));
                }
            }
        }
        else
        {
            float x = jugador.position.x + Random.Range(-2f, 2f);
            float y = jugador.position.y + Random.Range(-1f, 1f);
            float z = jugador.position.z - Random.Range(distanciaZMin, distanciaZMax);

            CrearObstaculo(new Vector3(x, y, z));
        }
    }

    void CrearObstaculo(Vector3 spawnPos)
    {
        GameObject nuevo = Instantiate(prefabObstaculo, spawnPos, Quaternion.identity);
        obstaculosActivos.Add(nuevo);

        // Escala inicial pequeña
        Vector3 escalaFinal = nuevo.transform.localScale;
        nuevo.transform.localScale = escalaFinal * 0.2f;

        // Escalado suave
        StartCoroutine(EscalarSuavemente(nuevo, escalaFinal, 1f));
    }

    IEnumerator EscalarSuavemente(GameObject obj, Vector3 escalaFinal, float duracion)
    {
        float tiempo = 0f;
        Vector3 escalaInicial = obj.transform.localScale;

        while (tiempo < duracion && obj != null)
        {
            tiempo += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempo / duracion);
            yield return null;
        }

        if (obj != null)
            obj.transform.localScale = escalaFinal;
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
}
