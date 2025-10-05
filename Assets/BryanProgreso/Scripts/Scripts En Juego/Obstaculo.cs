using UnityEngine;
using System.Collections.Generic;

public class ObstaculoSpawnerCaoticoFinal : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform jugador;
    public GameObject prefabObstaculo;          // obstáculo normal
    public GameObject prefabObstaculoCastigo;   // obstáculo castigador (ej: caja roja)

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

    [Header("Castigo Anti-Campeo")]
    public float umbralQuieto = 3f;      // segundos quieto para activar castigo
    public float rangoMovimiento = 0.5f; // cuánto puede moverse y aún contar como "quieto"
    public float homingCastigo = 8f;     // homing muy alto
    public float distanciaCastigo = 6f;  // spawnea más cerca

    private List<GameObject> obstaculosActivos = new List<GameObject>();
    private Vector3 ultimaPosJugador;
    private float tiempoQuieto = 0f;

    void Start()
    {
        ultimaPosJugador = jugador.position;
    }

    void Update()
    {
        if (jugador == null || prefabObstaculo == null) return;

        // --- Verificar si el jugador está quieto ---
        if (Vector3.Distance(jugador.position, ultimaPosJugador) < rangoMovimiento)
        {
            tiempoQuieto += Time.deltaTime;
        }
        else
        {
            tiempoQuieto = 0f;
            ultimaPosJugador = jugador.position;
        }

        // --- Castigo si está quieto demasiado ---
        if (tiempoQuieto >= umbralQuieto)
        {
            GenerarObstaculoCastigo();
            tiempoQuieto = 0f;
        }

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

    void GenerarObstaculoCastigo()
    {
        if (prefabObstaculoCastigo == null)
        {
            Debug.Log("Prefab de castigo no asignado");
            return;
        }

        float x = jugador.position.x + Random.Range(-1.5f, 1.5f);
        float y = jugador.position.y + Random.Range(-1f, 1f);
        float z = jugador.position.z - distanciaCastigo;

        Debug.Log("Spawn de obstáculo castigador en " + new Vector3(x, y, z));

        CrearObstaculo(prefabObstaculoCastigo, new Vector3(x, y, z), homingCastigo);
    }

    void CrearObstaculo(GameObject prefab, Vector3 spawnPos, float homing)
    {
        GameObject nuevo = Instantiate(prefab, spawnPos, Quaternion.identity);
        obstaculosActivos.Add(nuevo);

        MovObstaculoAux aux = nuevo.AddComponent<MovObstaculoAux>();
        aux.velZ = Random.Range(velZMin, velZMax);
        aux.homing = homing;
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

            // Movimiento en Z hacia adelante
            obs.transform.Translate(Vector3.forward * -aux.velZ * Time.deltaTime, Space.World);

            // Homing hacia jugador
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
