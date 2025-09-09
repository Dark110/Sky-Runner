        using UnityEngine;
        using TMPro; // Importante para TMP_Text

        public class Temporizador : MonoBehaviour
        {
            public TMP_Text UI_Tiempo; // Cambiado a TMP_Text
            public float MinutoTiempo = 60f;
            private float contador;
            private bool juego = true;
            private int segundos;
            private int minutos;

            void Start()
            {
                contador = MinutoTiempo;
            }

            void Update()
            {
                if (juego)
                {
                    // Restar tiempo
                    contador -= Time.deltaTime;
                    contador = Mathf.Max(0, contador);

                    // Calcular minutos y segundos
                    segundos = Mathf.FloorToInt(contador % 60);
                    minutos = Mathf.FloorToInt(contador / 60);

                    // Mostrar en el texto
                    UI_Tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

                    // Verificar si terminó el tiempo
                    if (contador <= 0)
                    {
                        juego = false;
                        JuegoTerminado();
                    }
                }
            }

            // Función cuando se acaba el tiempo
            void JuegoTerminado()
            {
                Debug.Log("Game OVER");
                UI_Tiempo.text = "00:00";
            }
        }