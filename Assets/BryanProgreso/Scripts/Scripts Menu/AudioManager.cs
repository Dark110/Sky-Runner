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
        //  1. Patrón Singleton y Persistencia
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

    //  2. Inicio de la Música
    void Start()
    {
        // Solo inicia la música si no está sonando ya. 
        if (!musicSource.isPlaying)
        {
            // Asegúrate de que "Musica" coincida exactamente con el nombre de tu AudioClip
            PlayMusic("Musica");
        }
    }

    // --- MÚSICA -
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

    //  VERSIÓN SIMPLIFICADA (ESTABLE)
    public void SetMasterVolume(float value)
    {
        // Usamos la fórmula logarítmica directamente. 
        // Si el slider tiene Min Value = 0.00001, esto silencia el audio sin necesidad de 'if'.
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    //  VERSIÓN SIMPLIFICADA (ESTABLE)
    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    //  VERSIÓN SIMPLIFICADA (ESTABLE)
    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}