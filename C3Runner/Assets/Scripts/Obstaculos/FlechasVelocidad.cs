using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechasVelocidad : MonoBehaviour
{
    public Vector3 direction;
    public float force = 20;
    public bool contrario;


    void Start()
    {
        //Si no se le asigna,
        if (direction == Vector3.zero)
        {
            direction = transform.right;
        }

        if (contrario)
        {
            direction *= -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
