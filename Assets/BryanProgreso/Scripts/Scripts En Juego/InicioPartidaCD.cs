using UnityEngine;
using TMPro; // Para mostrar la cuenta regresiva

public class GameManager : MonoBehaviour
{
    [Header("Cuenta regresiva")]
    public TMP_Text textoCuenta;   // Texto en la UI para mostrar la cuenta
    public int tiempoInicial = 3;  // De 3 a 0

    [Header("Spawners")]
    public ObstaculoSpawnerCaoticoFinal[] spawners; // Todos los spawners del nivel

    void Start()
    {
        // Antes de empezar, desactivamos los spawners
        foreach (var spawner in spawners)
            spawner.spawnerActivo = false;

        // Iniciamos la cuenta regresiva
        StartCoroutine(Countdown());
    }

    private System.Collections.IEnumerator Countdown()
    {
        int tiempo = tiempoInicial;

        while (tiempo > 0)
        {
            if (textoCuenta != null)
                textoCuenta.text = tiempo.ToString();

            yield return new WaitForSeconds(1f);
            tiempo--;
        }

        // Último "0"
        if (textoCuenta != null)
            textoCuenta.text = "¡YA!";

        // Activamos los spawners
        foreach (var spawner in spawners)
            spawner.spawnerActivo = true;

        // Limpiamos el texto después de un segundo
        yield return new WaitForSeconds(1f);
        if (textoCuenta != null)
            textoCuenta.text = "";
    }
}
