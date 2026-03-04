using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PAC_Passage : MonoBehaviour
{
    public Vector2 validDirection;
    public Transform connection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PAC_Movement>() != null)
        {
            Vector2 direction = other.gameObject.GetComponent<PAC_Movement>().direction;
            if (direction == validDirection)
            {
                Vector3 position = connection.position;
                position.z = other.transform.position.z;
                other.transform.position = position;
            }
        }

    }
}
