using UnityEngine;

public class PropRotator : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad de rotación en grados por segundo.")]
    public float velocidadRotacion = 45f;

    [Tooltip("Ejes en los que el objeto rotará (X, Y, Z).")]
    public Vector3 ejesRotacion = new Vector3(0, 1, 0);

    [Tooltip("Si está activado, la rotación será suave y continua (usando deltaTime).")]
    public bool rotacionActiva = true;

    void Update()
    {
        if (rotacionActiva)
        {
            // Rotar en base al tiempo para suavidad y framerate independiente
            transform.Rotate(ejesRotacion * velocidadRotacion * Time.deltaTime, Space.Self);
        }
    }

    // Método opcional para activar/desactivar desde otros scripts
    public void ActivarRotacion(bool activar)
    {
        rotacionActiva = activar;
    }
}
