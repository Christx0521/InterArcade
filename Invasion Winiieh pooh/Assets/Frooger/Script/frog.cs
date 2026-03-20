using UnityEngine;
using System.Collections;
using TMPro;

public class FroggerMovement : MonoBehaviour
{
    public Vector3 gridSize = new Vector3(0, 0, 1);
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    public bool EndGame = false;

    void Start()
    {
        targetPosition = transform.position; 
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                targetPosition += new Vector3(0, 0, gridSize.z);
                StartCoroutine(Move());
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                targetPosition -= new Vector3(0, 0, gridSize.z);
                StartCoroutine(Move());
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                targetPosition -= new Vector3(gridSize.x, 0, 0);
                StartCoroutine(Move());
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                targetPosition += new Vector3(gridSize.x, 0, 0);
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;
    }


}

