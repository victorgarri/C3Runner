using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : NetworkBehaviour
{
    Rigidbody rb;
    public float force = 10;
    public int collisionNumber = 0;
    public int collisionNumberMax = 20;
    public GameObject explosionFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Instantiate(explosionFX, transform.position, transform.rotation, null);
    }


    [ServerCallback]
    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag == "Ground")
        //{
        collisionNumber++;

        if (collisionNumber >= collisionNumberMax)
        {
            DestroySelf();
        }
        //foreach (var item in col.contacts)
        //{
        //    Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), .2f);
        //}
        //}
    }

    [Server]
    public void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    void OnDestroy()
    {
        Instantiate(explosionFX, transform.position, transform.rotation, null);
    }

}
