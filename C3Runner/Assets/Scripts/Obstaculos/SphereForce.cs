using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereForce : MonoBehaviour
{
    Rigidbody rb;
    float force = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //force *= GameManager.gravityScale;
        rb.AddForce(Vector3.right * force, ForceMode.Impulse);

    }

}
