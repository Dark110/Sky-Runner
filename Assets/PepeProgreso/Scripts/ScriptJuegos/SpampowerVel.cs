using UnityEngine;
using System.Collections;

public class SpampowerVel : MonoBehaviour
{
    public GameObject VelPowerPrefab; // Prefab del power up
    public Vector3 EspacioMin;
    public Vector3 EspacioMax;
    public float SegundosMin = 10f; // un minimo de reaperecion del power up
    public float SegundosMax = 15f;
    float x, y, z;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PowerUPVel());
    }

    private IEnumerator PowerUPVel()
    {
        while (true) // el ciclo que generara el power up de manera infinita
        {
            float TiempoEspera = Random.Range(SegundosMin, SegundosMax); // Tiempo de espera de aleatoridad del power up
            yield return new WaitForSeconds(TiempoEspera);

            // Posiciones aleatorias en que aparecera el power up
            x = Random.Range(EspacioMin.x, EspacioMax.x);
            y = Random.Range(EspacioMin.y, EspacioMax.y);
            z = 0f; // Esto asegura que solo aparezca en 0 en eje de Z
            Vector3 pocisionPowerUPVel = new Vector3(x, y, z);

            // Para poder Instanciar el power UP
            Instantiate(VelPowerPrefab, pocisionPowerUPVel, Quaternion.identity);
        }
    }
}