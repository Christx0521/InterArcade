using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Win");
            FindAnyObjectByType<Gameover>().WinGame();
        }
    }
}
