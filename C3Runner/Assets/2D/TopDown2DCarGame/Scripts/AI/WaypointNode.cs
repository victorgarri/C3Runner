using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    //what is the max speed allowed when passing this waypoint
    [Header("Speed set once we reach the waypoint")]
    public float maxSpeed = 0;

    [Header("This is the waypoint we are going towards, not yet reached")]
    public float minDistanceToReachWaypoint = 5;

    public WaypointNode[] nextWaypointNode;

    private void Start()
    {
        //Check and ensure that there is a waypoint assigned
        if (nextWaypointNode.Length == 0)
            Debug.LogError($"Waypoint {gameObject.name} is missing a nextWaypointNode. Please assign one in the inspector");
        else
        {
            //Check all the nodes and make sure they have a valid assiged waypoint
            foreach(WaypointNode waypoint in nextWaypointNode)
            {
                if(waypoint == null)
                    Debug.LogError($"Waypoint {gameObject.name} has an empty nextWaypointNode. Please assign one in the inspector");
            }
        }
    }
}
