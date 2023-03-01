using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] rockList;
    [SerializeField] private GameObject player;

    private bool spawning;
    // Start is called before the first frame update
    private void Update()
    {
        if (player.transform.position.x > 812 && (player.transform.position.z > 140 || player.transform.position.z < 70) && !spawning)
        {
            InvokeRepeating("RockSpawning",0f,3f);
            spawning = true;
        }
        else if (player.transform.position.x > 950)
        {
            CancelInvoke();
            enabled = false;
        }
        
    }

    // Update is called once per frame
    void RockSpawning()
    {
        print("pepe");
        Instantiate(rockList[Random.Range(0, rockList.Length)],transform.position,transform.rotation);
    }
}
