using UnityEngine;

public class PowerUpMover : MonoBehaviour
{
    [HideInInspector] public float velocidadZ = 2f;

    void Update()
    {
        transform.Translate(Vector3.forward * velocidadZ * Time.deltaTime);
    }
}
