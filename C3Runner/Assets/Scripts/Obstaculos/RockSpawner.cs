using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] rockList;
    //[SerializeField] private GameObject player;
    public float spawnFrequency = 3;
    public List<Transform> targets = new List<Transform>();

    private void Start()
    {
        //InvokeRepeating("RockSpawning", 0f, spawnFrequency);
    }

    void RockSpawning()
    {
        //print("pepe");
        if (targets.Count > 0)
        {
            Instantiate(rockList[Random.Range(0, rockList.Length)], transform.position, transform.rotation);
        }




    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    var obj = other.gameObject;

    //    if (obj.tag == "Player")
    //    {

    //        InvokeRepeating("RockSpawning", 0f, spawnFrequency);
    //        //if (obj.transform.position.x > 812 && (obj.transform.position.z > 140 || obj.transform.position.z < 70) && !spawning)
    //        //{
    //        //    InvokeRepeating("RockSpawning", 0f, 3f);
    //        //    spawning = true;
    //        //}
    //        //else if (obj.transform.position.x > 950)
    //        //{
    //        //    CancelInvoke();
    //        //    enabled = false;
    //        //}
    //    }
    //}


    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Add(c.gameObject.transform);


        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Remove(c.gameObject.transform);


        }
    }

    void OnEnable()
    {
        InvokeRepeating("RockSpawning", 0f, spawnFrequency);
    }

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
