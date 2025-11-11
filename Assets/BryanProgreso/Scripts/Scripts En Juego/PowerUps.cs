using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum TipoPowerUp
    {
        Velocidad,
        Invulnerabilidad
    }

    [Header("Configuración del PowerUp")]
    public TipoPowerUp tipo = TipoPowerUp.Velocidad;
    public float duracion = 5f;
    public float multiplicadorVelocidad = 1.5f;

    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;   // Movimiento hacia +Z
    public float limiteDespawnZ = 20f;       // Cuando pasa delante del jugador, se destruye

    private Transform jugador;

    private void Start()
    {
        // Buscar al jugador automáticamente
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            jugador = player.transform;
    }

    private void Update()
    {
        // Movimiento hacia adelante (+Z global)
        transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime, Space.World);

        // Si sobrepasa el límite frente al jugador, se destruye
        if (jugador != null && transform.position.z > jugador.position.z + limiteDespawnZ)
        {
           // Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var jugador = other.GetComponent<MovimientoParacaidista>();
        if (jugador == null) return;

        switch (tipo)
        {
            case TipoPowerUp.Velocidad:
                StartCoroutine(AumentarVelocidadTemporal(jugador));
                break;

            case TipoPowerUp.Invulnerabilidad:
                StartCoroutine(ActivarInvulnerabilidadTemporal(jugador));
                break;
        }

        Destroy(gameObject);
    }

    private IEnumerator AumentarVelocidadTemporal(MovimientoParacaidista jugador)
    {
        var field = jugador.GetType().GetField("Velocidad") ??
                    jugador.GetType().GetField("velocidad");

        if (field == null)
        {
            Debug.LogWarning("[PowerUp] No se encontró campo de velocidad en el jugador.");
            yield break;
        }

        float velocidadOriginal = (float)field.GetValue(jugador);
        field.SetValue(jugador, velocidadOriginal * multiplicadorVelocidad);

        yield return new WaitForSeconds(duracion);

        field.SetValue(jugador, velocidadOriginal);
    }

    private IEnumerator ActivarInvulnerabilidadTemporal(MovimientoParacaidista jugador)
    {
        jugador.invulnerable = true;
        yield return new WaitForSeconds(duracion);
        jugador.invulnerable = false;
    }
}
