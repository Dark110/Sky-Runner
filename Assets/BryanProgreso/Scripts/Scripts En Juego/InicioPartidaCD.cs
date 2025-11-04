using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Cuenta regresiva")]
    public TMP_Text textoCuenta;
    public int tiempoInicial = 3; // de 3 a 0

    [Header("Spawners")]
    public ObstaculoSpawnerCaoticoFinal[] spawnersObstaculos;
    public NubeSpawner[] spawnersNubes;
    public DianaSpawner[] spawnersDianas; // spawner de dianas o anillos

    [Header("Spawners de PowerUps")]
    public PowerUpSpawner[] spawnersPowerUps; // ← Nuevo

    private void Start()
    {
        // Desactivar todos los spawners antes de comenzar
        foreach (var spawner in spawnersObstaculos)
            spawner.spawnerActivo = false;

        foreach (var spawner in spawnersNubes)
            spawner.enabled = false;

        foreach (var spawner in spawnersDianas)
            spawner.enabled = false;

        foreach (var spawner in spawnersPowerUps)
            spawner.enabled = false; // desactivar powerups al inicio

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        int tiempo = tiempoInicial;

        while (tiempo > 0)
        {
            if (textoCuenta != null)
                textoCuenta.text = tiempo.ToString();

            yield return new WaitForSeconds(1f);
            tiempo--;
        }

        if (textoCuenta != null)
            textoCuenta.text = "¡0!";

        // Activar todos los spawners
        foreach (var spawner in spawnersObstaculos)
            spawner.spawnerActivo = true;

        foreach (var spawner in spawnersNubes)
            spawner.enabled = true;

        foreach (var spawner in spawnersDianas)
            spawner.enabled = true;

        foreach (var spawner in spawnersPowerUps)
            spawner.enabled = true; // ← activar powerups

        // Limpiar texto después de un segundo
        yield return new WaitForSeconds(1f);
        if (textoCuenta != null)
            textoCuenta.text = "";
    }
}
