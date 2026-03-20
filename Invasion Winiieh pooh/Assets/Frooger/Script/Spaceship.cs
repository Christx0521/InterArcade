using UnityEngine;
using System.Linq;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject[] cannon;
    [SerializeField] private float cooldown = 1f;



    private Rigidbody rigidBody;
    private int currentCannon = 0;

    private float time = 0f;

    private Collider triggerCollider;
    private Camera mainCamera;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        triggerCollider = GetComponents<Collider>().First(c => c.isTrigger);
    }

    void Update()
    {
        // Laser
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            var laserOriginTransform = transform;
            if (cannon.Length > 0)
            {
                laserOriginTransform = cannon[currentCannon++].transform;
                if (currentCannon >= cannon.Length)
                {
                    currentCannon = 0;
                }
            }

            Instantiate(laser, laserOriginTransform
                .TransformPoint(Vector3.forward * 2), transform.rotation);
            time = cooldown;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddForce(transform.forward * (movementSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddForce(transform.forward * (-movementSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }

        if (!IsPlayerVisible())
        {
            Vector3 playerPos = transform.position;

            Vector3 screenCenter = mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width / 2, Screen.height / 2,
                Mathf.Abs(mainCamera.transform.position.y - playerPos.y))
            );

            transform.position = new Vector3(
                screenCenter.x - playerPos.x,
                playerPos.y,
                screenCenter.z - playerPos.z
            );
        }

        bool IsPlayerVisible()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            return GeometryUtility.TestPlanesAABB(planes, triggerCollider.bounds);
        }


    }
    void OnDestroy()
    {
        GameObject.FindWithTag("GameController")?.GetComponent<Score>()?.GameOver();
    }
}
