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
    public float NivelSuavidad = 10f;
    public float DeltaMultiplicador = 1.5f;

    [Header("Efecto Skybox")]
    public float intensidadSkybox = 10f;
    public float rotacionInicialSkybox = 120f;

    [Header("Dirección de movimiento")]
    [Tooltip("Activa esto si el jugador se mueve en dirección contraria al input horizontal.")]
    public bool invertirHorizontal = true; // 🔹 NUEVA VARIABLE

    private Vector3 offset;
    private CharacterController controller;
    private Vector3 movimiento;
    private Quaternion rotacionInicial;
    private float rotacionSkybox;

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
        rotacionSkybox = rotacionInicialSkybox;
        RenderSettings.skybox.SetFloat("_Rotation", rotacionSkybox);
    }

    private void Update()
    {
        // Pausa: si el juego está pausado, no procesar inputs/rotaciones
        if (MenuGameManager.Instance != null &&
            MenuGameManager.Instance.CurrentState == GameState.PAUSE)
        {
            return;
        }

        float inputX;
        float inputY;

#if UNITY_STANDALONE || UNITY_EDITOR
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
#else
        float deadZone = 0.05f;
        float rawX = Input.acceleration.x - offset.x;
        float rawY = Input.acceleration.y - offset.y;
        inputX = Mathf.Abs(rawX) < deadZone ? 0f : rawX;
        inputY = Mathf.Abs(rawY) < deadZone ? 0f : rawY;
        inputX = Mathf.Sign(inputX) * Mathf.Pow(Mathf.Abs(inputX), 0.7f) * Sensibilidad;
        inputY = Mathf.Sign(inputY) * Mathf.Pow(Mathf.Abs(inputY), 0.7f) * Sensibilidad;
        inputX = Mathf.Clamp(inputX, -1f, 1f);
        inputY = Mathf.Clamp(inputY, -1f, 1f);
#endif

        // ✅ CAMBIO HECHO AQUÍ:
        // Antes: movimiento = new Vector3(inputX, inputY, 0f);
        // Ahora: permite invertir el eje horizontal según la variable pública.
        float x = invertirHorizontal ? -inputX : inputX;
        movimiento = new Vector3(x, inputY, 0f);

        if (movimiento.magnitude > 1f)
            movimiento.Normalize();

        // Rotación visual del jugador
        float tiltX = -inputY * AnguloMaxX;
        float tiltZ = -x * AnguloMaxZ;
        Quaternion rotacionTilt = Quaternion.Euler(tiltX, 0f, tiltZ);
        Quaternion rotacionObjetivo = rotacionInicial * rotacionTilt;
        float factorInput = Mathf.Clamp01(movimiento.magnitude);
        float velocidadSlerp = NivelSuavidad * factorInput * DeltaMultiplicador * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadSlerp);

        // Skybox: suavizar movimiento
        float objetivoSkybox = rotacionInicialSkybox + x * intensidadSkybox;
        rotacionSkybox = Mathf.Lerp(rotacionSkybox, objetivoSkybox, Time.deltaTime * 2f);
        RenderSettings.skybox.SetFloat("_Rotation", rotacionSkybox);
    }

    private void FixedUpdate()
    {
        // Pausa: si el juego está pausado, no mover al personaje
        if (MenuGameManager.Instance != null &&
            MenuGameManager.Instance.CurrentState == GameState.PAUSE)
        {
            return;
        }

        controller.Move(movimiento * Velocidad * Time.fixedDeltaTime);
    }

    public void Recalibrar()
    {
#if !UNITY_STANDALONE && !UNITY_EDITOR
        offset = Input.acceleration;
#endif
    }
}