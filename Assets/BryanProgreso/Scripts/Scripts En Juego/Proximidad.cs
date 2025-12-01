using UnityEngine;

public class ProximitySound : MonoBehaviour
{
    // Nombre del SFX a reproducir (ej: "Whoosh")
    public string whooshSFXName = "Whoosh";

    [Tooltip("Distancia en el eje Z donde el whoosh debe activarse (detrás del jugador).")]
    public float triggerOffsetZ = 1.0f;

    private Transform jugador;
    private bool played = false;

    void Start()
    {
        // Encuentra al jugador para rastrear su posición.
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            jugador = playerObj.transform;
    }

    void Update()
    {
        if (jugador == null || played) return;

        if (transform.position.z > jugador.position.z + triggerOffsetZ)
        {
            // ⭐ LLAMADA AL SONIDO WHOOSH
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(whooshSFXName);
            }

            // Asegurarse de que no se reproduzca de nuevo (es un Whoosh por paso)
            played = true;
        }
    }
}