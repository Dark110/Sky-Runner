using UnityEngine;
using System.Collections.Generic;

public class ObstaculoSpawnerCaoticoFinal : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform jugador;
    public GameObject prefabObstaculo; // obstáculo normal

    [Header("Spawn Normal")]
    public float distanciaZMin = 8f;
    public float distanciaZMax = 15f;
    public float offsetX = 5f;
    public float offsetY = 3f;
    public float alturaBase = 1f;

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
        if (jugador == null) return;

        // --- Spawn normal ---
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

        // Coroutine para escalar suavemente
        StartCoroutine(EscalarSuavemente(nuevo, escalaFinal, 1f)); // duración 1 segundo
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

            // --- Movimiento en Z hacia el jugador ---
            float step = aux.velZ * Time.deltaTime;
            obs.transform.position = Vector3.MoveTowards(
                obs.transform.position,
                new Vector3(obs.transform.position.x, obs.transform.position.y, jugador.position.z),
                step
            );

            // --- Homing lateral/vertical hacia jugador ---
            Vector3 targetPos = new Vector3(
                jugador.position.x,
                jugador.position.y,
                obs.transform.position.z
            );

            obs.transform.position = Vector3.Lerp(
                obs.transform.position,
                targetPos,
                aux.homing * Time.deltaTime
            );
        }
    }
}

public class MovObstaculoAux : MonoBehaviour
{
    [HideInInspector] public float velZ;
    [HideInInspector] public float homing;
}
