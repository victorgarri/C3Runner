using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Lives & Invulnerability")] public int numberOfLives = 3;
    public bool invulnerable = false;
    public float invulnerabilityPeriod = 2;
    public GameObject livesUI;
    public GameObject lifeUnit;

    [Header("Forces")] public float forceMove = 1000f;
    public float forceJump = 750f;
    public float forceKnockback = 10;

    //Components
    private Animator anim;
    private SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D bc;

    
    [Header("Inputs")] public bool jumping = false;

    public int collisionCount = 0;

    //
    float hInput, vInput;
    public bool isInControl = true;
    public Vector2 outOfControl;

    [Header("Input Buffer")]
    //simple input buffer for jumping
    public float bufferTime = 0.1f;

    bool jumpConsumed = true;

    [Header("Others")] public Sprite winSprite;


    private PlayerInput pi;

    void Start()
    {
        pi = GetComponent<PlayerInput>();

        Camera cam = Camera.main;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();

        for (int i = 0; i < numberOfLives; i++)
        {
            var obj = Instantiate(lifeUnit, livesUI.transform);
        }
    }

    void RemoveLife()
    {
        livesUI.transform.GetChild(numberOfLives - 1).gameObject.SetActive(false);
    }

    public void AddLife()
    {
        //Si el n�mero de vidas actual es menor que las vidas totales, activamos el �ltimo elemento desactivado
        if (numberOfLives < livesUI.transform.childCount)
        {
            numberOfLives++;
            livesUI.transform.GetChild(numberOfLives - 1).gameObject.SetActive(true);
        }
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    AddLife();
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    takeDamage(Vector2.right);
        //}


        //
        if (isInControl)
        {
            // hInput = Input.GetAxisRaw("Horizontal");
            // vInput = Input.GetAxisRaw("Vertical");
            hInput = GetInputMovement().normalized.x;
            vInput = Mathf.Abs(GetInputMovement().y) >= 0.7f ? GetInputMovement().y : 0;
            //vInput = GetInputMovement().y;


            //updatePhysics();
            if (GetInputButtonSouth())
            {
                //jumpConsumed = false;
                StartCoroutine("jumpBuffer");
            }


            //ANIMS

            //face left or right
            if (hInput < 0)
            {
                sr.flipX = true;
            }
            else if (hInput > 0)
            {
                sr.flipX = false;
            }

            //is on ground or is jumping/falling
            if (isGrounded())
            {
                jumping = false;
            }
            else if (Mathf.Sign(vInput) != -1)
            {
                jumping = true;
            }

            //set animator vars
            anim.SetBool("jumping", jumping);
            anim.SetFloat("velX", Mathf.Abs(rb.velocity.x / 5));
            anim.SetFloat("velY", Mathf.Abs(rb.velocity.y));
            anim.SetFloat("inputY", vInput);
        }
        else
        {
            //
            transform.position = (Vector2)transform.position + outOfControl;
        }
    }

    private void FixedUpdate()
    {
        updatePhysics();
    }

    void updatePhysics()
    {
        if (isInControl)
        {
            //move if not crouching
            if (!(vInput < 0))
                rb.AddForce(hInput * forceMove * Vector2.right * Time.deltaTime, ForceMode2D.Force);


            //jump
            if (!jumpConsumed && isGrounded())
            {
                jumpConsumed = true;
                jump();
            }
        }
    }

    public void jump()
    {
        rb.AddForce(forceJump * Vector2.up);
    }

    public void takeDamage(Vector2 dir)
    {
        if (!invulnerable)
        {
            RemoveLife();
            numberOfLives--;

            //anim.SetTrigger("tookDamage");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(Mathf.Sign(-dir.x) * 2, 2) * forceKnockback, ForceMode2D.Impulse);

            StartCoroutine("invulnerability");
        }

        if (numberOfLives <= 0)
        {
            isInControl = false;
            GameObject.Find("GameManager").GetComponent<Level2DGameManager>().GameOver();
        }
    }

    IEnumerator invulnerability()
    {
        invulnerable = true;
        InvokeRepeating("flashSprite", 0, 0.1f);
        yield return new WaitForSeconds(invulnerabilityPeriod);
        CancelInvoke("flashSprite");
        sr.enabled = true;
        invulnerable = false;
    }

    IEnumerator jumpBuffer()
    {
        jumpConsumed = false;
        yield return new WaitForSeconds(bufferTime);
        jumpConsumed = true;
    }

    void flashSprite()
    {
        sr.enabled = !sr.enabled;
    }

    private bool isGrounded()
    {
        return collisionCount > 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            //if above
            //if (bc.bounds.min.y >= col.gameObject.GetComponent<Collider2D>().bounds.max.y)
            collisionCount++;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            collisionCount--;
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
        //inputHandlerWASD = pi.actions["Movement"].ReadValue<Vector2>().normalized;
        inputHandlerWASD = pi.actions["Movement"].ReadValue<Vector2>();

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