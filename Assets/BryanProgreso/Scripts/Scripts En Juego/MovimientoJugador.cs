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

    [Header("Sensibilidad en Android")]
    public float Sensibilidad = 2f;     // Multiplicador de sensibilidad
    public bool CalibrarAlInicio = true; // ¿Se calibra automáticamente al inicio?
    public Vector3 offsetFijo = Vector3.zero; // Offset manual para pruebas

    private Vector3 offset;             // Offset actual (calibrado o fijo)
    private CharacterController controller;
    private Vector3 movimiento;
    private Quaternion rotacionInicial; // Guardar rotación base del jugador

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rotacionInicial = transform.rotation; // guardamos la rotación original del prefab
    }

    private void Start()
    {
#if !UNITY_STANDALONE && !UNITY_EDITOR
        if (CalibrarAlInicio)
            offset = Input.acceleration;   // Calibra con la posición actual
        else
            offset = offsetFijo;           // Usa el valor fijo definido en el Inspector
#endif
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        float inputX = Input.GetAxis("Horizontal"); // Movimiento lateral con teclado
        float inputY = Input.GetAxis("Vertical");   // Movimiento vertical con teclado
#else
        // Movimiento con acelerómetro (aplicando offset y sensibilidad)
        float inputX = (Input.acceleration.x - offset.x) * Sensibilidad;
        float inputY = (Input.acceleration.y - offset.y) * Sensibilidad;
#endif

        // Vector de movimiento
        movimiento = new Vector3(inputX, inputY, 0);

        if (movimiento.magnitude > 1f)
            movimiento.Normalize();

        // Calcular inclinación adicional para el efecto visual
        float tiltX = -inputY * AnguloMaxX;
        float tiltZ = -inputX * AnguloMaxZ;
        Quaternion rotacionTilt = Quaternion.Euler(tiltX, 0f, tiltZ);

        // Combinar rotación inicial + tilt (sin perder orientación original)
        Quaternion rotacionObjetivo = rotacionInicial * rotacionTilt;

        // Suavizar hacia la rotación objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, NivelSuavidad * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        controller.Move(movimiento * Velocidad * Time.fixedDeltaTime);
    }

    // Método opcional para recalibrar durante la partida
    public void Recalibrar()
    {
#if !UNITY_STANDALONE && !UNITY_EDITOR
        offset = Input.acceleration;
#endif
    }
}
