using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        GameObject nuevaDiana = PoolManager.Instance.GetFromPool(prefabDiana, new Vector3(x, y, z), Quaternion.identity);
        DianaPuntos script = nuevaDiana.GetComponent<DianaPuntos>();
        if (script != null)
            script.Initialize();

        dianasActivas.Add(nuevaDiana);
    }

    void LimpiarDianas()
    {
        for (int i = dianasActivas.Count - 1; i >= 0; i--)
        {
            if (!dianasActivas[i].activeInHierarchy)
                dianasActivas.RemoveAt(i);
        }
    }
}
