using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 1;
    public bool canMove = false;
    public float canMoveOffset = 20;
    public float despawnOffset = 25;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            transform.Translate(direction.normalized * speed);
        }

        if (!canMove && (cam.transform.position.x + canMoveOffset) > transform.position.x)
        {
            canMove = true;
        }


        if ((cam.transform.position.x - despawnOffset) > transform.position.x)
        {
            Destroy(gameObject);
        }

    }



}
