using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Asegura que solo exista una instancia del Manager
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
        // ⭐ 1. Patrón Singleton y Persistencia
        if (Instance == null)
        {
            Instance = this;
            // ¡Esto es crucial! Mantiene el objeto vivo entre escenas.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destruye la instancia nueva si ya existe la original (la que tiene la música sonando)
            Destroy(gameObject);
            return;
        }

        // Crear diccionario de SFX para acceso rápido
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }
    }

    // ⭐ 2. Inicio de la Música
    void Start()
    {
        // Solo inicia la música si no está sonando ya. 
        // Esto evita que se reinicie si el objeto Manager ya existe.
        if (!musicSource.isPlaying)
        {
            // Asegúrate de que "Musica" coincida exactamente con el nombre de tu AudioClip
            PlayMusic("Musica");
        }
    }

    // --- MÚSICA ---
    public void PlayMusic(string name, bool loop = true)
    {
        AudioClip clip = System.Array.Find(musicClips, m => m.name == name);
        if (clip != null)
        {
            // Solo reproduce si el clip es diferente O si no está sonando.
            if (musicSource.clip != clip || !musicSource.isPlaying)
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"No se encontró la música: {name}");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
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
    // (Estos métodos asumen que has configurado los parámetros "MasterVolume", "MusicVolume" y "SFXVolume" 
    // en tu Audio Mixer y que el rango del slider de UI va de 0.0001 a 1.0)
    public void SetMasterVolume(float value)
    {
        // Si el valor del slider está muy cerca de cero, lo forzamos a -80 dB.
        if (value < 0.001f)
        {
            mainMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            // La conversión Logarítmica es necesaria para el Audio Mixer
            mainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}