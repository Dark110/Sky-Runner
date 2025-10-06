using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimientoParacaidista : MonoBehaviour
{
    [Header("Velocidades")]
    public float Velocidad = 10f;

    [Header("Inclinación visual")]
    public float AnguloMaxX = 15f;
    public float AnguloMaxZ = 15f;

    [Header("Sensibilidad en Android")]
    public float Sensibilidad = 2f;
    public bool CalibrarAlInicio = true;
    public Vector3 offsetFijo = Vector3.zero;

    [Header("PowerUps")]
    public bool invulnerable = false;

    [Header("Suavizado de rotación")]
    public float NivelSuavidad = 10f; // Ajusta en Inspector
    public float DeltaMultiplicador = 1.5f; // Para Android, frames irregulares

    private Vector3 offset;
    private CharacterController controller;
    private Vector3 movimiento;
    private Quaternion rotacionInicial;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rotacionInicial = transform.rotation;
    }

    private void Start()
    {
#if !UNITY_STANDALONE && !UNITY_EDITOR
        if (CalibrarAlInicio)
            offset = Input.acceleration;
        else
            offset = offsetFijo;
#endif
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        // Entrada normal de teclado
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
#else
    // Entrada por acelerómetro para Android
    float deadZone = 0.05f; // evita movimientos por vibraciones leves
    float rawX = Input.acceleration.x - offset.x;
    float rawY = Input.acceleration.y - offset.y;

    // Aplica zona muerta
    float inputX = Mathf.Abs(rawX) < deadZone ? 0f : rawX;
    float inputY = Mathf.Abs(rawY) < deadZone ? 0f : rawY;

    // Escala no lineal para mayor sensibilidad en pequeños movimientos
    inputX = Mathf.Sign(inputX) * Mathf.Pow(Mathf.Abs(inputX), 0.7f) * Sensibilidad;
    inputY = Mathf.Sign(inputY) * Mathf.Pow(Mathf.Abs(inputY), 0.7f) * Sensibilidad;

    // Limita valores para evitar movimientos excesivos
    inputX = Mathf.Clamp(inputX, -1f, 1f);
    inputY = Mathf.Clamp(inputY, -1f, 1f);
#endif

        movimiento = new Vector3(inputX, inputY, 0);
        if (movimiento.magnitude > 1f)
            movimiento.Normalize();

        // Rotación objetivo para inclinación visual
        float tiltX = -inputY * AnguloMaxX;
        float tiltZ = -inputX * AnguloMaxZ;
        Quaternion rotacionTilt = Quaternion.Euler(tiltX, 0f, tiltZ);
        Quaternion rotacionObjetivo = rotacionInicial * rotacionTilt;

        // Suavizado de rotación
        float factorInput = Mathf.Clamp01(movimiento.magnitude);
        float velocidadSlerp = NivelSuavidad * factorInput * DeltaMultiplicador * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadSlerp);
    }

    private void FixedUpdate()
    {
        controller.Move(movimiento * Velocidad * Time.fixedDeltaTime);
    }

    public void Recalibrar()
    {
#if !UNITY_STANDALONE && !UNITY_EDITOR
        offset = Input.acceleration;
#endif
    }
}
