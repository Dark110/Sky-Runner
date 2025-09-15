using UnityEngine;
using UnityEngine.SceneManagement;

public class Siguiente_Nivel : MonoBehaviour
{
    // M�todo para cargar el siguiente nivel
    public void NuevoNivel()
    {
        // El nivel actual segun su arreglo
        int NivelAct = SceneManager.GetActiveScene().buildIndex;

        // Nivel desbloqueado
        SceneManager.LoadScene(NivelAct + 1);
    }
}