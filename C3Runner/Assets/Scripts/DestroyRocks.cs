using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRocks : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyingRocks",12);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Vallas")
        {
            Destroy(gameObject);
        }
    }

    private void DestroyingRocks()
    {
        Destroy(gameObject);
    }
}
