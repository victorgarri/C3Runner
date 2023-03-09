using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public float rotationAmount = 1;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, rotationAmount, 0));
    }
}
