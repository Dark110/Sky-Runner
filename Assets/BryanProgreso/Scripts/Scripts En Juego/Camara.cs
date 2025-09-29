using UnityEngine;

public class Camara3raPersona : MonoBehaviour
{
    [Header("Jugador a seguir")]
    public Transform jugador;

    [Header("Offset de la cámara")]
    public Vector3 offset = new Vector3(0, 2, -5); // altura y distancia detrás

    void LateUpdate()
    {
        if (jugador == null) return;

        // Posición detrás del jugador usando su forward
        Vector3 posicionDetras = jugador.position - jugador.forward * Mathf.Abs(offset.z) + Vector3.up * offset.y;

        // Aplicar posición
        transform.position = posicionDetras;

    }
}