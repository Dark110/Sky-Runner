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
    public float distanciaFrontal = 15f;    // Distancia delante del jugador para spawn (usará -Z)
    public int maxNubes = 10;               // Número máximo de nubes activas
    public float tiempoEntreSpawn = 0.7f;   // Tiempo entre spawns

    [Header("Escala y transición")]
    public float escalaInicial = 0.2f;      // Escala inicial al spawn
    public float duracionTransicion = 2f;   // Duración de la transición a escala normal

    [Header("Movimiento")]
    public float velocidadMin = 1f;         // Velocidad mínima de movimiento
    public float velocidadMax = 3f;         // Velocidad máxima de movimiento
    public bool moverHaciaJugador = false;  // Si las nubes deben moverse hacia el jugador

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

        LimpiarNubes();
    }

    void GenerarNube()
    {
        float x = jugador.position.x + Random.Range(-offsetX, offsetX);
        float y = jugador.position.y + Random.Range(-offsetY, offsetY);
        float z = jugador.position.z - distanciaFrontal;

        GameObject nuevaNube = Instantiate(prefabNube, new Vector3(x, y, z), Quaternion.identity);
        nubesActivas.Add(nuevaNube);

        Cloud cloudScript = nuevaNube.GetComponent<Cloud>();
        if (cloudScript != null)
        {
            cloudScript.velocidad = Random.Range(velocidadMin, velocidadMax);
            cloudScript.moverHaciaJugador = moverHaciaJugador;
            cloudScript.jugador = jugador;
        }

        Vector3 escalaFinal = nuevaNube.transform.localScale;
        nuevaNube.transform.localScale = escalaFinal * escalaInicial;

        StartCoroutine(EscalarSuavemente(nuevaNube, escalaFinal, duracionTransicion));
    }

    private System.Collections.IEnumerator EscalarSuavemente(GameObject obj, Vector3 escalaFinal, float duracion)
    {
        float tiempo = 0f;
        Vector3 escalaInicial = obj.transform.localScale;

        while (tiempo < duracion && obj != null) // Verificar si el objeto existe en cada iteración
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);

            // Verificar nuevamente antes de acceder al transform
            if (obj != null)
            {
                obj.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
            }
            else
            {
                yield break; // Salir de la corrutina si el objeto fue destruido
            }

            yield return null;
        }

        // Verificar una última vez antes de establecer la escala final
        if (obj != null)
        {
            obj.transform.localScale = escalaFinal;
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

            float distanciaX = Mathf.Abs(nubesActivas[i].transform.position.x - jugador.position.x);
            float distanciaY = Mathf.Abs(nubesActivas[i].transform.position.y - jugador.position.y);
            float distanciaZ = Mathf.Abs(nubesActivas[i].transform.position.z - jugador.position.z);
            float margenXY = 20f;
            float margenZ = 30f;

            if (distanciaX > margenXY || distanciaY > margenXY || distanciaZ > margenZ)
            {
                Destroy(nubesActivas[i]);
                nubesActivas.RemoveAt(i);
            }
        }
    }
}