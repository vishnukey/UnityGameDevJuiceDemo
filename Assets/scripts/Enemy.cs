using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float bulletStrength;
    private float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage()
    {
        health -= bulletStrength;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
