using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Assets.ARKANOID.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public enum GameState { Playing, Paused, GameOver, WinLevel, WinGame }
        public GameState currentState = GameState.Playing;

        [Header("Paneles UI (Se asignan solos)")]
        public GameObject PanelPause;
        public GameObject PanelGameOver;
        public GameObject PanelWinnerLevel;
        public GameObject PanelWinnerGame;

        [Header("Referencias (Se asignan solas)")]
        public PowerUps player;
        public InputField inputNombreGameOver;
        public InputField inputNombreWinnerGame;

        [Header("Puntuación y Nivel")]
        public int puntajeGlobal = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                ResetearNivel();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
            CheckVictoria();
        }

        public void ChangeState(GameState newState)
        {
            currentState = newState;
            switch (currentState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    if (PanelPause) PanelPause.SetActive(false);
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    if (PanelPause) PanelPause.SetActive(true);
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    if (PanelGameOver) PanelGameOver.SetActive(true);
                    ActualizarTextosPuntaje(PanelGameOver);
                    break;
                case GameState.WinLevel:
                    Time.timeScale = 0f;
                    if (PanelWinnerLevel) PanelWinnerLevel.SetActive(true);
                    ActualizarTextosPuntaje(PanelWinnerLevel);
                    break;
                case GameState.WinGame:
                    Time.timeScale = 0f;
                    if (PanelWinnerGame) PanelWinnerGame.SetActive(true);
                    ActualizarTextosPuntaje(PanelWinnerGame);
                    break;
            }
        }

        public void TogglePause()
        {
            if (currentState == GameState.GameOver || currentState == GameState.WinLevel || currentState == GameState.WinGame) return;

            if (currentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
            }
        }

        void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
        void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

        void AlCargarEscena(Scene escena, LoadSceneMode modo)
        {
            // Forzamos a que vuelva al estado de juego inmediatamente
            ChangeState(GameState.Playing);
            StartCoroutine(ReconectarTodo());
        }

        IEnumerator ReconectarTodo()
        {
            // Esperamos 2 frames para que Unity termine de destruir y reconstruir todo el UI
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // 1. Buscamos TODOS los objetos de tipo Canvas Activos e Inactivos
            Canvas canvasReal = null;
            Canvas[] todosLosCanvas = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (Canvas c in todosLosCanvas)
            {
                // Evitamos usar canvas de sistema y nos quedamos con el "sano" de la escena activa
                if (c.gameObject.scene.name == SceneManager.GetActiveScene().name)
                {
                    canvasReal = c;
                    break;
                }
            }

            if (canvasReal != null)
            {
                // Búsqueda profunda y robusta de Paneles
                PanelPause = BuscarHijoProfundo(canvasReal.transform, "PanelPause");
                PanelGameOver = BuscarHijoProfundo(canvasReal.transform, "PanelGameOver");
                PanelWinnerLevel = BuscarHijoProfundo(canvasReal.transform, "PanelWinnerLevel");
                PanelWinnerGame = BuscarHijoProfundo(canvasReal.transform, "PanelWinnerGame");
            }

            // Búsqueda profunda de Inputs dentro de los Paneles encontrados
            if (PanelGameOver != null)
            {
                GameObject o = BuscarHijoProfundo(PanelGameOver.transform, "InputNombreGameOver");
                if (o != null) inputNombreGameOver = o.GetComponent<InputField>();
            }

            if (PanelWinnerGame != null)
            {
                GameObject w = BuscarHijoProfundo(PanelWinnerGame.transform, "InputNombreWinnerGame");
                if (w != null) inputNombreWinnerGame = w.GetComponent<InputField>();
            }

            // Aplicamos restricciones si se encontraron los inputs
            if (inputNombreGameOver != null) inputNombreGameOver.characterLimit = 3;
            if (inputNombreWinnerGame != null) inputNombreWinnerGame.characterLimit = 3;

            // Reconectamos al jugador (PowerUps)
            player = FindFirstObjectByType<PowerUps>();
            if (player != null)
            {
                SpawnNuevaBola();

                // Forzamos actualización visual de vida/puntos del nuevo nivel
                player.ActualizarInterfaz();
            }
        }

        // Nueva función infalible para encontrar GameObjects hijos (activos o inactivos) por nombre
        GameObject BuscarHijoProfundo(Transform padre, string nombreBuscado)
        {
            if (padre.name == nombreBuscado) return padre.gameObject;

            foreach (Transform hijo in padre)
            {
                GameObject resultado = BuscarHijoProfundo(hijo, nombreBuscado);
                if (resultado != null) return resultado;
            }
            return null;
        }

        public void CheckVictoria()
        {
            if (currentState != GameState.Playing) return;

            GameObject[] bloques = GameObject.FindGameObjectsWithTag("Block");
            if (bloques.Length <= 0 && SceneManager.GetActiveScene().name != "MenuPrincipal")
            {
                MostrarFin(true);
            }
        }

        public void MostrarFin(bool victoria)
        {
            if (victoria)
            {
                if (ObtenerNivel() < 3)
                {
                    ChangeState(GameState.WinLevel);
                }
                else
                {
                    ChangeState(GameState.WinGame);
                }
            }
            else
            {
                ChangeState(GameState.GameOver);
            }
        }

        private void ActualizarTextosPuntaje(GameObject panelActivo)
        {
            if (player != null && panelActivo != null)
            {
                Text[] textos = panelActivo.GetComponentsInChildren<Text>(true);
                foreach (var t in textos)
                {
                    if (t.name.ToLower().Contains("score") || t.name.ToLower().Contains("punto") || t.text.ToUpper().Contains("POINTS"))
                    {
                        t.text = "SCORE:" + player.puntos;
                    }
                }
            }
        }

        public void SpawnNuevaBola()
        {
            if (player == null) return;
            if (FindObjectsByType<BallController>(FindObjectsSortMode.None).Length > 0) return;

            Vector3 posicion = player.transform.position + new Vector3(0, 0, 1.2f);
            GameObject b = Instantiate(player.prefabBall, posicion, Quaternion.identity);
            b.transform.SetParent(player.transform);

            b.transform.localScale = new Vector3(1f / player.transform.localScale.x,
                                                 1f / player.transform.localScale.y,
                                                 1f / player.transform.localScale.z);

            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }

        public void BotonSiguiente()
        {
            if (player != null) puntajeGlobal = player.puntos;

            // ¡GRAN CAMBIO!: Aseguramos que se guarde un nuevo valor sumando 1 al nivel actual
            int nivelNuevo = ObtenerNivel() + 1;
            GuardarNivel(nivelNuevo);

            // Ya no necesitamos poner estado aquí, porque AlCargarEscena() forzará el estado Playing
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReiniciarNivel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void BotonSalirYGuardar()
        {
            string nombreFinal = "";

            if (currentState == GameState.GameOver && inputNombreGameOver != null)
            {
                nombreFinal = inputNombreGameOver.text;
            }
            else if (currentState == GameState.WinGame && inputNombreWinnerGame != null)
            {
                nombreFinal = inputNombreWinnerGame.text;
            }

            // Si escribieron algo y no está vacío, guardamos el puntaje
            if (!string.IsNullOrEmpty(nombreFinal))
            {
                GuardarPuntaje(nombreFinal.ToUpper(), player != null ? player.puntos : puntajeGlobal);
            }

            // ¡CORRECCIÓN!: Siempre reiniciamos el nivel y cerramos el juego, hayan guardado texto o no
            ResetearNivel();

            // Esto cierra el juego en una compilación real
            Application.Quit();

            // Esto simula que se cierra si estás jugando dentro del editor de Unity
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        // --- LÓGICA DE PERSISTENCIA ---
        public void GuardarPuntaje(string nombre, int puntos)
        {
            PlayerPrefs.SetInt("Score_" + nombre, puntos);
            PlayerPrefs.Save();
        }

        public int ObtenerNivel()
        {
            return PlayerPrefs.GetInt("NivelActual", 1);
        }

        public void GuardarNivel(int n)
        {
            PlayerPrefs.SetInt("NivelActual", n);
            PlayerPrefs.Save();
            Debug.Log("GameManager guardó el nivel: " + n);
        }

        public void ResetearNivel()
        {
            PlayerPrefs.SetInt("NivelActual", 1);
            PlayerPrefs.Save();
            puntajeGlobal = 0;
        }
    }
}