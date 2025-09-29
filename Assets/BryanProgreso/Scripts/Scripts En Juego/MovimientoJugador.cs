using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimientoParacaidista : MonoBehaviour
{
    [Header("Velocidades")]
    public float Velocidad = 10f;       // Velocidad de movimiento
    public float NivelSuavidad = 5f;    // Suavidad de la inclinación

    [Header("Inclinación visual")]
    public float AnguloMaxX = 15f;      // Máximo tilt adelante/atrás
    public float AnguloMaxZ = 15f;      // Máximo tilt izquierda/derecha

    private CharacterController controller;
    private Vector3 movimiento;
    private Quaternion rotacionInicial; // Guardar rotación base del jugador

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rotacionInicial = transform.rotation; // guardamos la rotación original del prefab
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        float inputX = Input.GetAxis("Horizontal"); // Movimiento lateral
        float inputY = Input.GetAxis("Vertical");   // Movimiento vertical
#else
        float inputX = Input.acceleration.x;
        float inputY = Input.acceleration.y;
#endif

        // Vector de movimiento
        movimiento = new Vector3(inputX, inputY, 0);

        if (movimiento.magnitude > 1f)
            movimiento.Normalize();

        // Calcular inclinación adicional
        float tiltX = -inputY * AnguloMaxX;
        float tiltZ = -inputX * AnguloMaxZ;
        Quaternion rotacionTilt = Quaternion.Euler(tiltX, 0f, tiltZ);

        // Combinar rotación inicial + tilt (sin perder orientación)
        Quaternion rotacionObjetivo = rotacionInicial * rotacionTilt;

        // Suavizar hacia la rotación objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, NivelSuavidad * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        controller.Move(movimiento * Velocidad * Time.fixedDeltaTime);
    }
}
