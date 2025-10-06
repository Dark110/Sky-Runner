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

    [Header("Homing Normal")]
    public float homingMin = 1f;
    public float homingMax = 3f;

    [Header("Velocidad de Z")]
    public float velZMin = 3f;
    public float velZMax = 6f;

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
        AplicarHoming();
    }

    void GenerarObstaculoCaotico()
    {
        // Spawn dinámico en X e Y relativo al jugador
        float x = jugador.position.x + Random.Range(-2f, 2f);
        float y = jugador.position.y + Random.Range(-1f, 1f);
        float z = jugador.position.z - Random.Range(distanciaZMin, distanciaZMax);

        CrearObstaculo(prefabObstaculo, new Vector3(x, y, z), Random.Range(homingMin, homingMax));
    }

    void CrearObstaculo(GameObject prefab, Vector3 spawnPos, float homing)
    {
        GameObject nuevo = Instantiate(prefab, spawnPos, Quaternion.identity);
        obstaculosActivos.Add(nuevo);

        MovObstaculoAux aux = nuevo.AddComponent<MovObstaculoAux>();
        aux.velZ = Random.Range(velZMin, velZMax);
        aux.homing = homing;

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

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempo / duracion);
            yield return null;
        }

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

    void AplicarHoming()
    {
        foreach (var obs in obstaculosActivos)
        {
            if (obs == null) continue;

            MovObstaculoAux aux = obs.GetComponent<MovObstaculoAux>();
            if (aux == null) continue;

            // Movimiento Z hacia jugador
            obs.transform.position = Vector3.MoveTowards(
                obs.transform.position,
                new Vector3(obs.transform.position.x, obs.transform.position.y, jugador.position.z),
                aux.velZ * Time.deltaTime
            );

            // Homing lateral/vertical semi-aleatorio
            Vector3 targetPos = new Vector3(
                jugador.position.x,
                jugador.position.y,
                obs.transform.position.z
            );

            obs.transform.position = Vector3.Lerp(obs.transform.position, targetPos, aux.homing * Time.deltaTime);
        }
    }
}

[System.Serializable]
public class MovObstaculoAux : MonoBehaviour
{
    [HideInInspector] public float velZ;
    [HideInInspector] public float homing;
}
