using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform jugadorCam;                  // El objeto (jugador) a seguir
    public Vector3 offset = new Vector3(0, 5, -8); // Distancia de la cámara al jugador
    public float movimientoCam = 2.5f;           // Suavidad del movimiento
    public bool verJugador = true;               // Opcional: que mire al jugador

    void LateUpdate()
    {
        if (jugadorCam == null) return;

        // Posición deseada
        Vector3 posicionDeseada = jugadorCam.position + offset;

        // Suavizar movimiento
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, movimientoCam * Time.deltaTime);

        // Aplicar movimiento
        transform.position = posicionSuavizada;

        // Mirar al jugador si está activado
        if (verJugador)
        {
            transform.LookAt(jugadorCam);
        }
    }
}