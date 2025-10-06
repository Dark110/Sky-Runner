using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefabNube;

    [Header("Spawn")]
    public float tiempoSpawn = 2f;
    public int maxNubes = 10; // número máximo de nubes activas

    [Header("Rangos")]
    public Vector2 rangoY = new Vector2(-3f, 3f);
    public Vector2 rangoVelocidad = new Vector2(1f, 3f);
    public Vector2 rangoEscala = new Vector2(0.5f, 1.5f);
    public Vector2 rangoZ = new Vector2(3f, 8f); // profundidad entre cámara y fondo

    private float temporizador;

    private void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
        {
            // Solo spawnear si no superamos el límite
            if (GameObject.FindGameObjectsWithTag("Nube").Length < maxNubes)
            {
                SpawnNube();
            }

            temporizador = tiempoSpawn;
        }
    }

    private void SpawnNube()
    {
        // Posición fuera de la pantalla (izquierda del viewport)
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0, Random.Range(0f, 1f), Camera.main.nearClipPlane + 10));

        // Ajustar Y y Z
        float y = Random.Range(rangoY.x, rangoY.y);
        float z = Random.Range(rangoZ.x, rangoZ.y);
        spawnPos = new Vector3(spawnPos.x, y, z);

        // Instanciar nube
        GameObject nube = Instantiate(prefabNube, spawnPos, Quaternion.identity);

        // Tag para contar máximo
        nube.tag = "Nube";

        // Escala aleatoria
        float escala = Random.Range(rangoEscala.x, rangoEscala.y);
        nube.transform.localScale = Vector3.one * escala;

        // Velocidad aleatoria
        Cloud nubeScript = nube.GetComponent<Cloud>();
        nubeScript.velocidad = Random.Range(rangoVelocidad.x, rangoVelocidad.y);
    }
}
