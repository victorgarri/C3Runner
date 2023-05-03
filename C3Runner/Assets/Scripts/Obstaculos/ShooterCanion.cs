using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterCanion : NetworkBehaviour
{

    public GameObject bolaCanion;
    public GameObject explosionFX;
    public Transform pivot;
    public float force = 150;
    public float delay = 0;
    public float interval = 4;
    public float bulletDestroyTime = 12;
    public bool ableToShoot = true;
    public bool trackPlayer = true;
    public Vector3 offset;

    public AudioSource aud;

    public List<Transform> targets = new List<Transform>();

    public GameObject body, barrel;

    [ServerCallback]
    void Start()
    {
        //body = transform.Find("Parts").Find("Body").gameObject;
        //barrel = body.transform.Find("Barrel").gameObject;

        //InvokeRepeating("Shoot", delay, interval);
    }


    [ServerCallback]
    void OnEnable()
    {
        InvokeRepeating("Shoot", delay, interval);
    }

    [ServerCallback]
    void OnDisable()
    {
        CancelInvoke("Shoot");
    }


    [SyncVar] public int randomIndex;
    void PickTarget()
    {
        randomIndex = Random.Range(0, targets.Count);
    }

    [ServerCallback]
    void Shoot()
    {
        if (ableToShoot && targets.Count > 0)
        {
            PickTarget();
            Aim();

            GameObject go = Instantiate(bolaCanion, pivot.position, transform.rotation);
            //go.transform.parent = transform;
            //go.GetComponent<BolaCanion>().scale = go.transform.localScale;
            go.GetComponent<BolaCanion>().dir = -barrel.transform.forward;

            GameObject fx = Instantiate(explosionFX, pivot.position, transform.rotation);

            NetworkServer.Spawn(go);
            NetworkServer.Spawn(fx);
            //go.transform.parent = null;

            //Shoot forward
            //go.GetComponent<Rigidbody>().AddForce(force * -barrel.transform.forward, ForceMode.Impulse);

            aud.Stop();
            aud.Play();

            //go.GetComponent<BolaCanion>().DestroySelf();
        }

    }

    [ClientCallback]
    void Aim()
    {
        Quaternion dummy;
        Vector3 tdummy = targets[randomIndex].position;
        tdummy += offset;

        body.transform.LookAt(tdummy);
        dummy = body.transform.rotation;
        dummy.x = 0;
        //dummy.y = dummy.y + Mathf.PI; //pi is 180º, but doesn't work somehow
        dummy.z = 0;
        body.transform.rotation = dummy;
        body.transform.Rotate(new Vector3(0, 180, 0));



        barrel.transform.LookAt(targets[randomIndex]);
        dummy = barrel.transform.rotation;
        dummy = Quaternion.Euler(-dummy.eulerAngles.x, 0, 0);

        barrel.transform.localRotation = dummy;

    }

    [ServerCallback]
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Add(c.gameObject.transform);


        }
    }

    [ServerCallback]
    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Remove(c.gameObject.transform);


        }
    }

}