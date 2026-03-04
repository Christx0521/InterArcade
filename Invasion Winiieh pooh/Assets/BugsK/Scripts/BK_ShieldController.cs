using UnityEngine;

public class BK_ShieldController : MonoBehaviour
{
    SpriteRenderer sprRenderer;
    CapsuleCollider2D col2D;

    [SerializeField] float shieldPower = 1f;

    void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        sprRenderer.color = new Color(1f, 1f, 1f, shieldPower);

        if (shieldPower <= 0f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") || other.CompareTag("Enemy Laser"))
        {
            shieldPower -= 0.15f;
            Destroy(other.gameObject);
        }
    }

    public void ResetShield()
    {
        shieldPower = 1f;
        gameObject.SetActive(true);
    }
}
