using UnityEngine;

public class NivelesArray : MonoBehaviour
{
    [Header("Arreglo de niveles")]
    // Arreglo de 2 niveles inicializados: el primero desbloqueado, los demás bloqueados
    public bool[] niveles = new bool[2] { true, false};

    void Start()
    {
        // Si es la primera vez que juega, inicializar todos los niveles
        if (!PlayerPrefs.HasKey("nivel0"))
        {
            GuardarProceso(); // Guarda el estado inicial del array
        }
        else
        {
            CargarProgreso(); // Cargar progreso guardado
        }
    }

    // Desbloquear un nivel
    public void NivelDesbloqueado(int index)
    {
        if (index >= 0 && index < niveles.Length)
        {
            niveles[index] = true;
            GuardarProceso();
        }
    }

    // Guardar progreso en PlayerPrefs
    void GuardarProceso()
    {
        for (int i = 0; i < niveles.Length; i++)
        {
            PlayerPrefs.SetInt("nivel" + i, niveles[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    // Cargar progreso desde PlayerPrefs
    void CargarProgreso()
    {
        for (int i = 0; i < niveles.Length; i++)
        {
            niveles[i] = PlayerPrefs.GetInt("nivel" + i, 0) == 1;
        }
    }

    // Verificar si un nivel está desbloqueado
    public bool DesbloqueNivel(int index)
    {
        if (index >= 0 && index < niveles.Length)
            return niveles[index];
        return false;
    }
}