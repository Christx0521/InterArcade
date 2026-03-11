using UnityEngine;
using Assets.ARKANOID.Scripts;
public class ItemPowerUp : MonoBehaviour
{
    [Tooltip("Nombre exacto del poder:'Super' 'Grande', 'Pequeño', 'Vida', 'Multi', 'Lento'")]
    public string tipoPowerUp;
    public float velocidadCaida = 5f;

    void Update()
    {
        
        transform.Translate(new Vector3(0, 0, -velocidadCaida * Time.deltaTime));

        // Si se pasa del paddle, se destruye para no gastar memoria
        if (transform.position.z < -15f)
        {
            Destroy(gameObject);
        }
    }

    // Detecta colisión con el Paddle (que debe tener isTrigger o el item debe tenerlo)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            // Destroy(gameObject);
        }
    }
}