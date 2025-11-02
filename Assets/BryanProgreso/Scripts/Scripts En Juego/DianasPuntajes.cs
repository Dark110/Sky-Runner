using UnityEngine;

public class DianasPuntajes : MonoBehaviour
{
    [Header("Configuración de puntos")]
    public int PuntosObjetos = 10; // Cuántos puntos da este objeto

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador tocó el objeto
        if (other.CompareTag("Player"))
        {
            // Suma los puntos al sistema de puntuación
            ScoreManager.Instance.AddScore(PuntosObjetos);

            // Destruye este objeto al tocarlo
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Asegura que el objeto no se mueva por física
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.isKinematic = true;
    }
}