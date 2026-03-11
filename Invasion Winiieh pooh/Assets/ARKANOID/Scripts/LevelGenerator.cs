using UnityEngine;
using System.Collections;
using System;



namespace Assets.ARKANOID.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameObject bloquePrefab;
        public int columnas = 6;
        public int filasBase = 3;

        [Header("Ajustes de Espaciado")]
        public float distanciaX = 2.2f; // Espacio horizontal entre bloques
        public float distanciaZ = 1.2f; // Espacio vertical (profundidad) entre bloques


        void Start()
        {
            GenerarNivel();

        }

        void GenerarNivel()
        {
            int nivelActual = 0;

            // CAMBIO: Ahora obtenemos el nivel desde el GameManager centralizado
            if (GameManager.Instance != null)
                nivelActual = GameManager.Instance.ObtenerNivel();

            int filasTotales = filasBase + nivelActual;

            float anchoTotal = (columnas - 1) * distanciaX;

            for (int f = 0; f < filasTotales; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    // Posición Centrada respecto al generador
                    float posX = (c * distanciaX) - (anchoTotal / 2f);

                    // El generador es el punto más lejano (fondo)
                    float posZ = -(f * distanciaZ);

                    Vector3 posicionBloque = transform.position + new Vector3(posX, 0, posZ);
                    GameObject nuevoBloque = Instantiate(bloquePrefab, posicionBloque, Quaternion.identity);

                    nuevoBloque.transform.SetParent(this.transform);
                }
            }
        }
    }
}
