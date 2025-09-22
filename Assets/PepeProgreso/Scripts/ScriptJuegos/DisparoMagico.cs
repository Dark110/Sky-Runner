using UnityEngine;

public class DisparoMagico : MonoBehaviour
{
    public float VelDisparo = 20f; // La velocidad de la bala al disparar
    public float TiempoDisparo = 5f; // Tiempo de vida

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destrucci�n de la bala
        Destroy(gameObject, TiempoDisparo);
    }

    // Update is called once per frame
    void Update()
    {
        // El disparo hacia adelante en direcci�n de Z
        transform.Translate(Vector3.forward * VelDisparo * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If cuando la bala toca un obst�culo
        if (other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}