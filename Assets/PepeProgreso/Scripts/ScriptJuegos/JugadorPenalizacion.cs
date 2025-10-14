using UnityEngine;

public class JugadorPenalizacion : MonoBehaviour
{
    [Header("Penalizacion tiempo")]
    public float SegundoQuieto = 5f;
    private float TiempoEstatico = 0f;

    [Header("Referencia al script de puntaje")]
    public PuntajeJugador puntajeJugador; // Asigna tu script de puntaje en el inspector

    void Update()
    {
        float InputHorizontal = Input.GetAxis("Horizontal");
        float InputVertical = Input.GetAxis("Vertical");

        // Si no hay movimiento
        if (Mathf.Abs(InputHorizontal) < 0.01f && Mathf.Abs(InputVertical) < 0.01f)
        {
            TiempoEstatico += Time.deltaTime;
        }
        else
        {
            TiempoEstatico = 0f;
        }

        // Si ha estado quieto demasiado tiempo
        if (TiempoEstatico >= SegundoQuieto)
        {
            Debug.Log("Jugador penalizado por estar quieto");

            if (puntajeJugador != null)
            {
                puntajeJugador.PerderPuntos(10); // Llama a tu método de perder puntos
            }

            TiempoEstatico = 0f; // reinicia el contador
        }
    }
}