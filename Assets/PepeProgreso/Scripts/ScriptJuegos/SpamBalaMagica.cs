using UnityEngine;
using System.Collections;

public class SpamBalaMagica : MonoBehaviour
{
    public GameObject BalaPrefab;
    public Vector3 AreaMin;
    public Vector3 AreaMax;
    public float SegMin;
    public float SegMax;
    float x, y, z;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BalaPower());
    }

    private IEnumerator BalaPower()
    {
        while (true) // el ciclo que generara el power up de manera infinita
        {
            float PowerTime = Random.Range(SegMin, SegMax); // Tiempo de espera de aleatoridad del power up
            yield return new WaitForSeconds(PowerTime);

            // Posiciones aleatorias en que aparecera el power up
            x = Random.Range(AreaMin.x, AreaMax.x);
            y = Random.Range(AreaMin.y, AreaMax.y);
            z = 0f; // Esto asegura que solo aparezca en 0 en eje de Z
            Vector3 PosPowerUP = new Vector3(x, y, z);

            // Para poder Instanciar el power UP
            Instantiate(BalaPrefab, PosPowerUP, Quaternion.identity);
        }
    }
}