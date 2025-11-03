using UnityEngine;
using UnityEngine.UI;

public class PowerUpTrigger : MonoBehaviour
{
    public TipoPowerUp tipo;
    public float duracion = 5f;
    public float valorExtra = 5f; // solo para velocidad
    public Image uiIndicador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.ActivarPowerUp(tipo, duracion, valorExtra, uiIndicador);
            Destroy(gameObject); // desaparece al recogerlo
        }
    }
}