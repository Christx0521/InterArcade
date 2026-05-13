using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyPlat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entró al trigger: " + other.gameObject.name + 
                  " | Tag: " + other.tag + 
                  " | Layer: " + LayerMask.LayerToName(other.gameObject.layer));

        if (other.CompareTag("Barril") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}