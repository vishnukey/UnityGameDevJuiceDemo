using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private float gunStrength = 500f;
    [SerializeField] private float shotTTL = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shoot();
    }

    private void Shoot()
    {
        var shot = Instantiate(projectile, hand.position, hand.rotation);
        shot.AddForce(hand.forward * gunStrength);
        Destroy(shot.gameObject, shotTTL);
    }
}
