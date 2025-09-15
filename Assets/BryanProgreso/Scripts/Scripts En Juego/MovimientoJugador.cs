using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimientoParacaidista : MonoBehaviour
{
    [Header("Velocidades")]
    public float Velocidad = 10f;       // Velocidad de movimiento
    public float NivelSuavidad = 2f;    // (opcional, para suavizar input)

    private CharacterController controller;
    private Vector3 movimiento;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        float inputX = Input.GetAxis("Horizontal"); // Movimiento lateral
        float inputY = Input.GetAxis("Vertical");   // Movimiento arriba/abajo
#else
        float inputX = Input.acceleration.x;
        float inputY = Input.acceleration.y;
#endif

        // Vector de movimiento X y Y
        movimiento = new Vector3(inputX, inputY, 0);

        // Normalizacion para que se mueva bien en diagonal
        if (movimiento.magnitude > 1f)
            movimiento.Normalize();
    }

    private void FixedUpdate()
    {
        // Aplicamos el movimiento con colisiones
        controller.Move(movimiento * Velocidad * Time.fixedDeltaTime);
    }
}
