using UnityEngine;

public class MovObstaculo : MonoBehaviour
{
    public float VelObstaculo = 5f;
    public bool moverAdelante = true; // Si es true, se mueve hacia adelante (Z+); si es false, hacia atrás (Z-)

    void Update()
    {
        // Movimiento en Z
        float direccion = moverAdelante ? 1f : -1f;
        transform.Translate(Vector3.forward * direccion * VelObstaculo * Time.deltaTime);

        // Destruir el objeto si sale de la pantalla (ajusta el valor según tu escena)
        if (transform.position.z > 60f || transform.position.z < -60f)
        {
            Destroy(gameObject);
        }
    }
}   