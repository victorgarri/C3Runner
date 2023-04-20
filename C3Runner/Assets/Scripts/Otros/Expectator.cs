using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expectator : NetworkBehaviour
{

    PlayerSpot playerSpotter;
    GameObject currentplayer;
    [SyncVar] public bool isExpectator;
    bool ready;

    void Start()
    {

        if (!isServer)
        {
            Destroy(gameObject.GetComponent<Expectator>());
        }
        else
        {
            isExpectator = true;
            StartCoroutine("DelayExecution");
        }
    }

    IEnumerator DelayExecution()
    {
        yield return new WaitForSeconds(5);
        playerSpotter = GameObject.Find("3DScene").transform.Find("Others").Find("PlayerSpotter").GetComponent<PlayerSpot>();

        GetComponent<NetworkRigidbody>().enabled = false;
        GetComponent<NetworkAnimator>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        var playerEx = GetComponent<Player3D>();
        playerEx.DisableFeatures();
        transform.Find("Character").gameObject.SetActive(false);
        transform.Find("Main Camera").gameObject.SetActive(true);
        transform.Find("CM player").gameObject.SetActive(true);
        ready = true;
    }


    public Vector2 min;
    public Vector2 max;
    public float smooth;
    Vector2 velocity;

    public bool lockY = false;

    private void FixedUpdate()
    {
        if (ready)
        {
            currentplayer = playerSpotter.players[0].gameObject;

            float posX, posZ;

            posX = Mathf.SmoothDamp(transform.position.x, currentplayer.transform.position.x, ref velocity.x, smooth);
            posZ = Mathf.SmoothDamp(transform.position.z, currentplayer.transform.position.z, ref velocity.y, smooth);

            //if (!lockY)
            //{
            //    transform.position = new Vector3(Mathf.Clamp(posX, min.x, max.x), Mathf.Clamp(posY, min.y, max.y), transform.position.z);
            //}
            //else
            //{
            //    transform.position = new Vector3(Mathf.Clamp(posX, min.x, max.x), transform.position.y, transform.position.z);
            //}

            //transform.position = new Vector3(Mathf.Clamp(posX, min.x, max.x), transform.position.y, Mathf.Clamp(posZ, min.y, max.y));
            transform.position = new Vector3(posX, transform.position.y, posZ);
        }
    }
}
