using UnityEngine;

public class VolumenMaestro : MonoBehaviour
{
    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
    }
}
