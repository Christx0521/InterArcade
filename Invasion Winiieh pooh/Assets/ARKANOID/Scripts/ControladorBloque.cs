using UnityEngine;
using Assets.ARKANOID.Scripts;

public class ControladorBloque : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int resistencia; 
    public Material[] materialesVida;

    [Header("Configuración de Premios")]
    public GameObject[] listaPremios;
    [Range(0, 100)] public float probabilidadDrop = 25f;

    private MeshRenderer miRender;

    void Start()
    {
        miRender = GetComponent<MeshRenderer>();

        
        if (materialesVida != null && materialesVida.Length > 0)
            resistencia = Random.Range(1, materialesVida.Length + 1);
        else
            resistencia = 1;

        ActualizarVisual();
    }

    void OnCollisionEnter(Collision choque)
    {
        if (choque.gameObject.CompareTag("Ball"))
        {
            BallController ball = choque.gameObject.GetComponent<BallController>();
            int daño = (ball != null && ball.dañoDoble) ? 2 : 1;
            resistencia -= daño;

            if (resistencia <= 0) Muerte();
            else ActualizarVisual();
        }
    }

    void Muerte()
    {
        PowerUps p = Object.FindFirstObjectByType<PowerUps>();
        if (p != null) p.SumarPuntos(100);

        if (listaPremios != null && Random.Range(0f, 100f) <= probabilidadDrop)
        {
            int i = Random.Range(0, listaPremios.Length);
            if (listaPremios[i] != null)
                Instantiate(listaPremios[i], transform.position, Quaternion.identity);
        }

        gameObject.tag = "Untagged";
        //Rebote inicial para evitar que la bola quede atrapada en el bloque destruido
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (miRender != null)miRender.enabled = false;

        // retraso de destrucción para permitir que la bola rebote antes de eliminar el bloque
        Destroy(gameObject,0.05f);
        if (GameManager.Instance != null) GameManager.Instance.Invoke("CheckVictoria", 0.1f);
    }

    void ActualizarVisual()
    {
        if (miRender != null && materialesVida != null && materialesVida.Length > 0)
        {
            int indice = Mathf.Clamp(resistencia - 1, 0, materialesVida.Length - 1);
            miRender.material = materialesVida[indice];
        }
    }
}