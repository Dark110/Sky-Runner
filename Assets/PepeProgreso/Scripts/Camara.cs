using UnityEngine;

public class Camara : MonoBehaviour
{
    [Header("Jugador a seguir")]
    public Transform jugadorCam;                  // El objeto (jugador) a seguir

    [Header("Configuraci�n de la c�mara")]
    public Vector3 offset = new Vector3(0, 0, -8); // Distancia de la c�mara al jugador
    [Range(0.1f, 10f)]
    public float movimientoCam = 2.5f;           // Suavidad del movimiento
    public bool verJugador = true;               // Si la c�mara debe mirar al jugador

    void LateUpdate()
    {
        if (jugadorCam == null) return; // Seguridad por si no asignaste el jugador

        // Calculamos la posici�n deseada (jugador + offset)
        Vector3 posicionDeseada = jugadorCam.position + offset;

        // Movemos la c�mara suavemente hacia esa posici�n
        Vector3 posicionSuavizada = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            movimientoCam * Time.deltaTime
        );

        // Aplicamos la posici�n suavizada
        transform.position = posicionSuavizada;

        // Hacemos que la c�mara mire al jugador si est� activado
        if (verJugador)
        {
            transform.LookAt(jugadorCam);
        }
    }
}