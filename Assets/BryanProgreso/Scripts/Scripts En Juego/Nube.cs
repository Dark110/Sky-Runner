using UnityEngine;

public class Cloud : MonoBehaviour
{
    [HideInInspector] public float velocidad = 2f;

    void Update()
    {
        transform.Translate(Vector3.right * velocidad * Time.deltaTime, Space.World);
    }
}
