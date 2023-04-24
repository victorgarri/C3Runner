using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerGrapplingHooks : MonoBehaviour
{
    //Physics
    public float speed = 10, jumpforce = 30, pullSpeed = 10, ropeLegnth = 20;
    Rigidbody2D rb;
    Vector2 hor = new Vector2(1, 0);

    //Grappling Mechanic
    Vector3 anchorInitPos = new Vector3(-1000, -1000, 0);
    public Transform pivot, aim, anchor;
    public LayerMask levelMask;
    public LineRenderer ln;

    SpringJoint2D sj;
    public Vector2 lastGroundPosition;
    public GameObject anchorSymbol, aimSymbol;


    //Anim
    public Animator anim;
    public SpriteRenderer sr;
    public Transform rotationParent;

    Quaternion rotationRightWall = Quaternion.Euler(0, 0, 90),
        rotationLeftWall = Quaternion.Euler(0, 0, -90),
        rotationRightCorner = Quaternion.Euler(0, 0, 45),
        rotationLeftCorner = Quaternion.Euler(0, 0, -45);


    //Others
    float Z;

    void Start()
    {
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        sj.enabled = false;
        Z = transform.position.z;
    }


    void Update()
    {
        GetInput();
        Actions();
        ActionsUnscaled();

        isGrounded();
        isFacingLeftWall();
        isFacingRightWall();
        jumpBuffer();
        shootBuffer();

    }

    void GetInput()
    {
        GetInputMovement();
        GetInputAim();
        GetInputJump();
        GetInputShoot();
    }

    void Actions()
    {
        Move();
        //Jump();
        Aim();
        //Shoot();
        Animate();
    }

    void ActionsUnscaled()
    {
        //Move();
        Jump();
        //Aim();
        Shoot();
        //Animate();
    }


    void Animate()
    {
        //When anchored, rotation is controlled automatically
        if (!anchored)
        {
            if (rb.velocity.x < 0)
            {
                sr.flipX = true;
            }
            else if (rb.velocity.x > 0)
            {
                sr.flipX = false;
            }

            //Rotation
            if (grounded)
            {
                if (nextToLeftWall && nextToRightWall)
                {
                    rotationParent.rotation = Quaternion.identity;
                }
                else if (nextToLeftWall)
                {
                    rotationParent.rotation = rotationLeftCorner;
                }
                else if (nextToRightWall)
                {
                    rotationParent.rotation = rotationRightCorner;
                }
                else
                {
                    rotationParent.rotation = Quaternion.identity;
                }
            }
            else
            {
                if (nextToLeftWall && nextToRightWall)
                {
                    rotationParent.rotation = Quaternion.identity;
                }
                else if (nextToLeftWall)
                {
                    rotationParent.rotation = rotationLeftWall;
                }
                else if (nextToRightWall)
                {
                    rotationParent.rotation = rotationRightWall;
                }
                else
                {
                    rotationParent.rotation = Quaternion.identity;
                }
            }

            //
        }
        else
        {
            if (anchorDir != Vector2.zero)
                rotationParent.up = (anchor.position - pivot.position);
        }



        anim.SetBool("hang", anchored);
        anim.SetFloat("velX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velY", (rb.velocity.y));
    }


    public float speedMidairModifier = 4;
    void Move()
    {

        //rb.velocity = inputHandlerMove * hor * speed;
        //rb.AddForce((inputHandlerMove * hor * speed * Time.deltaTime) / (grounded ? 1 : speedMidairModifier));
        float x = inputHandlerMove.x != 0 ? Mathf.Sign(inputHandlerMove.x) : 0;
        rb.AddForce((x * Vector2.right * hor * speed * Time.deltaTime) / (grounded ? 1 : speedMidairModifier));
    }

    void Jump()
    {
        if (!jumpConsumed && isGroundedOrFacingWall())
        {
            jumpConsumed = true;
            rb.velocity = new Vector3(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            anim.Play("Jump", -1, 0.0f);
        }

    }

    void Aim()
    {
        //if (!Input.GetKey(KeyCode.LeftShift))
        //if (!pi.actions["Shoot"].IsPressed())
        if (true)
        {
            Vector2 lookRot = DecideBetweenMoveAndAimBasedAiming();
            aim.transform.localPosition = lookRot;
            lookRot.Normalize();

            if (lookRot != Vector2.zero)
                aim.transform.up = lookRot;
        }
    }


    bool anchored;
    Vector3 anchorPoint;
    float shotDist = 0;
    Vector2 anchorDir = new Vector2();
    SpringJoint2D anchoredObj;
    void Shoot()
    {

        //if (Input.GetKeyDown(KeyCode.LeftShift) && inputHandlerAim != Vector2.zero)
        if (pi.actions["Shoot"].WasPressedThisFrame() && DecideBetweenMoveAndAimBasedAiming() != Vector2.zero)
        //if (!shootConsumed && DecideBetweenMoveAndAimBasedAiming() != Vector2.zero)
        {
            ln.enabled = true;

            anchorDir = aim.position - pivot.position;
            anchorDir.Normalize();


            ln.SetPosition(0, pivot.position);

            RaycastHit2D hitLevel = Physics2D.Raycast(aim.position, anchorDir, ropeLegnth, levelMask);

            if (hitLevel)
            {
                anchorPoint = hitLevel.point;
                //anchorPoint.z = Z;
                anchorPoint.z = transform.position.z;
                if (hitLevel.transform.gameObject.CompareTag("Pullable"))
                {
                    anchoredObj = AddSpringJointComponent(hitLevel.transform.gameObject);
                }

                anchored = true;
                sj.enabled = true;
                shotDist = Vector3.Distance(transform.position, anchorPoint);
                sj.distance = shotDist;

                anchor.parent = hitLevel.collider.transform;

                anim.Play("Hang", -1, 0.0f);
            }
            else
            {
                //anchorPoint = aim.position;
                anchorPoint = anchorInitPos;
                ln.enabled = false;
            }


            //z
            //anchorPoint.z = Z;
            anchorPoint.z = transform.position.z;


            ln.SetPosition(1, anchorPoint);
            anchor.position = anchorPoint;

            anchorSymbol.SetActive(true);
            anchorSymbol.transform.position = anchorPoint;

            Debug.DrawRay(aim.position, anchorDir * ropeLegnth, Color.magenta);



        }

        if (pi.actions["Shoot"].IsPressed() && anchored)
        {

            ln.SetPosition(1, anchor.position);
            ln.SetPosition(0, pivot.position);
            //decrease sj distance till 0
            sj.distance -= Time.deltaTime * pullSpeed;
        }

        if (pi.actions["Shoot"].WasReleasedThisFrame())
        {
            if (anchoredObj != null && anchoredObj.CompareTag("Pullable"))
            {
                RemoveSpringJointComponent(anchoredObj);
                anchoredObj = null;
            }

            sj.enabled = false;
            ln.enabled = false;
            anchored = false;
            anchor.parent = null;
            anchorSymbol.SetActive(false);
            ln.SetPosition(0, aim.position);
            ln.SetPosition(1, aim.position);

        }
    }

    //
    float groundedRayLength = .7f;
    bool grounded, nextToRightWall, nextToLeftWall;

    SpringJoint2D AddSpringJointComponent(GameObject obj)
    {
        SpringJoint2D sjobj = obj.AddComponent<SpringJoint2D>();
        sjobj.connectedBody = rb;
        sjobj.distance = 0;
        sjobj.breakForce = float.PositiveInfinity;
        sjobj.dampingRatio = 1;
        sjobj.frequency = 1;
        return sjobj;
    }

    void RemoveSpringJointComponent(SpringJoint2D obj)
    {
        Destroy(obj);
    }

    bool isGroundedOrFacingWall()
    {
        return
            grounded ||
            nextToLeftWall ||
            nextToRightWall;
    }

    void isFacingRightWall()
    {
        Debug.DrawRay(transform.position, Vector2.right * groundedRayLength, Color.red, 0.2f);
        nextToRightWall = Physics2D.Raycast(transform.position, Vector2.right, groundedRayLength, levelMask);
        //return nextToRightWall;
    }
    void isFacingLeftWall()
    {
        Debug.DrawRay(transform.position, Vector2.left * groundedRayLength, Color.red, 0.2f);
        nextToLeftWall = Physics2D.Raycast(transform.position, Vector2.left, groundedRayLength, levelMask);
        //return nextToLeftWall;
    }

    void isGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * groundedRayLength, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundedRayLength, levelMask);
        if (hit.point != Vector2.zero && hit.transform.CompareTag("Level")) lastGroundPosition = hit.point;
        grounded = hit;
        //return hit;
    }


    Vector2 DecideBetweenMoveAndAimBasedAiming()
    {
        if (inputHandlerAim == Vector2.zero)
        {
            return inputHandlerMove;
        }
        else
        {
            return inputHandlerAim;
        }
    }


    ////////////////////////inputhandlers

    /////INPUT/////////
    ///
    Vector2 inputHandlerMove = new Vector2();
    Vector2 inputHandlerAim = new Vector2();
    bool inputHandlerButtonSouth, inputHandlerButtonTrigger;
    PlayerInput pi;

    //jump buffer
    bool jumpConsumed = true;
    float jumpConsumeTimer;
    readonly float jumpConsumeMaxTime = 0.5f;

    void jumpBuffer()
    {
        //if pressed space on this frame, store so
        if (inputHandlerButtonSouth)
        {
            jumpConsumed = false;
        }

        //if after x time spaceConsumed is still false, set to true and reset timer
        if (!jumpConsumed)
        {
            jumpConsumeTimer += Time.deltaTime;
            if (jumpConsumeTimer >= jumpConsumeMaxTime)
            {
                jumpConsumed = true; //consume regardless
                jumpConsumeTimer = 0;
            }
        }
    }


    //shoot buffer
    bool shootConsumed = true;
    float shootConsumeTimer;
    readonly float shootConsumeMaxTime = 0.5f;

    void shootBuffer()
    {
        //if pressed space on this frame, store so
        if (inputHandlerButtonTrigger)
        {
            shootConsumed = false;
        }

        //if after x time spaceConsumed is still false, set to true and reset timer
        if (!shootConsumed)
        {
            shootConsumeTimer += Time.deltaTime;
            if (shootConsumeTimer >= shootConsumeMaxTime)
            {
                shootConsumed = true; //consume regardless
                shootConsumeTimer = 0;
            }
        }
    }

    public Vector2 GetInputMovement()
    {
        //reset input
        inputHandlerMove = Vector2.zero;
        inputHandlerMove = pi.actions["Movement"].ReadValue<Vector2>();

        return inputHandlerMove;
    }

    public Vector2 GetInputAim()
    {
        //reset input
        inputHandlerAim = Vector2.zero;
        inputHandlerAim = pi.actions["Camera"].ReadValue<Vector2>().normalized;

        return inputHandlerAim;
    }

    public bool GetInputJump()
    {
        //reset input
        inputHandlerButtonSouth = false;
        inputHandlerButtonSouth = pi.actions["Jump"].WasPressedThisFrame();

        return inputHandlerButtonSouth;
    }

    public bool GetInputShoot()
    {
        //reset input
        inputHandlerButtonTrigger = false;
        inputHandlerButtonTrigger = pi.actions["Shoot"].WasPressedThisFrame();

        return inputHandlerButtonTrigger;
    }
}
