using UnityEngine;
using System.Collections.Generic;

public class Obstaculo : MonoBehaviour
{
    public GameObject PrefabObstaculo;
    public float AparicionObstaculos = 1f; // tiempo entre cada spawn
    public float RangoRandom = 2f;
    public float PosicionY = 0f;
    public float PosicionZ = 0f;
    public int MaximoObs = 10;

    private float temporizador = 0f;
    private List<GameObject> obstaculosActivos = new List<GameObject>();

    void Update()
    {
        temporizador -= Time.deltaTime;

        // Verificar si algún obstáculo fue destruido y eliminarlo de la lista
        for (int i = obstaculosActivos.Count - 1; i >= 0; i--)
        {
            if (obstaculosActivos[i] == null)
            {
                obstaculosActivos.RemoveAt(i);
            }
        }

        // Crear un nuevo obstáculo si hay espacio y pasó el tiempo
        if (temporizador <= 0f && obstaculosActivos.Count < MaximoObs)
        {
            GenerarObstaculo();
            temporizador = AparicionObstaculos;
        }
    }

    void GenerarObstaculo()
    {
        if (PrefabObstaculo != null)
        {
            float x = Random.Range(-RangoRandom, RangoRandom);
            Vector3 spawnPos = new Vector3(x, PosicionY, PosicionZ);

            GameObject nuevoObs = Instantiate(PrefabObstaculo, spawnPos, Quaternion.identity);
            nuevoObs.tag = "Obstaculo";

            obstaculosActivos.Add(nuevoObs);
        }
        else
        {
            Debug.LogWarning("PrefabObstaculo no está asignado en el Inspector!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MovimientoJugador PilotoPlayer = collision.gameObject.GetComponent<MovimientoJugador>();

        // El if original que activa el jugador al tocar un obstaculo
        if (PilotoPlayer != null)
        {
            // El if y el else para saber si recibio daño o es invunerable el jugador
            if (PilotoPlayer.Invunerabilidad)
            {
                Debug.Log("Esta recibiendo daño");
            }
            else
            {
                Debug.Log("El jugador tiene el power up de la invunerabilidad");
            }
        }
    }
}