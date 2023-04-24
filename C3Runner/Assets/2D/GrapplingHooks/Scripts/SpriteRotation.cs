using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public Transform objectA, objectB;
    void Start()
    {

    }


    void Update()
    {
        transform.up = (objectB.position - transform.position);
    }
}
