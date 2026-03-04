using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PAC_Movement))]
public class PAC_Pacman : MonoBehaviour
{
    [SerializeField]
    private PAC_AnimatedSprite deathSequence;

    public InputActionAsset inputActions;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private PAC_Movement movement;

    private InputAction moveAction;
    private InputAction pauseAction;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<PAC_Movement>();

        if (inputActions != null)
        {
            var map = inputActions.FindActionMap("Player");
            moveAction = map.FindAction("Move");
            pauseAction = map.FindAction("Pause");
        }
    }

    private void OnEnable()
    {
        inputActions?.FindActionMap("Player")?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.FindActionMap("Player")?.Disable();
    }

    private void Update()
    {
        Vector2 inputDir = Vector2.zero;

        if (moveAction != null)
        {
            Vector2 raw = moveAction.ReadValue<Vector2>();

            if (raw.magnitude > 0.1f)
            {
                if (Mathf.Abs(raw.x) > Mathf.Abs(raw.y))
                    inputDir = new Vector2(Mathf.Sign(raw.x), 0f);
                else
                    inputDir = new Vector2(0f, Mathf.Sign(raw.y));
            }
        }

        if (inputDir == Vector2.zero)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
                inputDir = Vector2.up;
            else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
                inputDir = Vector2.down;
            else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
                inputDir = Vector2.left;
            else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
                inputDir = Vector2.right;
        }

        if (inputDir != Vector2.zero)
        {
            movement.SetDirection(inputDir);
        }

        if (movement.direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (pauseAction != null && pauseAction.triggered)
        {
            PAC_GameManager.Instance.TogglePause();
        }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
}