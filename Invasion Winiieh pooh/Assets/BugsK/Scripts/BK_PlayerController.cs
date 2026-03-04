using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BK_PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] float minPos, maxPos;
    public BK_Projectile laser;
    public bool reloading, invincible;
    public int lives = 3;
    public GameObject explosion;
    public BK_GameManagerBugs gM;
    public BK_LivesManager livesMan;

    public InputActionAsset InputActions;

    InputAction moveAction;
    InputAction shootAction;
    InputAction pauseAction;

    SpriteRenderer sr;
    Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        var map = InputActions.FindActionMap("Player", true);

        moveAction = map.FindAction("Move", true);
        shootAction = map.FindAction("Attack", true);
        pauseAction = map.FindAction("Pause", true);
    }

    void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
        pauseAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
        pauseAction.Disable();
    }

    void Start()
    {
        livesMan.livesCounter = lives;
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
            gM.TogglePause();

        if (!sr.enabled) return;

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        transform.Translate(Vector2.right * moveInput.x * moveSpeed * Time.deltaTime);

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, minPos, maxPos),
            transform.position.y
        );

        if (shootAction.WasPressedThisFrame())
            Shoot();
    }

    void Shoot()
    {
        if (reloading) return;

        BK_Projectile p = Instantiate(
            laser,
            transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        p.destroyed += () => reloading = false;
        reloading = true;
    }

    public void KillPlayer(GameObject other)
    {
        if (invincible) return;

        if (other != null) Destroy(other);

        lives--;
        livesMan.livesCounter = lives;

        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);

        sr.enabled = false;
        col.enabled = false;

        if (lives > 0)
            gM.StartCoroutine(gM.PlayerRespawn());
        else
            gM.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Invader") || other.CompareTag("Enemy Laser"))
            KillPlayer(other.gameObject);
    }

    public IEnumerator Blink(float duration, float interval)
    {
        float time = 0f;

        while (time < duration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(interval);
            time += interval;
        }

        sr.enabled = true;
    }

    public void Revive()
    {
        sr.enabled = true;
        col.enabled = true;
    }
}
