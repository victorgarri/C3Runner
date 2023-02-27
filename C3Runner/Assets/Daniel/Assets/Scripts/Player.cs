using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public bool LOCAL_DEBUG;

    string VEL = "vel", VELY = "vely", GROUNDED = "grounded", JUMP = "jump";

    //PHYSICS
    Rigidbody rb;
    Collider col;
    public float speed = 10;
    public float jumpForce = 5;

    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    [SyncVar] Vector2 vel = new Vector2();


    //space key buffer
    bool space, spaceConsumed = true;
    float spaceConsumeTimer;
    readonly float spaceConsumeMaxTime = 0.5f;

    //ground stuff
    //int groundCount = 0;
    bool grounded;


    //Model
    Animator anim;
    GameObject model;
    CinemachineVirtualCamera cam;
    CinemachineComposer fram;


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
        cam = gameObject.transform.Find("CM player").GetComponent<CinemachineVirtualCamera>();
        //fram = cam.AddCinemachineComponent<CinemachineFramingTransposer>();
        fram = cam.GetCinemachineComponent<CinemachineComposer>();

        if (!localPlayer())
        {
            gameObject.transform.Find("Main Camera").gameObject.SetActive(false);
            gameObject.transform.Find("CM player").gameObject.SetActive(false);
        }

        //if (isLocalPlayer)
        if (focused && localPlayer())
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
        if (focus && localPlayer())
        {
            GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.3f);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
        }
    }


    bool localPlayer()
    {
        return isLocalPlayer || LOCAL_DEBUG;
    }

    void Update()
    {
        //if (isLocalPlayer)
        if (focused && localPlayer())
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
        if (focused && localPlayer())
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

        if (focused && localPlayer())
        {
            Move();
            Rotate();
            Jump();
        }
    }


    public bool DEBUG_adjustToSlope;

    [Command]
    void UpdateVel(Vector2 newVel)
    {
        if (vel != newVel)
        {
            //adjust to terrain
            
            if(DEBUG_adjustToSlope) newVel = AdjustVelocityToSlope(newVel);
            //newVel.y += ySpeed;


            vel = newVel;
        }
    }


    //Not sure if it works
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray,out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
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

    public float isGroundedDist = .3f;
    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (col.bounds.extents.y + isGroundedDist), Color.red, 1 / 60);
        grounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + isGroundedDist);
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
    float deadzone = 0.6f;
    //public float vOffset = 0.5f, vUpThreshold = 0.725f, vDownThreshold = -0.25f;
    public float vOffset = 0.7f, vUpThreshold = 1f, vDownThreshold = 0f;
    void Rotate()
    {
        if (Mathf.Abs(inputArrows.x) > deadzone) //deadzone
        {
            transform.Rotate(new Vector3(0, inputArrows.x * rotationSpeed, 0));
        }

        if (Mathf.Abs(inputArrows.y) > deadzone) //deadzone
        {
            //vertical camera rotation
            fram.m_ScreenY =
                Mathf.Lerp(
                    Mathf.Clamp(
                                (inputArrows.y) + vOffset
                                , vDownThreshold, vUpThreshold)
                , vOffset, .5f)
                ; //aim does not exist, need to find property to acess aim>screen.y, which is by default 0.5
        }
        else
        {
            fram.m_ScreenY = Mathf.Lerp(fram.m_ScreenY, vOffset, .5f);
        }



    }

    //private void OnCollisionEnter(Collision c)
    //{
    //    var g = c.gameObject;
    //    switch (g.tag)
    //    {
    //        case "Ground":
    //            groundCount++;
    //            break;
    //    }
    //}

    //private void OnCollisionExit(Collision c)
    //{
    //    var g = c.gameObject;
    //    switch (g.tag)
    //    {
    //        case "Ground":
    //            groundCount--;
    //            break;
    //    }

    //}









    /////INPUT/////////
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
