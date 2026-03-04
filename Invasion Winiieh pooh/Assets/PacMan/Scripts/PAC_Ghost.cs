using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(PAC_Movement))]
public class PAC_Ghost : MonoBehaviour
{
    public PAC_Movement movement { get; private set; }
    public PAC_GhostHome home { get; private set; }
    public PAC_GhostScatter scatter { get; private set; }
    public PAC_GhostChase chase { get; private set; }
    public PAC_GhostFrightened frightened { get; private set; }
    public PAC_GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<PAC_Movement>();
        home = GetComponent<PAC_GhostHome>();
        scatter = GetComponent<PAC_GhostScatter>();
        chase = GetComponent<PAC_GhostChase>();
        frightened = GetComponent<PAC_GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
            home.Disable();

        if (initialBehavior != null)
            initialBehavior.Enable();
    }

    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                PAC_GameManager.Instance.GhostEaten(this);
            }
            else
            {
                PAC_GameManager.Instance.PacmanEaten();
            }
        }
    }
}
