using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    [Header("Cuenta regresiva")]
    public TMP_Text textoCuenta;  
    public int tiempoInicial = 3;  // De 3 a 0

    [Header("Spawners")]
    public ObstaculoSpawnerCaoticoFinal[] spawners;

    void Start()
    {
        // Antes de empezar, desactivamos los spawners
        foreach (var spawner in spawners)
            spawner.spawnerActivo = false;
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

        if (textoCuenta != null)
            textoCuenta.text = "¡0!";

        // Activar spawners
        foreach (var spawner in spawners)
            spawner.spawnerActivo = true;

        // Limpiar texto
        yield return new WaitForSeconds(1f);
        if (textoCuenta != null)
            textoCuenta.text = "";
    }
}
