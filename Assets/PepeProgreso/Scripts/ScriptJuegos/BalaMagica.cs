using UnityEngine;

public class BalaMagica : MonoBehaviour
{
    public GameObject prefabBalaMagica; // Prefab de la bala
    public Transform jugadorDisparo;    // Desde donde saldrá la bala

    private void Start()
    {
        // Asignar tag automáticamente
        gameObject.tag = "PowerUpBala";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("piloto"))
        {
            Debug.Log("¡Jugador tocó el Power-Up de Bala!");
            Instantiate(prefabBalaMagica, jugadorDisparo.position, jugadorDisparo.rotation);
            Destroy(gameObject);
        }
    }
}