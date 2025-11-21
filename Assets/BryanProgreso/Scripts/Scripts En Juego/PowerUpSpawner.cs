using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab del PowerUp")]
    public GameObject powerUpPrefab; // Asigna aquí tu prefab "PowerUp"

    [Header("Cantidad y rango de aparición")]
    public int cantidadPowerUps = 2; // Cuántos power ups aparecerán
    public Vector3 rango = new Vector3(20f, 0f, 20f); // Área de aparición

    private void Start()
    {
        if (powerUpPrefab == null)
        {
            Debug.LogError("⚠️ No has asignado el prefab de PowerUp en el inspector");
            return;
        }

        // Generar los power ups
        for (int i = 0; i < cantidadPowerUps; i++)
        {
            Vector3 posicion = transform.position + new Vector3(
                Random.Range(-rango.x, rango.x),
                Random.Range(-rango.y, rango.y),
                Random.Range(-rango.z, rango.z)
            );

            Instantiate(powerUpPrefab, posicion, Quaternion.identity);
            Debug.Log("spam de poder");
        }
    }
}
