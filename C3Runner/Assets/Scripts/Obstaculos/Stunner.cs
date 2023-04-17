using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunner : MonoBehaviour
{
    Rigidbody rb;
    public float speedThreshold = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && rb.velocity.magnitude >= speedThreshold)
        {
            col.gameObject.GetComponent<Player3D>().GetStunned();
        }
    }
}
