using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public int health = 100;
    public int currentHealth;
    [SerializeField] private GameObject explosion;

    private bool isExploding = false;

    void Start()
    {
        currentHealth = health;
    }

    public void DealDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth <= 0 && !isExploding)
        {
            isExploding = true;

            if (!gameObject.CompareTag("Player"))
            {
                GameObject.FindWithTag("GameController")?.GetComponent<Score>()?.IncrementScore();
            }

            var explosionGameObject = Instantiate(explosion, transform.position,
                Quaternion.Euler(0, 0, 0));

            Destroy(gameObject, 0.5f);
            Destroy(explosionGameObject, 1f);
        }
    }
}
