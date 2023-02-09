using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController : MonoBehaviour
{
    //PHYSICS
    Rigidbody rb;
    public float speed = 20;
    public float jumpForce = 20;
    private bool PositivePowerUp;
    private bool NegativePowerUp;
    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    Vector2 vel = new Vector2();


    //space key buffer
    bool space, spaceConsumed = true;
    float spaceConsumeTimer;
    readonly float spaceConsumeMaxTime = 1f;

    //
    bool isGrounded;

    private int groundCount = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed *= GameManager.gravityScale;
        jumpForce *= GameManager.gravityScale;

    }


    void Update()
    {
        grounded();
        inputWASD = GameManager.GetInputWASD();
        inputArrows = GameManager.GetInputArrows();
        space = Input.GetKeyDown(KeyCode.Space);

        SpaceBuffer();

        inputWASD.Normalize();
        inputArrows.Normalize();
        Debug.Log(speed);
    }

    void FixedUpdate()
    {
        vel = inputWASD * speed;

        if (!isGrounded)
        {
            vel /= 4;//midair velocity
        }

        Move();
        Rotate();

        if (isGrounded && !spaceConsumed)
        {
            spaceConsumed = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void SpaceBuffer()
    {
        //if pressed space on this frame, store so
        if (space)
        {
            spaceConsumed = false;
        }

        //if after x time spaceConsumed is still false, set to true and reset timer
        if (!spaceConsumed)
        {
            spaceConsumeTimer += Time.deltaTime;
            if (spaceConsumeTimer >= spaceConsumeMaxTime)
            {
                spaceConsumed = true; //consume regardless
                spaceConsumeTimer = 0;
            }
        }
    }


    void Move()
    {
        //rb.AddForce(new Vector3(vel.x, 0, vel.y)); //old

        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        rb.AddForce(forward * vel.y + right * vel.x);



    }

    float rotationSpeed = 2;
    void Rotate()
    {
        if (inputArrows.x != 0)
        {
            transform.Rotate(new Vector3(0, inputArrows.x * rotationSpeed, 0));
        }
    }


    void grounded()
    {
        isGrounded = groundCount > 0;
        
    }

    void powerUpOff()
    {
        if (PositivePowerUp)
        {
            PositivePowerUp = false;
            speed -= 10;
        }else if (NegativePowerUp)
        {
            NegativePowerUp = false;
            speed += 5;
        }
    }
    private void OnCollisionEnter(Collision c)
    {
        var g = c.gameObject;
        switch (g.tag)
        {
            case "Ground":
                groundCount++;
                //isGrounded = true;
                break;
        }
    }

    private void OnCollisionExit(Collision c)
    {
        var g = c.gameObject;
        switch (g.tag)
        {
            case "Ground":
                groundCount--;
                //isGrounded = false;
                break;
        }

    }
    
    private void OnTriggerEnter(Collider c)
    {
        var g = c.gameObject;
        switch (g.tag)
        {
            case "PositiveArrow":
                if (!PositivePowerUp && !NegativePowerUp)
                {
                    PositivePowerUp = true;
                    speed += 10;
                    Invoke("powerUpOff", 5f);
                }
                break;
            case  "NegativeArrow":
                if (!PositivePowerUp && !NegativePowerUp)
                {
                    NegativePowerUp = true;
                    speed -= 5; 
                    Invoke("powerUpOff", 5f);
                }
                break;
        }
    }
}

