using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Lazer : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnBecameInvisible() => Destroy(gameObject);
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;

        if (other.gameObject.TryGetComponent<Health>(out var Health))
        {
            Health.DealDamage(25);
        }

        Destroy(gameObject);
    }
}
