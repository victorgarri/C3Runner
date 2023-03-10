using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRockSpawner : MonoBehaviour
{
    public float spawnheight = 140;
    public float spawnRate = 0.3f;

    public GameObject[] rocks;
    public Transform[] spawnPositions;
    int currentSpawnPositionIndex;

    int playerInsideCount;
    bool keepSpawning;

    void Start()
    {
        InvokeRepeating("SpawnRock", 0, spawnRate);

    }

    void SpawnRock()
    {
        if (KeepSpawning())
        {
            int randomIndex = Random.Range(0, rocks.Length);

            Instantiate(rocks[randomIndex], spawnPositions[currentSpawnPositionIndex].position, Quaternion.identity);

            currentSpawnPositionIndex = (currentSpawnPositionIndex + 1) % spawnPositions.Length;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCount--;
        }
    }

    bool KeepSpawning()
    {
        keepSpawning = playerInsideCount > 0;

        return keepSpawning;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

}
