using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJuice : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float bulletStrength;
    [SerializeField] private ParticleSystem sparksPrefab; 
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

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"hit from: {other.gameObject.name}");
        if (other.collider.CompareTag("Projectile"))
        {
            var contact = other.contacts[0];
            var point = contact.point;
            var norm = contact.normal;
            TakeDamage();
            var sparks = Instantiate(sparksPrefab, point, Quaternion.LookRotation(norm));
            Destroy(sparks, sparksPrefab.main.duration);
            //Destroy(other.gameObject);
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
