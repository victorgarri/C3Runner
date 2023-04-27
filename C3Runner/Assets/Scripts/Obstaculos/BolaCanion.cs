using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaCanion : NetworkBehaviour
{
    Rigidbody rb;
    GameObject body, barrel;

    void Start()
    {
        body = transform.parent.Find("Parts").Find("Body").gameObject;
        barrel = body.transform.Find("Barrel").gameObject;
        rb = GetComponent<Rigidbody>();
        transform.parent = null;
        rb.AddForce(150 * -barrel.transform.forward, ForceMode.Impulse);
        

        StartCoroutine("delay");
    }

    // destroy for everyone on the server
    [ClientRpc]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(12);
        DestroySelf();
    }

}
