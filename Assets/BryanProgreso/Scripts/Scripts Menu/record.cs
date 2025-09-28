using UnityEngine;
using UnityEngine.UI;

public class record : MonoBehaviour
{
    public Text PuntajeActual;
    public Text Record;

    private int PunActual;
    private int PuntajeRecord;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //guardado y la carga de partida (en caso que no existe sera 0)
        PuntajeRecord = PlayerPrefs.GetInt("Record", 0);
        Record.text = "record: " + PuntajeRecord;
        PuntajeActual.text = "puntaje: " + PunActual;
    }

    public void PuntosRecor(int puntos)
    {
        PunActual += puntos;
        PuntajeActual.text = "puntaje: " + PunActual;

        // El if para guardar el puntaje en caso que se supere el record actual
        if (PunActual > PuntajeRecord)
        {
            PuntajeRecord = PunActual;
            Record.text = "record: " + PuntajeRecord;

            PlayerPrefs.SetInt("Record", PuntajeRecord); // Este puntaje se va guardar en la memoria
            PlayerPrefs.Save();
        }
    }
}