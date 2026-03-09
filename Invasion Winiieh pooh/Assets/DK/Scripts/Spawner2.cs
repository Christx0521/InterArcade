using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 2f;
    public float platformSpeed = 2f;
    public bool moveUp = true;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    private void Spawn()
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        MovingPlatform platform = obj.AddComponent<MovingPlatform>();
        platform.speed = platformSpeed;
        platform.moveUp = moveUp;
    }
}

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public bool moveUp = true;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector2 direction = moveUp ? Vector2.up : Vector2.down;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}