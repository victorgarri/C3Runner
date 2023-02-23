using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    string VEL = "vel", VELY = "vely", GROUNDED = "grounded", JUMP = "jump";

    //PHYSICS
    Rigidbody rb;
    Collider col;
    public float speed = 20;
    public float jumpForce = 20;

    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    [SyncVar] Vector2 vel = new Vector2();


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



    //
    PlayerInput pi;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        speed *= GameManager.gravityScale;
        jumpForce *= GameManager.gravityScale;

        model = transform.Find("Character").gameObject;
        anim = model.GetComponent<Animator>();


        if (!isLocalPlayer)
        {
            gameObject.transform.Find("Main Camera").gameObject.SetActive(false);
            gameObject.transform.Find("CM player").gameObject.SetActive(false);
        }

        //if (isLocalPlayer)
        if (focused && isLocalPlayer)
        {

            //rb = GetComponent<Rigidbody>();
            //col = GetComponent<Collider>();
            //speed *= GameManager.gravityScale;
            //jumpForce *= GameManager.gravityScale;

            //model = transform.Find("Character").gameObject;
            //anim = model.GetComponent<Animator>();

            pi = GetComponent<PlayerInput>();

            GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.3f);

        }
        //else
        //{
        //    pi.SwitchCurrentActionMap("Player2");
        //    gameObject.transform.Find("Main Camera").gameObject.SetActive(false);
        //    gameObject.transform.Find("CM player").gameObject.SetActive(false);
        //    GetComponent<PlayerInput>().enabled = false;
        //    GetComponent<PlayerController>().enabled = false;

        //}
    }

    bool focused = true;
    private void OnApplicationFocus(bool focus)
    {
        focused = focus;
        if (focus && isLocalPlayer)
        {
            GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.3f);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
        }
    }


    void Update()
    {
        //if (isLocalPlayer)
        if (focused && isLocalPlayer)
        {
            isGrounded();

            inputWASD = GetInputMovement();
            inputArrows = GetInputCamera();
            space = GetInputButtonSouth();

            SpaceBuffer();

            inputWASD.Normalize();
            inputArrows.Normalize();
        }
    }

    void FixedUpdate()
    {
        //if (isLocalPlayer)
        if (focused && isLocalPlayer)
        {
            //vel = inputWASD * speed;
            UpdateVel(inputWASD * speed);

            if (!grounded)
            {
                //vel /= 4;//midair velocity
                UpdateVel(vel / 4);//midair velocity
            }

        }

        Animation();

        if (focused && isLocalPlayer)
        {
            Move();
            Rotate();
            Jump();
        }
    }


    [Command]
    void UpdateVel(Vector2 newVel)
    {
        if (vel != newVel)
            vel = newVel;
    }

    Vector3 lookVel = new Vector3();

    void RotateCharacter()
    {

        lookVel.x = vel.x;
        lookVel.y = 0;
        lookVel.z = vel.y;

        lookVel = transform.rotation * lookVel;
        if (lookVel != Vector3.zero)
        {
            model.transform.rotation = Quaternion.LookRotation(lookVel);
            model.transform.forward = lookVel;
        }
    }

    void Animation()
    {
        anim.SetFloat(VEL, vel.normalized.magnitude);

        //bool jumpAnimStarted = false;

        //if (grounded && !spaceConsumed)
        //{
        //    anim.SetTrigger(JUMP);
        //    //jumpAnimStarted = true;
        //}
        anim.SetFloat(VELY, Mathf.Abs(rb.velocity.y));
        anim.SetBool(GROUNDED, grounded);

        RotateCharacter();

    }

    public float dist = .3f;
    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (col.bounds.extents.y + dist), Color.red, 1 / 60);
        grounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + dist);
        return grounded;
    }

    //bool isGrounded()
    //{
    //    grounded = groundCount > 0;
    //    return grounded;
    //}

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
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            spaceConsumed = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger(JUMP);
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









    //////////////
    ///
    Vector2 inputHandlerWASD = new Vector2();
    Vector2 inputHandlerArrows = new Vector2();
    bool inputHandlerButtonSouth;
    public Vector2 GetInputMovement()
    {
        //reset input
        inputHandlerWASD = Vector2.zero;
        inputHandlerWASD = pi.actions["Movement"].ReadValue<Vector2>().normalized;

        return inputHandlerWASD;
    }

    public Vector2 GetInputCamera()
    {
        //reset input
        inputHandlerArrows = Vector2.zero;
        inputHandlerArrows = pi.actions["Camera"].ReadValue<Vector2>().normalized;

        return inputHandlerArrows;
    }

    public bool GetInputButtonSouth()
    {
        //reset input
        inputHandlerButtonSouth = false;
        inputHandlerButtonSouth = pi.actions["Jump"].WasPressedThisFrame();

        return inputHandlerButtonSouth;
    }

}
