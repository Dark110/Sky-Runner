using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStarted;

    [Header("Cuenta regresiva")]
    public TMP_Text textoCuenta;
    public int tiempoInicial = 3;

    [Header("Spawners")]
    public ObstaculoSpawnerFinal[] spawnersObstaculos;
    public NubeSpawner[] spawnersNubes;
    public DianaSpawner[] spawnersDianas;

    [Header("Spawners de PowerUps")]
    public PowerUpSpawner[] spawnersPowerUps;

    private void Start()
    {
        
        DesactivarSpawners();

        StartCoroutine(Countdown());
    }

    private void DesactivarSpawners()
    {
        // Desactiva todos los Spawners.
        foreach (var spawner in spawnersObstaculos)
            spawner.enabled = false;

        foreach (var spawner in spawnersNubes)
            spawner.enabled = false;

        foreach (var spawner in spawnersDianas)
            spawner.enabled = false;

        foreach (var spawner in spawnersPowerUps)
            spawner.enabled = false;
    }

    private void ActivarSpawners()
    {
        // Activa todos los Spawners cuando el juego comienza.
        foreach (var spawner in spawnersObstaculos)
            spawner.enabled = true;

        foreach (var spawner in spawnersNubes)
            spawner.enabled = true;

        foreach (var spawner in spawnersDianas)
            spawner.enabled = true;

        foreach (var spawner in spawnersPowerUps)
            spawner.enabled = true;
    }

    private IEnumerator Countdown()
    {
        int tiempo = tiempoInicial;

        while (tiempo > 0)
        {
            if (textoCuenta != null)
                textoCuenta.text = tiempo.ToString();

            // CLAVE: Usamos WaitForSecondsRealtime. Esto sigue contando aunque Time.timeScale sea 0 (pausa).
            yield return new WaitForSecondsRealtime(1f);

            tiempo--;
        }

        if (textoCuenta != null)
            textoCuenta.text = "¡0!";

        // ACTIVAción de Spawners: SOLO ocurre aquí, al final de la corrutina.
        ActivarSpawners();

        // Notificamos que la fase de inicio terminó.
        OnGameStarted?.Invoke();

        yield return new WaitForSecondsRealtime(1f);

        if (textoCuenta != null)
            textoCuenta.text = "";
    }
}