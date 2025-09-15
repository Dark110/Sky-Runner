using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour
{
    public GameObject BotonPref;       // Prefab del bot�n
    public Transform DivBotones;       // Panel donde se generar�n los botones
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

            // Cambiar el texto del bot�n
            Text texto = boton.GetComponentInChildren<Text>();
            if (texto != null)
                texto.text = "Nivel " + (i + 1);

            // Configurar interacci�n seg�n si el nivel est� desbloqueado
            Button BTN = boton.GetComponent<Button>();
            if (BTN != null)
            {
                int index = i; // importante para que capture el �ndice correcto
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