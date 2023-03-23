using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    public Transform[] waypoints;

    private int currentWpIndex;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        print(gameObject.name);
        _navMeshAgent.SetDestination(waypoints[currentWpIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            currentWpIndex = (currentWpIndex + 1) % waypoints.Length;
            _navMeshAgent.SetDestination(waypoints[currentWpIndex].position);
        }
    }
}
