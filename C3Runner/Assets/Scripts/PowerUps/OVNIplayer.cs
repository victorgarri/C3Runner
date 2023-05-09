using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OVNIplayer : MonoBehaviour
{
    public List<OVNIWaypoint> ovniWaypoints;
    public float maxpoints = 2; //in this case it's number of waypoints that will be taken into account
    public float speed = 100;
    public Player3D localplayer;

    //public Player3D debugPlayer;
    int indexOfWP = 0;

    Transform nextWaypoint;
    bool readyToGo;

    public void Activate(Player3D p)
    {
        localplayer = p;
        //var explosion = Instantiate(explosionFX, transform.position, transform.rotation, null);
        //explosion.transform.localScale *= 20;
        StartUp();

    }
    void OnEnable()
    {
        localplayer = GetComponent<Player3D>();
        StartUp();
    }

    void StartUp()
    {
        //debug
        //if (localplayer == null)
        //{
        //    localplayer = debugPlayer;
        //}

        GetAllWaypoints();
        RemovePreviousPoints();
        SortWaypointsByDistance();
        PickFirstWaypoint();

        ParentPlayerToOVNI(); //parent player to ovni
        readyToGo = true;
        //start moving OVNI
        MoveOvni();
    }

    void Update()
    {
        if (readyToGo)
        {
            MoveOvni();
            //MovePlayer();
        }
    }

    void GetAllWaypoints()
    {
        ovniWaypoints = GameObject.FindObjectsOfType<OVNIWaypoint>().ToList();
    }
    void RemovePreviousPoints()
    {
        List<OVNIWaypoint> dummyList = new List<OVNIWaypoint>();
        foreach (var wp in ovniWaypoints)
        {
            //valido
            if (wp.transform.position.x >= transform.position.x)
            {
                dummyList.Add(wp);
            }
        }

        ovniWaypoints = dummyList;
    }

    void SortWaypointsByDistance()
    {
        ovniWaypoints.Sort((p, q) => p.distanceFromZero.CompareTo(q.distanceFromZero));
        //ovniWaypoints.Reverse(); //first should be the closest to the player
    }

    void PickFirstWaypoint()
    {
        nextWaypoint = ovniWaypoints[0].transform;
    }

    void ParentPlayerToOVNI()
    {
        //localplayer.transform.parent = this.transform;
        localplayer.pi.enabled = false;
        localplayer.DisableRB();
    }

    void UnarentPlayerToOVNI()
    {
        //localplayer.transform.parent = null;
        localplayer.pi.enabled = true;
        localplayer.EnableRB();
        localplayer.GetComponent<Player3D>().OVNIToggle();
        
    }


    float changeDist = 0.3f;
    void MoveOvni()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextWaypoint.position) < changeDist)
        {
            pickNextWaypoint();
        }
    }

    void pickNextWaypoint()
    {
        indexOfWP++;
        nextWaypoint = ovniWaypoints[indexOfWP].transform;

        if (indexOfWP >= maxpoints || nextWaypoint == null)
        {
            //Destroy OVNI and drop players
            UnarentPlayerToOVNI();
        }
    }

}
