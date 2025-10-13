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
    public float variacionEscala = 0.25f; // +-25% de variación de tamaño

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

        // Rotación aleatoria (solo en Z si son sprites planos)
        Quaternion rotacionAleatoria = Quaternion.Euler(
            Random.Range(-10f, 10f),  // leve inclinación X
            Random.Range(0f, 360f),   // rotación completa Y
            Random.Range(-10f, 10f)   // leve inclinación Z
        );

        GameObject nuevaNube = Instantiate(prefabNube, new Vector3(x, y, z), rotacionAleatoria);
        nubesActivas.Add(nuevaNube);

        // Variación de escala individual (entre 0.75x y 1.25x)
        float factorEscala = 1f + Random.Range(-variacionEscala, variacionEscala);
        Vector3 escalaFinal = prefabNube.transform.localScale * factorEscala;

        // Escala inicial más pequeña (para efecto de aparición)
        nuevaNube.transform.localScale = escalaFinal * escalaInicial;
        StartCoroutine(EscalarSuavemente(nuevaNube, escalaFinal, duracionTransicion));

        // Configurar el movimiento
        Cloud nubeScript = nuevaNube.GetComponent<Cloud>();
        if (nubeScript != null)
        {
            nubeScript.velocidad = Random.Range(velocidadMin, velocidadMax);
            nubeScript.objetivoZ = jugador.position.z + 10f; // que pase al jugador
        }
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
