using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.ARKANOID.Scripts;


public class PowerUps : MonoBehaviour
{
    [Header("Configuración del Jugador")]
    public GameObject prefabBall;
    public int vida = 3;
    public float velocidad = 15f;
    public float duracionEfecto = 5f;

    [Header("Interfaz de Usuario (UI)")]
    public int puntos = 0;
    public Text displayPuntos;
    public Text displayVidas;
    public Text displayNivel;

    private Vector3 escalaNormal = new Vector3(3f, 0.5f, 0.5f);
    private Vector3 escalaGrande = new Vector3(5f, 0.5f, 0.5f);
    private Vector3 escalaPequeña = new Vector3(1.5f, 0.5f, 0.5f);

    private float velocidadNormal;

    void Start()
    {
        velocidadNormal = velocidad;

        // CAMBIO: Sincronizar con el nuevo GameManager
        if (GameManager.Instance != null)
        {
            puntos = GameManager.Instance.score;
        }
        ActualizarInterfaz();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * h * velocidad * Time.deltaTime);

        // Límites de movimiento
        float xClamped = Mathf.Clamp(transform.position.x, -8.32f, 7.9f);
        transform.position = new Vector3(xClamped, transform.position.y, transform.position.z);
    }

    public void SumarPuntos(int cant)
    {
        puntos += cant;
        ActualizarInterfaz();
    }

    public void ActualizarInterfaz()
    {
        if (displayPuntos) displayPuntos.text = " " + puntos;
        if (displayVidas) displayVidas.text = "" + vida;

        // CAMBIO: Obtener nivel desde GameManager
        if (displayNivel && GameManager.Instance != null)
            displayNivel.text = "NIVEL : " + GameManager.Instance.level;
    }

    // --- AQUÍ ESTÁ EL CAMBIO PRINCIPAL ---
    public void PerderVida()
    {
        BallController[] todasLasPelotas = Object.FindObjectsByType<BallController>(FindObjectsSortMode.None);
        if (todasLasPelotas.Length > 1) return;

        vida--;
        ActualizarInterfaz();

        if (vida <= 0)
        {
            // Usamos la función correcta de tu GameManager para el Game Over
            if (GameManager.Instance != null)
                Object.FindFirstObjectByType<Assets.ARKANOID.Scripts.GameManager>().MostrarFin(false);
        }
        else
        {
            // El propio jugador llama a reponer su bola después de 1 segundo
            Invoke("RespawnSeguro", 1f);
        }
    }

    // --- NUEVA FUNCIÓN AÑADIDA ---
    void RespawnSeguro()
    {
        if (prefabBall == null)
        {
            Debug.LogError("¡Aviso! Faltó asignar el PrefabBall en el script PowerUps.");
            return;
        }

        Vector3 posicion = transform.position + new Vector3(0, 0, 1.2f);
        GameObject nuevaBola = Instantiate(prefabBall, posicion, Quaternion.identity);
        nuevaBola.transform.SetParent(this.transform);

        nuevaBola.transform.localScale = new Vector3(
            1f / transform.localScale.x,
            1f / transform.localScale.y,
            1f / transform.localScale.z
        );
    }
    // -------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            string nombre = other.gameObject.name;
            if (nombre.Contains("Grande")) ActivarPowerUp("Grande");
            else if (nombre.Contains("Pequeño")) ActivarPowerUp("Pequeño");
            else if (nombre.Contains("Vida")) ActivarPowerUp("Vida");
            else if (nombre.Contains("Multi")) ActivarPowerUp("Multi");
            else if (nombre.Contains("Super")) ActivarPowerUp("Super");
            else if (nombre.Contains("Lento")) ActivarPowerUp("Lento"); // RE-AÑADIDO

            Destroy(other.gameObject);
        }
    }

    public void ActivarPowerUp(string tipo)
    {
        switch (tipo)
        {
            case "Grande": StartCoroutine(TempEscala(escalaGrande)); break;
            case "Pequeño": StartCoroutine(TempEscala(escalaPequeña)); break;
            case "Vida": vida++; ActualizarInterfaz(); break;
            case "Multi": CrearBallsExtra(); break;
            case "Super": StartCoroutine(TempSuperBall()); break;
            case "Lento": StartCoroutine(TempVelocidad(velocidadNormal * 0.5f)); break; // RE-AÑADIDO
        }
    }

    IEnumerator TempEscala(Vector3 nueva)
    {
        transform.localScale = nueva;
        yield return new WaitForSeconds(duracionEfecto);
        transform.localScale = escalaNormal;
    }

    IEnumerator TempVelocidad(float nueva)
    {
        velocidad = nueva;
        yield return new WaitForSeconds(duracionEfecto);
        velocidad = velocidadNormal;
    }

    IEnumerator TempSuperBall()
    {
        BallController[] bolas = Object.FindObjectsByType<BallController>(FindObjectsSortMode.None);
        foreach (BallController b in bolas)
        {
            b.dañoDoble = true;
            if (b.GetComponent<MeshRenderer>()) b.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        yield return new WaitForSeconds(duracionEfecto);
        bolas = Object.FindObjectsByType<BallController>(FindObjectsSortMode.None);
        foreach (BallController b in bolas)
        {
            if (b != null)
            {
                b.dañoDoble = false;
                if (b.GetComponent<MeshRenderer>()) b.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
    }

    void CrearBallsExtra()
    {
        BallController original = Object.FindFirstObjectByType<BallController>();
        if (original != null)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject extra = Instantiate(prefabBall, original.transform.position, Quaternion.identity);
                if (extra.GetComponent<BallController>())
                    extra.GetComponent<BallController>().LanzarDirecto();
            }
        }
    }
}
