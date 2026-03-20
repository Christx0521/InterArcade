using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 1f;
    public float size = 1f;

    private Vector3 leftEdge;
    private Vector3 rightEdge;

    public bool EndGame = false;

    void Start()
    {
        float distance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
    }

    void Update()
    {

        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            transform.position = new Vector3(leftEdge.x - size, transform.position.y, transform.position.z);
        }

        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            transform.position = new Vector3(rightEdge.x + size, transform.position.y, transform.position.z);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over");
            FindAnyObjectByType<Gameover>().GameOver();
        }
    }
}
