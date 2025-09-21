using UnityEngine;
using System.Collections;

public class SpamInvunerabilidad : MonoBehaviour
{
    public GameObject InvunePrefab; // Prefab del powerUP
    public Vector3 espacio_min;     // esquina mínima del área
    public Vector3 espacio_max;     // esquina máxima del área
    public float TimeMinUP = 5f;    // tiempo mínimo entre spawns
    public float TimeMaxUP = 15f;   // tiempo máximo entre spawns

    private void Start()
    {
        StartCoroutine(Invunerabilidad_Spam());
    }

    private IEnumerator Invunerabilidad_Spam()
    {
        while (true)
        {
            float espera = Random.Range(TimeMinUP, TimeMaxUP);
            yield return new WaitForSeconds(espera);

            // Posición aleatoria dentro del área
            float x = Random.Range(espacio_min.x, espacio_max.x);
            float y = Random.Range(espacio_min.y, espacio_max.y);
            float z = 0f; // para un juego 2D en X-Y, o ajusta para 3D
            Vector3 pocisionInvunerabilidad = new Vector3(x, y, z);

            // Debug para verificar que la corrutina se ejecuta
            Debug.Log("Intentando spawnear power-up en: " + pocisionInvunerabilidad);

            // Instanciar solo si el prefab está asignado
            if (InvunePrefab != null)
            {
                Instantiate(InvunePrefab, pocisionInvunerabilidad, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("InvunePrefab no está asignado!");
            }
        }
    }
}