using JetBrains.Annotations;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float Vel = 2000f;
    public bool ControlSuavidad = true;
    public float NivelSuavidad = 2f;
    public float IzqDer = 5000f;

    private Vector3 JugadorMov;

    private bool ladoIzquierda = false;
    private bool ladoDerecho = false;

    private void Update()
    {

#if UNITY_EDITOR
        //detecta el movimiento del celular de Izquierda y Derecha
        IzqDer = Input.GetAxis("Horizontal");

        Debug.Log(IzqDer);
#else
 //detecta el movimiento del celular de Izquierda y Derecha
        IzqDer = Input.acceleration.x;
#endif

        //Guarda el movimento en su actualizacion
        JugadorMov = new Vector3(IzqDer, 0, 0);
        transform.Translate(JugadorMov * Time.deltaTime);

        // el if y else para que el jugador escoga que lado
    }

    public void izquierda(bool lado)
    {
        ladoIzquierda = lado;
    }

    public void derecha(bool lado)
    {
        ladoDerecho = lado;
    }
}
