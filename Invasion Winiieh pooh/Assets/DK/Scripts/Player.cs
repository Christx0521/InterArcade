using UnityEngine;

public class Player : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private Vector2 direction;
    private Rigidbody2D rb;

    private new Collider2D collider;
    private Collider2D[] results;

    public float moveSpeed = 3f;
    public float jumpForce = 3.0f;

    public float slideFriction = 5f;

    public bool bONGROUND;
    public bool climbing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckGround()
    {


        bONGROUND = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBox(transform.position, size, 0f, ContactFilter2D.noFilter, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                bONGROUND = hit.transform.position.y < transform.position.y - 0.5f;

                Physics2D.IgnoreCollision(collider, results[i], !bONGROUND);

            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                if (direction.y <= 0f)
                {
                    climbing = true;
                }
            }


        }
    }

    private void Update()
    {
        CheckGround();
        float input = Input.GetAxis("Horizontal");



        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (bONGROUND && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpForce;

        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }


        if (bONGROUND)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (Mathf.Abs(input) > 0.01f)
        {
            direction.x = input * moveSpeed;
        }
        else
        {
            direction.x = Mathf.Lerp(direction.x, 0f, slideFriction * Time.deltaTime);
        }

        if (input > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (input < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if (climbing || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;
            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindAnyObjectByType<GameManager>().LevelComplete();

        }
        else if (collision.gameObject.CompareTag("Barril") || collision.gameObject.CompareTag("Fire"))
        {
            enabled = false;
            FindAnyObjectByType<GameManager>().LevelFailed();
        }


    }
}