using Mirror;
using System.Collections;
using UnityEngine;

public class DestroyRocks : NetworkBehaviour
{
    private void Start()
    {
        //Destroy(gameObject, 15);
        StartCoroutine("delay");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Vallas")
        {
            //Destroy(gameObject, 5);
            StartCoroutine("delay2");
        }
    }

    // destroy for everyone on the server
    [ClientRpc]
    public void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(15);
        DestroySelf();
    }

    IEnumerator delay2()
    {
        yield return new WaitForSeconds(15);
        DestroySelf();
    }
}
