using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    // Mantenemos el evento por si otros scripts lo necesitan
    public static event Action OnGameStarted;

    [Header("Cuenta regresiva")]
    public TMP_Text textoCuenta;
    public int tiempoInicial = 3;

    [Header("Referencias UI")]
    // Referencia al objeto Game Object del botón de Pausa en la UI
    public GameObject botonPausaUI;

    [Header("Spawners")]
    public ObstaculoSpawnerFinal[] spawnersObstaculos;
    public NubeSpawner[] spawnersNubes;
    public DianaSpawner[] spawnersDianas;

    [Header("Spawners de PowerUps")]
    public PowerUpSpawner[] spawnersPowerUps;

    private void Start()
    {
        // Ocultamos el botón de Pausa antes de empezar la cuenta
        if (botonPausaUI != null)
        {
            botonPausaUI.SetActive(false);
        }

        // Desactivamos los Spawners
        DesactivarSpawners();

        // Iniciamos el Countdown
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

            // Usamos WaitForSecondsRealtime para que la pausa no detenga la cuenta
            yield return new WaitForSecondsRealtime(1f);

            tiempo--;
        }

        if (textoCuenta != null)
            textoCuenta.text = "¡0!";

        // Activación de Spawners
        ActivarSpawners();

        // Activación del Botón de Pausa (permitimos al jugador pausar)
        if (botonPausaUI != null)
        {
            botonPausaUI.SetActive(true);
        }

        // Notificamos que la fase de inicio terminó.
        OnGameStarted?.Invoke();

        yield return new WaitForSecondsRealtime(1f);

        if (textoCuenta != null)
            textoCuenta.text = "";
    }
}