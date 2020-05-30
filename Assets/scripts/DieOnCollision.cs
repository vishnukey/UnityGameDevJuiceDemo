using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
