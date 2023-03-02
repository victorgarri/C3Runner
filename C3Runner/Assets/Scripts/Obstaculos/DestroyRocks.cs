using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRocks : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 15);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Vallas")
        {
            Destroy(gameObject, 5);
        }
    }
}
