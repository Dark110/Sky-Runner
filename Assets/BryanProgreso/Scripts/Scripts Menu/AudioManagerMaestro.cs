using UnityEngine;
using UnityEngine.Audio;

// Este script solo sirve para probar la funcionalidad del Slider con el AudioMixer.
public class MasterVolumeTester : MonoBehaviour
{
    // Asegúrate de arrastrar tu AudioMixer aquí en el Inspector
    [Header("Audio Mixer")]
    public AudioMixer testMixer;

    // --- FUNCIÓN DE PRUEBA ---
    // Esta función será llamada por el evento On Value Changed del Slider.
    public void SetMasterVolume(float value)
    {
        if (testMixer == null)
        {
            Debug.LogError("ERROR: El campo 'Test Mixer' está vacío. ¡Arrastra tu AudioMixer aquí!");
            return;
        }

        // La fórmula logarítmica es la única que necesitamos.
        // Si el Slider tiene MinValue = 0.00001, esto silencia el audio perfectamente.
        float volumeInDB = Mathf.Log10(value) * 20;

        // Establece el parámetro 'MasterVolume'.
        //  RECUERDA: El parámetro debe llamarse EXACAMENTE "MasterVolume" en el Mixer.
        testMixer.SetFloat("MasterVolume", volumeInDB);

        // Opcional: para ver en la consola lo que se está enviando
        Debug.Log($"Slider Value: {value:F5} -> Volume (dB): {volumeInDB:F2}");
    }
}