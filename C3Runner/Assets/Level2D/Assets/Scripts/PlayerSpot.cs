using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpot : MonoBehaviour
{
    List<Player3D> players = new List<Player3D>();

    void Start()
    {
        StartCoroutine("CountPlayers");
    }

    IEnumerator CountPlayers()
    {
        yield return new WaitForSeconds(4);
        players = FindObjectsOfType<Player3D>().ToList();
        InvokeRepeating("UpdatePlayerSpot", 0, 1);
    }




    void Update()
    {

    }


    void UpdatePlayerSpot()
    {
        players.Sort((p, q) => p.distanceFromZero.CompareTo(q.distanceFromZero));

        for (int i = 0; i < players.Count; i++)
        {
            players[0].spot = i + 1;
        }
        print(players);
    }
}
