using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour
{
    public GameObject BotonPref;       // Prefab del botón
    public Transform DivBotones;       // Panel donde se generarán los botones
    public NivelesArray controlNiveles; // Script con arreglo de niveles

    void Start()
    {
        CrearMenu();
    }

    void CrearMenu()
    {
        if (BotonPref == null || DivBotones == null || controlNiveles == null)
        {
            Debug.LogError("Faltan referencias en el Inspector en MenuNiveles");
            return;
        }

        for (int i = 0; i < controlNiveles.niveles.Length; i++)
        {
            GameObject boton = Instantiate(BotonPref, DivBotones);

            // Cambiar el texto del botón
            Text texto = boton.GetComponentInChildren<Text>();
            if (texto != null)
                texto.text = "Nivel " + (i + 1);

            // Configurar interacción según si el nivel está desbloqueado
            Button BTN = boton.GetComponent<Button>();
            if (BTN != null)
            {
                int index = i; // importante para que capture el índice correcto
                BTN.interactable = controlNiveles.DesbloqueNivel(index);
                BTN.onClick.AddListener(() => CargarNivel(index));
            }
        }
    }

    void CargarNivel(int index)
    {
        SceneManager.LoadScene(index);
    }
}