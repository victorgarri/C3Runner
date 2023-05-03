using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] rockList;
    //[SerializeField] private GameObject player;
    public float spawnFrequency = 3;
    public List<Transform> targets = new List<Transform>();

    private void Start()
    {
        //InvokeRepeating("RockSpawning", 0f, spawnFrequency);
    }

    [ServerCallback]
    void RockSpawning()
    {
        //print("pepe");
        if (targets.Count > 0)
        {
            GameObject go = Instantiate(rockList[Random.Range(0, rockList.Length)], transform.position, transform.rotation);
            NetworkServer.Spawn(go);
        }

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

    [ServerCallback]
    void OnEnable()
    {
        InvokeRepeating("RockSpawning", 0f, spawnFrequency);
    }

    [ServerCallback]
    void OnDisable()
    {
        CancelInvoke("RockSpawning");
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    CancelInvoke("RockSpawning");
    //    //enabled = false;
    //}



}
