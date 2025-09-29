using UnityEngine;
using TMPro;

public class Temporizador : MonoBehaviour
{
    public TMP_Text UI_Tiempo;
    public float MinutoTiempo = 60f;

    // Hacemos público el contador
    public float tiempoActual;

    // Variable para saber si el jugador sigue vivo
    public bool jugadorVivo = true;

    private bool juego = true;
    private int segundos;
    private int minutos;

    void Start()
    {
        tiempoActual = MinutoTiempo;
    }

    void Update()
    {
        if (juego)
        {
            tiempoActual -= Time.deltaTime;
            tiempoActual = Mathf.Max(0, tiempoActual);

            segundos = Mathf.FloorToInt(tiempoActual % 60);
            minutos = Mathf.FloorToInt(tiempoActual / 60);

            UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            if (tiempoActual <= 0)
            {
                juego = false;
                JuegoTerminado();
            }
        }
    }

    void JuegoTerminado()
    {
        Debug.Log("Game OVER");
        UI_Tiempo.text = "00:00";
    }
}