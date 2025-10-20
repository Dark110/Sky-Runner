using UnityEngine;

[RequireComponent(typeof(Transform))]
public class IdlePenalty : MonoBehaviour
{
    [Header("Ajustes de penalización por inactividad")]
    [Tooltip("Segundos sin moverse tras los cuales se aplica la penalización")]
    public float secondsToPenalize = 5f;

    [Tooltip("Puntos a restar cada vez que se penaliza (usa número positivo, el script resta)")]
    public int penaltyAmount = 1;

    [Tooltip("Distancia mínima entre frames para considerar que el jugador se movió")]
    public float movementThreshold = 0.01f;

    [Tooltip("Tiempo mínimo entre penalizaciones consecutivas (puedes dejar 0 para penalizar cada secondsToPenalize automáticamente)")]
    public float penaltyCooldown = 0f;

    [Tooltip("Si true usa el cambio de posición world; si false también revisa entrada de ejes (Horizontal/Vertical) para detectar movimiento)")]
    public bool considerInputAxes = true;

    // Opcional: referencia al ScoreManager (si no pones nada intenta usar el singleton)
    [Header("Referencias (opcional)")]
    public ScoreManager scoreManager;

    // Estado interno
    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private float lastPenaltyTime = -999f;

    private void Start()
    {
        lastPosition = transform.position;
        if (scoreManager == null && ScoreManager.Instance != null)
            scoreManager = ScoreManager.Instance;
    }

    private void Update()
    {
        bool moved = HasMoved();

        if (moved)
        {
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= secondsToPenalize && Time.time - lastPenaltyTime >= penaltyCooldown)
            {
                ApplyPenalty();
                lastPenaltyTime = Time.time;
                idleTimer = 0f; // reiniciamos contador para permitir penalizaciones repetidas
            }
        }

        lastPosition = transform.position;
    }

    private bool HasMoved()
    {
        // Comprueba cambio de posicion en world
        float dist = Vector3.Distance(transform.position, lastPosition);
        if (dist > movementThreshold) return true;

        // Opcional: si consideras input de ejes (para jugadores que mueven con Input.GetAxis)
        if (considerInputAxes)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            // también revisar botones de salto si quieres; umbral muy pequeño
            if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f) return true;
        }

        return false;
    }

    private void ApplyPenalty()
    {
        if (scoreManager == null)
        {
            Debug.LogWarning("IdlePenalty: no hay ScoreManager asignado y tampoco hay singleton ScoreManager.Instance.");
            return;
        }

        // Se resta penaltyAmount (se pasa el negativo al método AddScore)
        scoreManager.AddScore(-Mathf.Abs(penaltyAmount));
        // Mensaje de debug
        Debug.Log($"IdlePenalty: se aplicó una penalización de {penaltyAmount} puntos al jugador porque estuvo {secondsToPenalize} s sin moverse.");
    }
}