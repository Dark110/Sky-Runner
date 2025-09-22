using UnityEngine;
using System.Collections;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float Vel = 15f;
    private float VelActual;

    private Vector3 JugadorMov;

    // Invulnerabilidad
    public bool Invunerabilidad = false;
    public float TiempoInvulnerable = 3f;
    private Renderer rend;

    private void Start()
    {
        VelActual = Vel;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        float IzqDer = Input.GetAxis("Horizontal");
        float ArribaAbajo = Input.GetAxis("Vertical");
#else
        float IzqDer = Input.acceleration.x;
        float ArribaAbajo = Input.acceleration.y;
#endif
        JugadorMov = new Vector3(IzqDer, ArribaAbajo, 0);
        transform.Translate(JugadorMov * VelActual * Time.deltaTime);
    }

    // PowerUp de velocidad
    public void ActPowerUP(float aumento_vel, float duracion_Powerup)
    {
        StartCoroutine(PowerVelocidad(aumento_vel, duracion_Powerup));
    }

    private IEnumerator PowerVelocidad(float MulVel, float TiempoPower)
    {
        VelActual = Vel * MulVel;
        yield return new WaitForSeconds(TiempoPower);
        VelActual = Vel;
    }

    // PowerUp de invulnerabilidad
    public void ActivarPowerUP_Inv()
    {
        if (!Invunerabilidad)
            StartCoroutine(InvulnerableTemporal());
    }

    private IEnumerator InvulnerableTemporal()
    {
        Invunerabilidad = true;
        Debug.Log("Jugador ahora es invulnerable");
        rend.material.color = Color.yellow;

        yield return new WaitForSeconds(TiempoInvulnerable);

        Invunerabilidad = false;
        Debug.Log("Jugador ya no es invulnerable");
        rend.material.color = Color.white;
    }

    // Detección de power-ups
    private void OnTriggerEnter(Collider other)
    {
        // Bala mágica
        if (other.CompareTag("PowerUpBala"))
        {
            Debug.Log("Jugador recogió Bala Mágica");
            // Se dispara automáticamente desde el Power-Up
            Destroy(other.gameObject);
        }

        // Power-Up de velocidad
        if (other.CompareTag("PowerUpVelocidad"))
        {
            ActPowerUP(2f, 5f);
            Destroy(other.gameObject);
        }

        // Power-Up de invulnerabilidad
        if (other.CompareTag("piloto"))
        {
            ActivarPowerUP_Inv();
            Destroy(other.gameObject);
        }
    }
}
