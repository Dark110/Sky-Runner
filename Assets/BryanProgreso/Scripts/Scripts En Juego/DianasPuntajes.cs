using UnityEngine;

public class DianasPuntajes : MonoBehaviour
{
    public int PuntosObjetos = 10; // Cuántos puntos da este objeto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga el tag "Player"
        {
            // Suma puntos al jugador
            ScoreManager.Instance.AddScore(PuntosObjetos);

            // Destruye este objeto al tocarlo
            Destroy(gameObject);
        }
    }
}