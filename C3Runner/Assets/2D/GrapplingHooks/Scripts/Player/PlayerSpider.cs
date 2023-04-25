using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerSpider : MonoBehaviour
{
    //Physics
    [Header("Physics")]
    public float speed = 1500;
    public float jumpforce = 30;
    public float pullSpeed = 10;
    public float ropeLegnth = 20;
    public float speedMidairModifier = 3;
    Rigidbody2D rb;
    BoxCollider2D col; Vector3 offsetL, offsetR;
    SpringJoint2D sj;
    Vector2 hor = new Vector2(1, 0);
    Vector2 ver = new Vector2(0, 1);
    bool jumping;

    //Grappling Mechanic
    [Header("Grappling")]
    public LayerMask levelMask;
    public Transform pivot, aim, anchor;
    public LineRenderer ln;
    Vector3 anchorInitPos = new Vector3(-1000, -1000, 0);

    //Anim
    [Header("Visual")]
    public Animator anim;
    public SpriteRenderer sr;
    public Transform rotationParent;
    public GameObject anchorSymbol, aimSymbol;
    Quaternion rotationRightWall = Quaternion.Euler(0, 0, 90),
        rotationLeftWall = Quaternion.Euler(0, 0, -90),
        rotationRightCorner = Quaternion.Euler(0, 0, 45),
        rotationLeftCorner = Quaternion.Euler(0, 0, -45);

    //Audio
    [Header("Audio")]
    public AudioClip audclipSwish;
    public AudioClip audclipSquish;
    public AudioClip audclipJump;
    public AudioClip audclipLand;
    AudioSource aud;

    //Others
    [Header("Info")]
    public Vector2 lastGroundPosition;

    void Start()
    {
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        offsetL = new Vector3(-col.bounds.extents.x, 0, 0);
        offsetR = new Vector3(col.bounds.extents.x, 0, 0);

        aud = GetComponent<AudioSource>();

        sj = GetComponent<SpringJoint2D>();
        sj.enabled = false;
    }

    void Update()
    {
        GetInput();
        Actions();
        ActionsUnscaled();
        //
        isGrounded();
        isFacingLeftWall();
        isFacingRightWall();
        jumpBuffer();
        shootBuffer();

    }


    #region Visual

    private void Animate()
    {
        //When anchored, rotation is controlled automatically
        if (!anchored)
        {
            //Direction
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

    #endregion

    #region Actions

    private void Actions()
    {
        Move();
        Aim();
        Animate();
    }

    private void ActionsUnscaled()
    {
        Jump();
        Shoot();
    }

    private void Move()
    {
        float x = inputHandlerMove.x != 0 ? Mathf.Sign(inputHandlerMove.x) : 0;
        rb.AddForce((x * Vector2.right * hor * speed * Time.deltaTime) / (grounded ? 1 : speedMidairModifier));

        Climbing();
    }

    private void Climbing()
    {
        float y = inputHandlerMove.y != 0 ? Mathf.Sign(inputHandlerMove.y) : 0;
        if (isFacingWall() && !anchored && !jumping)
        {
            rb.gravityScale = 0;
            //able to move up or down
            if (y != 0)
            {
                rb.AddForce((y * Vector2.up * ver * speed * Time.deltaTime));
            }
            else
            {
                //decelerate
                rb.velocity = rb.velocity * 0.5f * Time.deltaTime;
            }
        }
        else
        {
            rb.gravityScale = 9.8f;

        }
    }

    private void Jump()
    {
        if (!jumpConsumed && isGroundedOrFacingWall())
        {
            jumpConsumed = true;
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0);
            rb.AddForce((Vector2.up * jumpforce) / (isFacingWall() ? speedMidairModifier : 1), ForceMode2D.Impulse);
            anim.Play("Jump", -1, 0.0f);
            aud.PlayOneShot(audclipJump);
            StartCoroutine("JumpingBool");
        }

    }

    IEnumerator JumpingBool()
    {
        jumping = true;
        yield return new WaitForSeconds(0.2f);
        jumping = false;
    }

    private void Aim()
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
    private void Shoot()
    {
        if (pi.actions["Shoot"].WasPressedThisFrame() && DecideBetweenMoveAndAimBasedAiming() != Vector2.zero)
        {
            ln.enabled = true;

            anchorDir = aim.position - pivot.position;
            anchorDir.Normalize();

            ln.SetPosition(0, pivot.position);

            RaycastHit2D hitLevel = Physics2D.Raycast(aim.position, anchorDir, ropeLegnth, levelMask);

            if (hitLevel)
            {
                anchorPoint = hitLevel.point;
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
                //play swish sound
                aud.PlayOneShot(audclipSwish);
            }
            else
            {
                anchorPoint = anchorInitPos;
                ln.enabled = false;
            }

            //z
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

    #endregion

    #region Ground and wall checking, springjoint
    //Ground and wall checking, springjoint
    float groundedRayLength = .7f;
    bool grounded, nextToRightWall, nextToLeftWall;

    private SpringJoint2D AddSpringJointComponent(GameObject obj)
    {
        SpringJoint2D sjobj = obj.AddComponent<SpringJoint2D>();
        sjobj.connectedBody = rb;
        sjobj.distance = 0;
        sjobj.breakForce = float.PositiveInfinity;
        sjobj.dampingRatio = 1;
        sjobj.frequency = 1;
        return sjobj;
    }

    private void RemoveSpringJointComponent(SpringJoint2D obj)
    {
        Destroy(obj);
    }

    private bool isGroundedOrFacingWall()
    {
        return grounded || isFacingWall();
    }

    private bool isFacingWall()
    {
        return nextToLeftWall ||
            nextToRightWall;
    }

    private void isFacingRightWall()
    {
        Debug.DrawRay(transform.position, Vector2.right * groundedRayLength, Color.red, 0.2f);
        nextToRightWall = Physics2D.Raycast(transform.position, Vector2.right, groundedRayLength, levelMask);
    }
    private void isFacingLeftWall()
    {
        Debug.DrawRay(transform.position, Vector2.left * groundedRayLength, Color.red, 0.2f);
        nextToLeftWall = Physics2D.Raycast(transform.position, Vector2.left, groundedRayLength, levelMask);
    }


    private void isGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * groundedRayLength, Color.red, 0.2f);
        Debug.DrawRay(transform.position + offsetL, Vector2.down * groundedRayLength, Color.red, 0.2f);
        Debug.DrawRay(transform.position + offsetR, Vector2.down * groundedRayLength, Color.red, 0.2f);
        RaycastHit2D hitC = Physics2D.Raycast(transform.position, Vector2.down, groundedRayLength, levelMask);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position + offsetL, Vector2.down, groundedRayLength, levelMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + offsetR, Vector2.down, groundedRayLength, levelMask);

        bool hit = hitC || hitL || hitR;

        if (hitC.point != Vector2.zero && hitC.transform.CompareTag("Level"))
        {
            lastGroundPosition = hitC.point;
        }

        grounded = hit;
    }

    /*private void isGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * groundedRayLength, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundedRayLength, levelMask);
        if (hit.point != Vector2.zero && hit.transform.CompareTag("Level")) lastGroundPosition = hit.point;
        grounded = hit;
    }*/


    private Vector2 DecideBetweenMoveAndAimBasedAiming()
    {
        return inputHandlerAim == Vector2.zero ? inputHandlerMove : inputHandlerAim;
    }

    #endregion

    #region Input

    private void GetInput()
    {
        GetInputMovement();
        GetInputAim();
        GetInputJump();
        GetInputShoot();
    }

    /////INPUT/////////
    Vector2 inputHandlerMove = new Vector2();
    Vector2 inputHandlerAim = new Vector2();
    bool inputHandlerButtonSouth, inputHandlerButtonTrigger;
    PlayerInput pi;

    //jump buffer
    bool jumpConsumed = true;
    float jumpConsumeTimer;
    readonly float jumpConsumeMaxTime = 0.5f;

    private void jumpBuffer()
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

    private void shootBuffer()
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
    #endregion

    #region Others

    //Audio
    public void PlaySquishSound()
    {
        aud.PlayOneShot(audclipSquish);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.layer == levelMask)
        if (levelMask == (levelMask | (1 << col.gameObject.layer)))
        {
            aud.PlayOneShot(audclipLand);
        }
    }

    #endregion
}
