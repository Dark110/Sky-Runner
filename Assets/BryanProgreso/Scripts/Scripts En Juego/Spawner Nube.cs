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

    [Header("Transición de aparición (fade)")]
    public float duracionFade = 2f;
    [Range(0f, 1f)] public float alphaInicial = 0f;
    [Range(0f, 1f)] public float alphaFinal = 1f;

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
        // Posición inicial: delante del jugador en -Z
        float x = jugador.position.x + Random.Range(-rangoX, rangoX);
        float y = jugador.position.y + Random.Range(-rangoY, rangoY);
        float z = jugador.position.z + distanciaZ;

        Quaternion rotacionAleatoria = Quaternion.Euler(
            Random.Range(-10f, 10f),
            Random.Range(0f, 360f),
            Random.Range(-10f, 10f)
        );

        GameObject nuevaNube = Instantiate(prefabNube, new Vector3(x, y, z), rotacionAleatoria);
        nubesActivas.Add(nuevaNube);

        // Aparecer con fade en lugar de escala
        StartCoroutine(FadeInNube(nuevaNube, duracionFade, alphaInicial, alphaFinal));

        // Configurar movimiento
        Cloud nubeScript = nuevaNube.GetComponent<Cloud>();
        if (nubeScript != null)
        {
            nubeScript.velocidad = Random.Range(velocidadMin, velocidadMax);
            nubeScript.objetivoZ = jugador.position.z + 10f;
        }
    }

    private System.Collections.IEnumerator FadeInNube(GameObject obj, float duracion, float alphaInicio, float alphaFin)
    {
        if (obj == null) yield break;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null || renderer.material == null) yield break;

        // Clonar el material para no modificar el prefab original
        Material mat = renderer.material;
        Color color = mat.color;
        color.a = alphaInicio;
        mat.color = color;

        float tiempo = 0f;
        while (tiempo < duracion && obj != null)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            color.a = Mathf.Lerp(alphaInicio, alphaFin, t);
            mat.color = color;
            yield return null;
        }

        if (obj != null)
        {
            color.a = alphaFin;
            mat.color = color;
        }
    }

    void LimpiarNubes()
    {
        for (int i = nubesActivas.Count - 1; i >= 0; i--)
        {
            GameObject nube = nubesActivas[i];
            if (nube == null)
            {
                nubesActivas.RemoveAt(i);
                continue;
            }

            // Si ya pasó al jugador en el eje Z, destruirla
            if (nube.transform.position.z > jugador.position.z + 5f)
            {
                Destroy(nube);
                nubesActivas.RemoveAt(i);
            }
        }
    }
}
