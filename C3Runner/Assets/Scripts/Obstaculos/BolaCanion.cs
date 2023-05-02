using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaCanion : NetworkBehaviour
{
    Rigidbody rb;
    GameObject body, barrel;
    public Vector3 scale;
    public Vector3 dir;

    void Start()
    {
        //transform.localScale = scale;

        //body = transform.parent.Find("Parts").Find("Body").gameObject;
        //barrel = body.transform.Find("Barrel").gameObject;
        //body = transform.parent.GetComponent<ShooterCanion>().body;
        //barrel = transform.parent.GetComponent<ShooterCanion>().barrel;

        rb = GetComponent<Rigidbody>();
        //transform.parent = null;
        rb.AddForce(150 * dir, ForceMode.Impulse);
        

        StartCoroutine("delay");
    }

    // destroy for everyone on the server
    //[ClientRpc]
    public void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(12);
        DestroySelf();
    }

}
