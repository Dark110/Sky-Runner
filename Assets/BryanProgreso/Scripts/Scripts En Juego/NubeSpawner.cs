using UnityEngine;
using System.Collections.Generic;

public class NubeSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;               // Jugador para spawn relativo
    public GameObject prefabNube;           // Prefab de la nube

    [Header("Spawn")]
    public float offsetX = 10f;             // Rango lateral
    public float offsetY = 5f;              // Rango vertical
    public float distanciaZMin = 15f;       // Distancia mínima delante del jugador
    public float distanciaZMax = 25f;       // Distancia máxima delante del jugador
    public int maxNubes = 10;               // Número máximo de nubes activas
    public float tiempoEntreSpawn = 0.7f;   // Tiempo entre spawns

    [Header("Escala y transición")]
    public float escalaInicial = 0.2f;      // Escala inicial al spawn
    public float duracionTransicion = 2f;   // Duración de la transición a escala normal

    [Header("Movimiento")]
    public float velocidadZ = 1f;           // Velocidad a la que se alejan (hacia atrás)

    private float temporizador = 0f;
    private List<GameObject> nubesActivas = new List<GameObject>();

    void Update()
    {
        if (jugador == null || prefabNube == null) return;

        temporizador -= Time.deltaTime;
        if (temporizador <= 0f && nubesActivas.Count < maxNubes)
        {
            GenerarNube();
            temporizador = tiempoEntreSpawn;
        }

        MoverNubes();
        LimpiarNubes();
    }

    void GenerarNube()
    {
        float x = jugador.position.x + Random.Range(-offsetX, offsetX);
        float y = jugador.position.y + Random.Range(-offsetY, offsetY);
        float z = jugador.position.z + Random.Range(distanciaZMin, distanciaZMax);

        GameObject nuevaNube = Instantiate(prefabNube, new Vector3(x, y, z), Quaternion.identity);
        nubesActivas.Add(nuevaNube);

        // Escala inicial pequeña
        Vector3 escalaFinal = nuevaNube.transform.localScale;
        nuevaNube.transform.localScale = escalaFinal * escalaInicial;

        // Iniciar transición de escala
        StartCoroutine(EscalarSuavemente(nuevaNube, escalaFinal, duracionTransicion));
    }

    private System.Collections.IEnumerator EscalarSuavemente(GameObject obj, Vector3 escalaFinal, float duracion)
    {
        float tiempo = 0f;
        Vector3 escalaInicial = obj.transform.localScale;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            obj.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
            yield return null;
        }

        obj.transform.localScale = escalaFinal; // asegurar valor final exacto
    }

    void MoverNubes()
    {
        foreach (var nube in nubesActivas)
        {
            if (nube == null) continue;
            nube.transform.Translate(Vector3.back * velocidadZ * Time.deltaTime, Space.World);
        }
    }

    void LimpiarNubes()
    {
        for (int i = nubesActivas.Count - 1; i >= 0; i--)
        {
            if (nubesActivas[i] == null)
            {
                nubesActivas.RemoveAt(i);
                continue;
            }

            // Si la nube quedó detrás del jugador, se destruye
            if (nubesActivas[i].transform.position.z < jugador.position.z - 5f)
            {
                Destroy(nubesActivas[i]);
                nubesActivas.RemoveAt(i);
            }
        }
    }
}
