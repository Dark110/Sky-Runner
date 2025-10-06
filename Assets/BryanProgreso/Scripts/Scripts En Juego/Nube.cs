using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float velocidad = 2f;

    private void Update()
    {
        // Mover de izquierda a derecha
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);

        // Si sale de la vista de la cámara, destruir
        if (!IsVisibleFrom(Camera.main))
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisibleFrom(Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds);
    }
}
