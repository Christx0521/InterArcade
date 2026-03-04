using UnityEngine;

public class BK_Projectile : MonoBehaviour
{
    public Vector2 moveDir;
    public float speed;
    public System.Action destroyed;
    public GameObject explosion;

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        destroyed?.Invoke();
        if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
