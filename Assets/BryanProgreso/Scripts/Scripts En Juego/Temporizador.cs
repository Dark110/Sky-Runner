using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Temporizador : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float MinutoTiempo = 60f;
    public bool modoPrueba = false;
    public float velocidadTest = 5f;

    [Header("UI")]
    public TMP_Text UI_Tiempo;

    private float contador;
    private bool juegoActivo = true;
    private int minutos;
    private int segundos;

    void Start()
    {
        contador = MinutoTiempo;
    }

    void Update()
    {
        if (!juegoActivo) return;

        float delta = Time.deltaTime;
        if (modoPrueba) delta *= velocidadTest;

        contador -= delta;
        contador = Mathf.Max(0, contador);

        minutos = Mathf.FloorToInt(contador / 60f);
        segundos = Mathf.FloorToInt(contador % 60f);

        UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (contador <= 0f)
        {
            juegoActivo = false;
            Victoria();
        }
    }

    void Victoria()
    {
        Debug.Log("¡Tiempo completado! Victoria");
        UI_Tiempo.text = "00:00";

        if (SaveDataManager.Instance != null)
            SaveDataManager.Instance.EndGame();

        SceneManager.LoadScene("Victoria");
    }

    public void DetenerTemporizador()
    {
        juegoActivo = false;
    }
}
