using UnityEngine;

public class AsteroidManagement : MonoBehaviour
{
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private int damage = 10;

    private float currentTime;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.left * 20f, ForceMode.Impulse);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (currentTime <= 0f)
        {
            other.gameObject.GetComponent<Health>()?.DealDamage(damage);
            currentTime = damageCooldown;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        currentTime = 0f;
    }
}