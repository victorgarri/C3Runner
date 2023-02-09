using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float force = 10;
    Transform direction;

    void Start()
    {
        direction = transform.Find("Direction");
        force *= GameManager.gravityScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.CompareTag("Player"))
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero; //Reset velocity
            obj.GetComponent<Rigidbody>().AddForce(direction.up * force, ForceMode.Impulse);
        }
    }
}
