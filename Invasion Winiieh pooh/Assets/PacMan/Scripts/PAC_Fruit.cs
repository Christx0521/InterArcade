using UnityEngine;

public class PAC_Fruit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PAC_GameManager.Instance.FruitEaten();
            Destroy(gameObject);
        }
    }
}
