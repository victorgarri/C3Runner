using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public float rotationAmount = 1;
    public bool forceActive = false;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        cam = Camera.main;
        if ((forceActive) || (cam != null && Vector3.Distance(transform.position, cam.transform.position) < 400))
            transform.Rotate(new Vector3(0, rotationAmount, 0));
    }
}
