using UnityEngine;
using System.Collections;
 

[RequireComponent(typeof(CharacterController))]
public class MovimientoParacaidista : MonoBehaviour
{
    [Header("Velocidades")]
    public float Velocidad = 10f;
    private float velocidadBase; // Para recordar la velocidad original

    [Header("Inclinación visual")]
    public float AnguloMaxX = 15f;
    public float AnguloMaxZ = 15f;

    //  Referencia necesaria para la UI 
    [Header("PowerUp UI")]
    [Tooltip("Arrastra el GameObject que tenga el script PowerUpUI")]
    public PowerUpUI powerUpUI;
    //  Fin Referencia UI 

    [Header("Sensibilidad en Android")]
    public float Sensibilidad = 2.2f;
    public bool CalibrarAlInicio = true;
    public Vector3 offsetFijo = Vector3.zero;

    [Header("PowerUps Estado")]
    public bool invulnerable = false;
    public bool boostVelocidadActivo = false;

    [Header("Suavizado de rotación")]
    public float NivelSuavidad = 10f;
    public float DeltaMultiplicador = 1.5f;

    [Header("Efecto Skybox")]
    public float intensidadSkybox = 10f;
    public float rotacionInicialSkybox = 120f;

    private Vector3 offset;
    private CharacterController controller;
    private Vector3 movimiento;
    private Quaternion rotacionInicial;
    private float rotacionSkybox;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rotacionInicial = transform.rotation;
        velocidadBase = Velocidad; // Guardamos la velocidad inicial al arrancar
    }

    private void Start()
    {
        invulnerable = false;
        boostVelocidadActivo = false;
        Velocidad = velocidadBase;

#if !UNITY_STANDALONE && !UNITY_EDITOR
        if (CalibrarAlInicio)
            offset = Input.acceleration;
        else
            offset = offsetFijo;
#endif
        rotacionSkybox = rotacionInicialSkybox;
        RenderSettings.skybox.SetFloat("_Rotation", rotacionSkybox);
    }

    public void ActivarVelocidad(float multiplicador, float duracion)
    {
        if (powerUpUI != null)
            powerUpUI.IniciarTimer(TipoPowerUp.Velocidad, duracion);
        
        StartCoroutine(RutinaVelocidad(multiplicador, duracion));
    }

    public void ActivarInvulnerabilidad(float duracion)
    {
        if (powerUpUI != null)
            powerUpUI.IniciarTimer(TipoPowerUp.Invulnerabilidad, duracion);

        StartCoroutine(RutinaInvulnerabilidad(duracion));
    }

    private IEnumerator RutinaVelocidad(float multiplicador, float duracion)
    {
        if (!boostVelocidadActivo)
        {
            boostVelocidadActivo = true;
            Velocidad = velocidadBase * multiplicador;
            Debug.Log($"<color=cyan>[POWERUP] Velocidad AUMENTADA a: {Velocidad}</color>");
        }
        else
        {
            // Si ya está activo, la UI ya se reinició en ActivarVelocidad
            Debug.Log("<color=cyan>[POWERUP] Tiempo de velocidad extendido o reiniciado</color>");
        }

        yield return new WaitForSeconds(duracion);

        Velocidad = velocidadBase;
        boostVelocidadActivo = false;
        Debug.Log($"<color=cyan>[POWERUP] Velocidad TERMINADA. Vuelta a: {Velocidad}</color>");
    }

    private IEnumerator RutinaInvulnerabilidad(float duracion)
    {
        invulnerable = true;
        Debug.Log("<color=yellow>[POWERUP] ¡Jugador INVULNERABLE activado!</color>");


        yield return new WaitForSeconds(duracion);

        invulnerable = false;
        Debug.Log("<color=yellow>[POWERUP] Invulnerabilidad TERMINADA.</color>");
    }

    private void Update()
    {
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

        movimiento = new Vector3(-inputX, inputY, 0f);
        if (movimiento.magnitude > 1f)
            movimiento.Normalize();

        float tiltX = inputY * AnguloMaxX;
        float tiltZ = inputX * AnguloMaxZ;
        Quaternion rotacionTilt = Quaternion.Euler(tiltX, 0f, tiltZ);
        Quaternion rotacionObjetivo = rotacionInicial * rotacionTilt;

        float factorInput = Mathf.Clamp01(movimiento.magnitude);
        float velocidadSlerp = NivelSuavidad * factorInput * DeltaMultiplicador * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadSlerp);

        float objetivoSkybox = rotacionInicialSkybox - inputX * intensidadSkybox;
        rotacionSkybox = Mathf.Lerp(rotacionSkybox, objetivoSkybox, Time.deltaTime * 2f);
        RenderSettings.skybox.SetFloat("_Rotation", rotacionSkybox);
    }

    private void FixedUpdate()
    {
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