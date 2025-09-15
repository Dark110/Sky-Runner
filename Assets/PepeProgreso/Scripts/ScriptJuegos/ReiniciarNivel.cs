using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarNivel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Condicion para que no se reinicie el nivel infinitamente presionando R
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Re inicio de nivel
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}