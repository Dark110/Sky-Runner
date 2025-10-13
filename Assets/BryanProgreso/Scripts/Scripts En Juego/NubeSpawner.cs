using UnityEngine;
using System.Collections.Generic;

public class NubeSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabNube;

    [Header("Área de spawn alrededor del jugador")]
    public float rangoX = 20f;
    public float rangoY = 10f;
    public float distanciaZ = -25f;

    [Header("Spawn")]
    public float tiempoEntreSpawn = 1f;
    public int maxNubes = 15;

    [Header("Escala y transición")]
    public float escalaInicial = 0.2f;
    public float duracionTransicion = 2f;

    [Header("Movimiento")]
    public float velocidadMin = 1f;
    public float velocidadMax = 3f;

    private float temporizador;
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
        // Posición inicial alrededor del jugador, delante en -Z
        float x = jugador.position.x + Random.Range(-rangoX, rangoX);
        float y = jugador.position.y + Random.Range(-rangoY, rangoY);
        float z = jugador.position.z + distanciaZ;

        GameObject nuevaNube = Instantiate(prefabNube, new Vector3(x, y, z), Quaternion.identity);
        nubesActivas.Add(nuevaNube);

        Cloud nubeScript = nuevaNube.GetComponent<Cloud>();
        if (nubeScript != null)
        {
            nubeScript.velocidad = Random.Range(velocidadMin, velocidadMax);
            nubeScript.objetivoZ = jugador.position.z + 10f; // hasta pasar al jugador
        }

        // Escalado progresivo
        Vector3 escalaFinal = nuevaNube.transform.localScale;
        nuevaNube.transform.localScale = escalaFinal * escalaInicial;
        StartCoroutine(EscalarSuavemente(nuevaNube, escalaFinal, duracionTransicion));
    }

    private System.Collections.IEnumerator EscalarSuavemente(GameObject obj, Vector3 escalaFinal, float duracion)
    {
        float tiempo = 0f;
        Vector3 escalaInicialLocal = obj.transform.localScale;

        while (tiempo < duracion && obj != null)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            obj.transform.localScale = Vector3.Lerp(escalaInicialLocal, escalaFinal, t);
            yield return null;
        }

        if (obj != null)
            obj.transform.localScale = escalaFinal;
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

            // Si la nube ya pasó más allá del jugador (en Z), destruirla
            if (nubesActivas[i].transform.position.z > jugador.position.z + 5f)
            {
                Destroy(nubesActivas[i]);
                nubesActivas.RemoveAt(i);
            }
        }
    }
}
