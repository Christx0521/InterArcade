using UnityEngine;
using Assets.ARKANOID.Scripts;
public class BallController : MonoBehaviour
{
    [Header("Física de la Bola")]
    public float fuerza = 15f; // Aumentamos un poco para mejor respuesta
    public bool dañoDoble = false;

    private Rigidbody rb;
    private bool enJuego = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

      
        
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        // Nos aseguramos de que no use gravedad para que no se caiga del plano
        rb.useGravity = false;
    }

    void Update()
    {
        // Lanzamiento inicial con la tecla Espacio (o Jump)
        if (!enJuego && Input.GetButtonDown("Jump"))
        {
            LanzarDirecto();
        }
    }

    public void LanzarDirecto()
    {
        if (rb == null) rb = GetComponent<Rigidbody>(); 

        enJuego = true;
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 direccionInicial = new Vector3(Random.Range(-0.194f, 4f), 0, 10f).normalized;
        rb.linearVelocity = direccionInicial * fuerza;
    }

    void FixedUpdate()
    {
        if (enJuego)
        {
            // Mantenemos la velocidad constante para que no se frene al chocar
            rb.linearVelocity = rb.linearVelocity.normalized * fuerza;

           
            if (transform.position.y != 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaMuerta"))
        {
            ProcesarCaida();
        }
    }

    //  modifica  OnCollisionEnter 
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("ZonaMuerta"))
        {
            ProcesarCaida();
        }
    }

    void ProcesarCaida()
    {
        PowerUps scriptVidas = Object.FindFirstObjectByType<PowerUps>();
        if (scriptVidas != null)
        {
            scriptVidas.PerderVida();
        }
        Destroy(gameObject);
    }
}