using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumenEfectos : MonoBehaviour
{
    [Header("Mixer y Slider")]
    public AudioMixer AudioMix;
    public Slider Volumen;
    public Slider Efectos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cargar los volumens de Guardadado
        float MusicVol = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
        float EfectoVol = PlayerPrefs.GetFloat("SFXVolumen", 0.75f);

        Volumen.value = MusicVol;
        Efectos.value = EfectoVol;

        SetMusicVolumen(MusicVol);
        SetSFXVolumen(EfectoVol);

        //Se escuchan los cambios de volumen
        Volumen.onValueChanged.AddListener(SetMusicVolumen);
        Efectos.onValueChanged.AddListener(SetSFXVolumen);
    }

    public void SetMusicVolumen(float value)
    {
        AudioMix.SetFloat("MusicVolumen", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolumen", value);
    }

    public void SetSFXVolumen(float value)
    {
        AudioMix.SetFloat("SFXVolumen", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolumen", value);
    }
}