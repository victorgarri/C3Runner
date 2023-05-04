using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Rigidbody rb;
    public float force = 10;
    public int collisionNumber = 0;
    public int collisionNumberMax = 20;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            collisionNumber++;

            if (collisionNumber >= collisionNumberMax)
            {
                Destroy(gameObject);
            }
            //foreach (var item in col.contacts)
            //{
            //    Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), .2f);
            //}
        }
    }

}
