using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    string VEL = "vel", VELY = "vely", GROUNDED = "grounded", JUMP = "jump";

    //PHYSICS
    Rigidbody rb;
    Collider col;
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

    private float PosX;
    private float PosY;
    private float PosZ;
    private Vector3 Posicion;
    
    public float InicialX;
    public float InicialY;
    public float InicialZ;
    private bool isPlayerEnter;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        speed *= GameManager.gravityScale;
        jumpForce *= GameManager.gravityScale;

        model = transform.Find("Character").gameObject;
        anim = model.GetComponent<Animator>();
        
        ResetearPosicion();

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

        if (isPlayerEnter)
        {
            GuardarPosicion();
        }
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

    private void OnTriggerEnter(Collider c)
    {
        var g = c.gameObject;
        switch (g.tag)
        {
            case "CambioEscena":
                isPlayerEnter = true;
                Debug.Log("funciona");
                break;
        }
    }
    
    public void GuardarPosicion()
    {
        PlayerPrefs.SetFloat("PosicionX", transform.position.x+10);
        PlayerPrefs.SetFloat("PosicionY", transform.position.y);
        PlayerPrefs.SetFloat("PosicionZ", transform.position.z);
    }

    public void CargarPosicion()
    {
        PosX = PlayerPrefs.GetFloat("PosicionX");
        PosY = PlayerPrefs.GetFloat("PosicionY");
        PosZ = PlayerPrefs.GetFloat("PosicionZ");
        
        Posicion.x = PosX;
        Posicion.y = PosY;
        Posicion.z = PosZ;

        this.transform.position = Posicion;
    }
    
    private void ResetearPosicion()
    {
        CargarPosicion();
        
        PlayerPrefs.SetFloat("PosicionX", InicialX);
        PlayerPrefs.SetFloat("PosicionY", InicialY);
        PlayerPrefs.SetFloat("PosicionZ", InicialZ);
    }
}
