using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// 🔹 Enum visible para todos los scripts
public enum TipoPowerUp
{
    Velocidad,
    Invulnerable
}

[System.Serializable]
public class PowerUpData
{
    public TipoPowerUp tipo;
    public float tiempoRestante;
    public float duracion;
    public Image indicadorUI;
    public float valorExtra; // solo para velocidad
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("Jugador")]
    public MovimientoParacaidista player;

    [Header("PowerUps activos")]
    public List<PowerUpData> powerUpsActivos = new List<PowerUpData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        for (int i = powerUpsActivos.Count - 1; i >= 0; i--)
        {
            PowerUpData p = powerUpsActivos[i];
            p.tiempoRestante -= Time.deltaTime;

            if (p.indicadorUI != null)
                p.indicadorUI.fillAmount = p.tiempoRestante / p.duracion;

            if (p.tiempoRestante <= 0)
            {
                DesactivarPowerUp(p);
                powerUpsActivos.RemoveAt(i);
            }
        }
    }

    public void ActivarPowerUp(TipoPowerUp tipo, float duracion, float valorExtra = 0, Image ui = null)
    {
        PowerUpData nuevo = new PowerUpData
        {
            tipo = tipo,
            duracion = duracion,
            tiempoRestante = duracion,
            valorExtra = valorExtra,
            indicadorUI = ui
        };

        // Aplicar efecto al jugador
        switch (tipo)
        {
            case TipoPowerUp.Velocidad:
                if (player != null) player.Velocidad += valorExtra;
                break;
            case TipoPowerUp.Invulnerable:
                if (player != null) player.invulnerable = true;
                break;
        }

        powerUpsActivos.Add(nuevo);
    }

    void DesactivarPowerUp(PowerUpData p)
    {
        switch (p.tipo)
        {
            case TipoPowerUp.Velocidad:
                if (player != null) player.Velocidad -= p.valorExtra;
                break;
            case TipoPowerUp.Invulnerable:
                if (player != null) player.invulnerable = false;
                break;
        }

        if (p.indicadorUI != null)
            p.indicadorUI.fillAmount = 0;
    }
}
