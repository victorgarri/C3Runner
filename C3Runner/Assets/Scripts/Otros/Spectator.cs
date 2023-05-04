using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class Spectator : NetworkBehaviour
{

    PlayerSpot playerSpotter;
    GameObject currentplayer;
    [SyncVar] public bool isSpectator;
    bool ready;
    public bool wantsToSpectate;

    float height = 56; //56
    public float horizontalOffset = 67.25f; //67.25
    public float aimTrackedObjectOffsetY = -12.56f;//aim tracked object offset, y = -12.56

    CinemachineVirtualCamera cam;
    CinemachineComposer fram;

    void Start()
    {
        if (isServer && isLocalPlayer)
        {
            if (wantsToSpectate)
            {
                isSpectator = true;
                transform.Find("Character").gameObject.SetActive(false);
                StartCoroutine("DelayExecution");
            }
        }
        else
        {
            //delay execution and check if it's expectating, then disable its components
            StartCoroutine("DelayExecutionClient");
        }
    }

    IEnumerator DelayExecution()
    {

        yield return new WaitForSeconds(5);
        if (isServer && isLocalPlayer)
        {
            playerSpotter = GameObject.Find("3DScene").transform.Find("Others").Find("PlayerSpotter").GetComponent<PlayerSpot>();

            GetComponent<NetworkRigidbody>().enabled = false;
            GetComponent<NetworkAnimator>().enabled = false;
            GetComponent<AudioSource>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            var playerEx = GetComponent<Player3D>();
            playerEx.DisableFeatures();
            transform.Find("PlayerName").gameObject.SetActive(false);
            transform.Find("Main Camera").gameObject.SetActive(true);
            transform.Find("CM player").gameObject.SetActive(true);

            cam = gameObject.transform.Find("CM player").GetComponent<CinemachineVirtualCamera>();
            fram = cam.GetCinemachineComponent<CinemachineComposer>();
            fram.m_TrackedObjectOffset.y = aimTrackedObjectOffsetY;

            //transform.Rotate(Vector3.right, 90);
            transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);

            ready = true;
        }
    }

    IEnumerator DelayExecutionClient()
    {
        yield return new WaitForSeconds(5);
        if (!(isServer))
        {
            List<Player3D> players = FindObjectsOfType<Player3D>().ToList();

            Player3D playerEx = null;
            foreach (Player3D p in players)
            {
                if (p.GetComponent<Spectator>() != null && p.GetComponent<Spectator>().isSpectator)
                {
                    playerEx = p;
                }
            }


            playerEx.GetComponent<NetworkRigidbody>().enabled = false;
            playerEx.GetComponent<NetworkAnimator>().enabled = false;
            playerEx.GetComponent<AudioSource>().enabled = false;
            playerEx.GetComponent<CapsuleCollider>().enabled = false;
            playerEx.GetComponent<Rigidbody>().useGravity = false;
            playerEx.GetComponent<Rigidbody>().velocity = Vector3.zero;

            playerEx.DisableFeatures();
            playerEx.transform.Find("Character").gameObject.SetActive(false);
        }
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
            if (playerSpotter.players.Count > 0)
            {
                //currentplayer = playerSpotter.players[playerSpotter.players.Count - 1].gameObject;

                for (int i = playerSpotter.players.Count - 1; i >= 0; i--)
                {
                    if (playerSpotter.players[i] != null)
                    {
                        currentplayer = playerSpotter.players[i].gameObject;

                        if (currentplayer.GetComponent<Player3D>().in2DGame)
                        {
                            continue;
                        }

                        float posX, posZ;

                        posX = Mathf.SmoothDamp(transform.position.x, currentplayer.transform.position.x, ref velocity.x, smooth);
                        posZ = Mathf.SmoothDamp(transform.position.z, currentplayer.transform.position.z, ref velocity.y, smooth);

                        transform.position = new Vector3(posX - horizontalOffset, transform.position.y, posZ);
                    }
                }
            }
        }
    }

}
