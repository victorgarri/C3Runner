using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVNIWaypoint : MonoBehaviour
{
    public Transform zero;
    public Vector3 pos;
    public float distanceFromZero;

    void Start()
    {
        pos = transform.position;
        zero = GameObject.Find("Zero").transform;
        UpdateDistanceFromZero();
    }

    void UpdateDistanceFromZero()
    {
        distanceFromZero = Vector3.Distance(pos, zero.position);
    }

}
