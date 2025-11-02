using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private Dictionary<string, AudioClip> sfxDictionary;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crear diccionario de SFX
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }
    }

    // --- MÚSICA ---
    public void PlayMusic(string name, bool loop = true)
    {
        AudioClip clip = System.Array.Find(musicClips, m => m.name == name);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"No se encontró la música: {name}");
        }
    }

    // --- EFECTOS ---
    public void PlaySFX(string name)
    {
        if (sfxDictionary.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxDictionary[name]);
        }
        else
        {
            Debug.LogWarning($"No se encontró el efecto: {name}");
        }
    }

    // --- CONTROL DE VOLUMEN ---
    public void SetMasterVolume(float value)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
