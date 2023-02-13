using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PHYSICS
    Rigidbody rb;
    public float speed = 20;
    public float jumpForce = 20;

    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    Vector2 vel = new Vector2();


    //space key buffer
    bool space, spaceConsumed = true;
    float spaceConsumeTimer;
    readonly float spaceConsumeMaxTime = 0.5f;

    //ground stuff
    int groundCount = 0;
    bool grounded;


    //Model
    Animator anim;
    GameObject model;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed *= GameManager.gravityScale;
        jumpForce *= GameManager.gravityScale;

        model = transform.Find("Character").gameObject;
        anim = model.GetComponent<Animator>();

    }


    void Update()
    {
        isGrounded();

        inputWASD = GameManager.GetInputMovement();
        inputArrows = GameManager.GetInputCamera();
        space = GameManager.GetInputButtonSouth();

        SpaceBuffer();

        inputWASD.Normalize();
        inputArrows.Normalize();
    }

    void FixedUpdate()
    {
        vel = inputWASD * speed;

        if (!grounded)
        {
            vel /= 4;//midair velocity
        }

        Animation();

        Move();
        Rotate();
        Jump();



    }

    void Animation()
    {
        anim.SetFloat("vel", vel.normalized.magnitude);

        //bool jumpAnimStarted = false;

        if (grounded && !spaceConsumed)
        {
            anim.SetTrigger("jump");
            //jumpAnimStarted = true;
        }
        anim.SetFloat("vely", Mathf.Abs(rb.velocity.y));
        anim.SetBool("grounded", grounded);



    }

    bool isGrounded()
    {
        grounded = groundCount > 0;
        return grounded;
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
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        rb.AddForce(forward * vel.y + right * vel.x);



    }

    void Jump()
    {
        if (isGrounded() && !spaceConsumed)
        {
            spaceConsumed = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    float rotationSpeed = 2;
    void Rotate()
    {
        if (inputArrows.x != 0)
        {
            transform.Rotate(new Vector3(0, inputArrows.x * rotationSpeed, 0));
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        var g = c.gameObject;
        switch (g.tag)
        {
            case "Ground":
                groundCount++;
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
                break;
        }

    }
}
