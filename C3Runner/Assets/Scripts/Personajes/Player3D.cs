using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player3D : NetworkBehaviour
{
    public bool LOCAL_DEBUG;

    //anim vars
    string VEL = "vel", VELY = "vely", GROUNDED = "grounded", JUMP = "jump";

    //PHYSICS
    Rigidbody rb;
    Collider col;
    public float speed = 10;
    public float jumpForce = 5;

    //Input
    public PlayerInput pi;
    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    [SyncVar] Vector2 vel = new Vector2();

    //space key buffer
    bool space, spaceConsumed = true;
    float spaceConsumeTimer;
    readonly float spaceConsumeMaxTime = 0.5f;

    //ground stuff
    bool grounded;

    //Model
    Animator anim;
    GameObject model;
    CinemachineVirtualCamera cam;
    CinemachineComposer fram;

    //Networking
    public bool _isLocalPlayer; //para que otros scripts lo puedan referenciar


    public AudioClip jumpSound;
    private AudioSource audioSource;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        speed *= GameManager.gravityScale;
        jumpForce *= GameManager.gravityScale;

        audioSource = GetComponent<AudioSource>();
        

        model = transform.Find("Character").gameObject;
        anim = model.GetComponent<Animator>();
        cam = gameObject.transform.Find("CM player").GetComponent<CinemachineVirtualCamera>();
        fram = cam.GetCinemachineComponent<CinemachineComposer>();
        
        _isLocalPlayer = localPlayer();

        if (!localPlayer())
        {
            gameObject.transform.Find("Main Camera").gameObject.SetActive(false);
            gameObject.transform.Find("CM player").gameObject.SetActive(false);
        }

        if (focused && localPlayer())
        {
            pi = GetComponent<PlayerInput>();

            GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.3f);

        }

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
        if (focused && localPlayer())
        {
            UpdateVel(inputWASD * speed);

            if (!grounded)
            {
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

            if (DEBUG_adjustToSlope) newVel = AdjustVelocityToSlope(newVel);


            vel = newVel;
        }
    }


    //Not sure if it works
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
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
            audioSource.PlayOneShot(jumpSound, 1);
        }
    }

    float rotationSpeed = 2;
    float deadzone = 0.6f;
    public float vOffset = 0.7f, vUpThreshold = 1f, vDownThreshold = 0f;
    public float damping = .3f;
    void Rotate()
    {
        if (Mathf.Abs(inputArrows.x) > deadzone) //deadzone
        {
            transform.Rotate(new Vector3(0, inputArrows.x * rotationSpeed, 0));
        }

        if (Mathf.Abs(inputArrows.y) > deadzone) //deadzone
        {
            //vertical camera rotation
            var newscreenY = Mathf.Clamp((inputArrows.y) + vOffset, vDownThreshold, vUpThreshold);

            fram.m_ScreenY = Mathf.Lerp(fram.m_ScreenY, newscreenY, Mathf.SmoothStep(0, 1, damping));
        }
        else
        {
            fram.m_ScreenY = Mathf.Lerp(fram.m_ScreenY, vOffset, Mathf.SmoothStep(0, 1, damping));
        }



    }

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
