using UnityEngine;

public class DisparoMagico : MonoBehaviour
{
    public float VelDisparo = 20f; // La velocidad de la bala al disparar
    public float TiempoDisparo = 5f; // Tiempo de vida

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destrucción de la bala
        Destroy(gameObject, TiempoDisparo);
    }

    // Update is called once per frame
    void Update()
    {
        // El disparo hacia adelante en dirección de Z
        transform.Translate(Vector3.forward * VelDisparo * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If cuando la bala toca un obstáculo
        if (other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}