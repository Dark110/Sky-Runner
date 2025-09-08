using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Biomas : MonoBehaviour
{
    public Text textoBioma; 
    public string[] nombresBiomas = { "OCEANO", "BOSQUE", "DESIERTO" };

    private Coroutine mostrarCoroutine;

    // Mostrar nombre bioma 5 seg
    public void MostrarBioma(int indice)
    {
        if (indice < 0 || indice >= nombresBiomas.Length)
            return;

        if (mostrarCoroutine != null)
            StopCoroutine(mostrarCoroutine);

        mostrarCoroutine = StartCoroutine(MostrarTextoTemporal(nombresBiomas[indice]));
    }

    private IEnumerator MostrarTextoTemporal(string nombre)
    {
        textoBioma.text = nombre;
        textoBioma.enabled = true;
        yield return new WaitForSeconds(5f);
        textoBioma.enabled = false;
    }
}
