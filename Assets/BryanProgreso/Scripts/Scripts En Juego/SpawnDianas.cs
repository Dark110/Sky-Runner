using UnityEngine;

public class SpawnDianas : MonoBehaviour
{
    [Header("Ajustes del Spawn")]
    public GameObject PrefabDianas;   // Prefab de la diana
    public int Inicio = 5;             // Cantidad inicial de dianas
    public float SegundosSpawns = 5f;  // Intervalo de aparición
    public Vector3 AreaSpawn = new Vector3(10, 1, 10); // Tamaño del área de spawn

    private Vector3 spawnCenter;       // Centro fijo del spawn

    void Start()
    {
        // Guardamos la posición inicial como centro de spawn
        spawnCenter = transform.position;

        // Generamos las dianas iniciales
        for (int i = 0; i < Inicio; i++)
        {
            NuevasDianas();
        }

        // Generamos nuevas dianas cada SegundosSpawns segundos
        InvokeRepeating(nameof(NuevasDianas), SegundosSpawns, SegundosSpawns);
    }

    private void NuevasDianas()
    {
        if (PrefabDianas == null)
        {
            Debug.LogWarning("No está asignado el prefab de la Diana");
            return;
        }

        // Posición aleatoria dentro del área, centrada en spawnCenter
        Vector3 posicion = spawnCenter + new Vector3(
            Random.Range(-AreaSpawn.x / 2, AreaSpawn.x / 2),
            Random.Range(-AreaSpawn.y / 2, AreaSpawn.y / 2),
            Random.Range(-AreaSpawn.z / 2, AreaSpawn.z / 2)
        );

        // Instanciamos la diana
        GameObject diana = Instantiate(PrefabDianas, posicion, Quaternion.identity);

        // Si tiene Rigidbody, lo hacemos kinematic para que no se mueva
        Rigidbody rb = diana.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Centro del spawn en tiempo de edición
        Vector3 center = Application.isPlaying ? spawnCenter : transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, AreaSpawn); // Dibujamos un cubo para representar el área
    }
}