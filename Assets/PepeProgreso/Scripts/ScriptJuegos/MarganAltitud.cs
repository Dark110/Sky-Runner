using UnityEngine;
using UnityEngine.UI;

public class MarganAltitud : MonoBehaviour
{
    public Transform piloto;
    public RectTransform UI_Altitud;
    public Camera PrinCamara;
    public Vector2 AreaPantalla = new Vector2(100f, 100f); //matgen de la camara

    // Update is called once per frame
    void Update()
    {
        if (piloto == null || UI_Altitud == null || PrinCamara == null) return;

        // Gurada las coordenadas del objeto y luego las convierte en coordenada de pantalla
        Vector3 MundoPocisison = PrinCamara.WorldToScreenPoint(piloto.position);

        // Condicion para normalizar e invertir signos del jugador en caso de que el jugador esta detras de la camara
        if (MundoPocisison.z < 0)
        {
            MundoPocisison *= -1; //Multiplicacion por un menos para normalizar y cambiar la pocisison
        }

        // --- MARCADOR DE ALTURA ---
        // Solo usamos la altura (eje Y del jugador) y la representamos en UI
        float AltitudPilotop = piloto.position.y;

        // Normalizamos la altitud dentro del rango de pantalla
        float alturaPantalla = Mathf.Clamp(AltitudPilotop, AreaPantalla.y, Screen.height - AreaPantalla.y);

        // Movemos el marcador en el eje Y de la pantalla (como barra de altitud)
        UI_Altitud.position = new Vector3(Screen.width - AreaPantalla.x, alturaPantalla, 0);

        // Texto del marcador de altitud
        Text Altura = UI_Altitud.GetComponentInChildren<Text>();
        if (Altura != null)
            Altura.text = "Altura: " + Mathf.RoundToInt(AltitudPilotop) + " m";
    }
}