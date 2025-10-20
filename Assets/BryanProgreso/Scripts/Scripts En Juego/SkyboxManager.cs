using UnityEngine;

public class SkyboxRotacionInicial : MonoBehaviour
{
    [Header("Rotación inicial de la Skybox")]
    [Range(0f, 360f)]
    public float rotacionInicial = 45f;

    [Header("Rotación dinámica opcional")]
    public bool rotarDuranteJuego = true;
    public float velocidadRotacion = 1f;

    void Start()
    {
        // Rotacion inicial de la skybox
        RenderSettings.skybox.SetFloat("_Rotation", rotacionInicial);
    }

    void Update()
    {
       
    }
}
