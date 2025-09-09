using UnityEngine;

public class Camara : MonoBehaviour
{
    [Header("Jugador a seguir")]
    public Transform jugadorCam;                  // El objeto (jugador) a seguir

    [Header("Configuración de la cámara")]
    public Vector3 offset = new Vector3(0, 0, -8); // Distancia de la cámara al jugador
    [Range(0.1f, 10f)]
    public float movimientoCam = 2.5f;           // Suavidad del movimiento
    public bool verJugador = true;               // Si la cámara debe mirar al jugador

    void LateUpdate()
    {
        if (jugadorCam == null) return; // Seguridad por si no asignaste el jugador

        // Calculamos la posición deseada (jugador + offset)
        Vector3 posicionDeseada = jugadorCam.position + offset;

        // Movemos la cámara suavemente hacia esa posición
        Vector3 posicionSuavizada = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            movimientoCam * Time.deltaTime
        );

        // Aplicamos la posición suavizada
        transform.position = posicionSuavizada;

        // Hacemos que la cámara mire al jugador si está activado
        if (verJugador)
        {
            transform.LookAt(jugadorCam);
        }
    }
}