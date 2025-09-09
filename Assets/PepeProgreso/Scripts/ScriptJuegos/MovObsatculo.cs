using UnityEngine;

public class MovObstaculo : MonoBehaviour
{
    public float VelObstaculo = 5f;

    void Update()
    {
        // Movimiento hacia arriba
        transform.Translate(Vector3.up * VelObstaculo * Time.deltaTime);

        // Destruir el objeto si sale de la pantalla
        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
    }
}