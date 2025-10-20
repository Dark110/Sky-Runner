using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DianaSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabDiana;

    [Header("Área de spawn alrededor del jugador")]
    public float rangoX = 15f;
    public float rangoY = 8f;
    public float distanciaZ = -40f;

    [Header("Spawn")]
    public float tiempoEntreSpawn = 4f;
    public int maxDianas = 5;
    public float duracionFade = 0.8f; // duración del efecto fade-in
    public float velocidadZMin = 12f; // velocidad mínima hacia adelante
    public float velocidadZMax = 18f; // velocidad máxima hacia adelante

    private float temporizador;
    private List<GameObject> dianasActivas = new List<GameObject>();

    void Update()
    {
        if (jugador == null || prefabDiana == null) return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f && dianasActivas.Count < maxDianas)
        {
            GenerarDiana();
            temporizador = tiempoEntreSpawn;
        }

        LimpiarDianas();
    }

    void GenerarDiana()
    {
        float x = jugador.position.x + Random.Range(-rangoX, rangoX);
        float y = jugador.position.y + Random.Range(-rangoY, rangoY);
        float z = jugador.position.z + distanciaZ;

        Quaternion rotacion = Quaternion.identity;
        GameObject nuevaDiana = Instantiate(prefabDiana, new Vector3(x, y, z), rotacion);

        dianasActivas.Add(nuevaDiana);

        // Fade-in visual
        StartCoroutine(FadeInDiana(nuevaDiana));

        // Configurar velocidad aleatoria de la diana
        DianaPuntos dianaScript = nuevaDiana.GetComponent<DianaPuntos>();
        if (dianaScript != null)
        {
            dianaScript.velocidadZ = Random.Range(velocidadZMin, velocidadZMax);
        }
    }

    IEnumerator FadeInDiana(GameObject diana)
    {
        Renderer rend = diana.GetComponent<Renderer>();
        if (rend == null) yield break;

        Material mat = rend.material;
        Color color = mat.color;
        color.a = 0f;
        mat.color = color;

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / duracionFade);
            mat.color = color;
            yield return null;
        }

        color.a = 1f;
        mat.color = color;
    }

    void LimpiarDianas()
    {
        for (int i = dianasActivas.Count - 1; i >= 0; i--)
        {
            if (dianasActivas[i] == null)
            {
                dianasActivas.RemoveAt(i);
                continue;
            }

            if (jugador != null && dianasActivas[i].transform.position.z > jugador.position.z + 10f)
            {
                Destroy(dianasActivas[i]);
                dianasActivas.RemoveAt(i);
            }
        }
    }
}
