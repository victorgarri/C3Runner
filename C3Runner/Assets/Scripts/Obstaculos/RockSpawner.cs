using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] rockList;
    //[SerializeField] private GameObject player;


    void RockSpawning()
    {
        print("pepe");
        Instantiate(rockList[Random.Range(0, rockList.Length)],transform.position,transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            InvokeRepeating("RockSpawning", 0f, 3f);
            //if (obj.transform.position.x > 812 && (obj.transform.position.z > 140 || obj.transform.position.z < 70) && !spawning)
            //{
            //    InvokeRepeating("RockSpawning", 0f, 3f);
            //    spawning = true;
            //}
            //else if (obj.transform.position.x > 950)
            //{
            //    CancelInvoke();
            //    enabled = false;
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke("RockSpawning");
        //enabled = false;
    }



}
