using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Prefabs")]
    public GameObject prefabPowerUp;

    [Header("Control")]
    public bool spawnerActivo = true;

    [Header("Área de Spawn")]
    public float rangoX = 3f;
    public float rangoY = 2f;
    public float distanciaZMin = 4f;
    public float distanciaZMax = 8f;

    [Header("Frecuencia de Spawn")]
    public float tiempoEntreSpawn = 6f;
    private float temporizador = 0f;

    [Header("Límite de PowerUps")]
    public int maxPowerUps = 3;
    public float distanciaDetras = 8f;

    [Header("Movimiento del PowerUp")]
    public float velocidadZ = 2f;

    private List<GameObject> powerUpsActivos = new List<GameObject>();

    void Update()
    {
        if (!spawnerActivo || jugador == null || prefabPowerUp == null) return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f && powerUpsActivos.Count < maxPowerUps)
        {
            GenerarPowerUp();
            temporizador = tiempoEntreSpawn;
        }

        LimpiarPowerUps();
    }

    void GenerarPowerUp()
    {
        float x = jugador.position.x + Random.Range(-rangoX, rangoX);
        float y = jugador.position.y + Random.Range(-rangoY, rangoY);
        float z = jugador.position.z - Random.Range(distanciaZMin, distanciaZMax);

        GameObject nuevoPowerUp = Instantiate(prefabPowerUp, new Vector3(x, y, z), Quaternion.identity);
        powerUpsActivos.Add(nuevoPowerUp);

        // Añadimos el movimiento automático
        PowerUpMover mover = nuevoPowerUp.GetComponent<PowerUpMover>();
        if (mover == null)
            mover = nuevoPowerUp.AddComponent<PowerUpMover>();

        mover.velocidadZ = velocidadZ;
    }

    void LimpiarPowerUps()
    {
        for (int i = powerUpsActivos.Count - 1; i >= 0; i--)
        {
            GameObject obj = powerUpsActivos[i];

            if (obj == null)
            {
                powerUpsActivos.RemoveAt(i);
                continue;
            }

            // Si pasa detrás del jugador, destruir
            if (obj.transform.position.z > jugador.position.z + distanciaDetras)
            {
                Destroy(obj);
                powerUpsActivos.RemoveAt(i);
            }
        }
    }
}
