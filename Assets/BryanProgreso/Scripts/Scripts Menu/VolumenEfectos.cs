using UnityEngine;
using UnityEngine.UI;

public class VolumenEfectos : MonoBehaviour
{
    [Header("Sliders de Volumen")]
    public Slider sliderMaster;
    public Slider sliderMusic;
    public Slider sliderSFX;

    void Start()
    {
        float masterVol = PlayerPrefs.GetFloat("MasterVol", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 1f);

        sliderMaster.value = masterVol;
        sliderMusic.value = musicVol;
        sliderSFX.value = sfxVol;

        CambiarVolumenMaster(masterVol);
        CambiarVolumenMusica(musicVol);
        CambiarVolumenSFX(sfxVol);
    }

    public void CambiarVolumenMaster(float valor)
    {
        AudioManager.Instance.SetMasterVolume(valor);
        PlayerPrefs.SetFloat("MasterVol", valor);
    }

    public void CambiarVolumenMusica(float valor)
    {
        AudioManager.Instance.SetMusicVolume(valor);
        PlayerPrefs.SetFloat("MusicVol", valor);
    }

    public void CambiarVolumenSFX(float valor)
    {
        AudioManager.Instance.SetSFXVolume(valor);
        PlayerPrefs.SetFloat("SFXVol", valor);
    }
}