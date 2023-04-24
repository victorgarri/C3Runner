using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followTarget;
    public Vector2 min;
    public Vector2 max;
    public float smooth;
    Vector2 velocity;

    public bool lockY = false;

    private void FixedUpdate()
    {
        float posX, posY;

        posX = Mathf.SmoothDamp(transform.position.x, followTarget.transform.position.x, ref velocity.x, smooth);
        posY = Mathf.SmoothDamp(transform.position.y, followTarget.transform.position.y, ref velocity.y, smooth);

        if (!lockY)
        {
            transform.position = new Vector3(Mathf.Clamp(posX, min.x, max.x), Mathf.Clamp(posY, min.y, max.y), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(posX, min.x, max.x), transform.position.y, transform.position.z);
        }
    }
}
