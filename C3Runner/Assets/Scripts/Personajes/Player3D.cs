using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player3D : NetworkBehaviour
{
    #region VARIABLES

    [Header("Custom Player Data")]
    [SyncVar] public string playerName;
    [SyncVar(hook = nameof(SetColor))] public Color playerColor;
    [SyncVar(hook = nameof(SetPlayerType))] public float playerType;


    void SetColor(Color _, Color newColor)
    {
        var charr = transform.Find("Character").Find("CharType1");

        foreach (Material material in charr.GetComponent<Renderer>().materials)
        {
            if (material.name.Contains("Shirt"))
            {
                material.SetColor("_Color", newColor);
            }
        }
    }

    void SetPlayerType(float _, float newType)
    {
        //when it's 0 this method is never executed FOR SOME REASON
        var charr = transform.Find("Character").Find("CharType1");
        SkinnedMeshRenderer smr = charr.GetComponent<SkinnedMeshRenderer>();
        smr.SetBlendShapeWeight(0, newType);

        switch (newType)
        {
            case 0: hairType1.SetActive(true); break;
            case 100: hairType1.SetActive(false); hairType2.SetActive(true); break;
        }

    }

    //PHYSICS
    [Header("Physics")]
    public float speed = 60;
    public float jumpForce = 17;
    [SyncVar] public Vector2 vel = new Vector2();
    Rigidbody rb;
    Collider col;

    //Input
    [Header("Input")]
    public PlayerInput pi;
    Vector2 inputWASD = new Vector2();
    Vector2 inputArrows = new Vector2();
    bool inControl = true;

    //Model&Camera
    [Header("Model&Camera")]
    public GameObject model;
    public GameObject hairType1;
    public GameObject hairType2;
    Animator anim;
    string VEL = "vel"; string VELY = "vely"; string GROUNDED = "grounded"; string JUMP = "jump";
    CinemachineVirtualCamera cam;
    CinemachineComposer fram;


    //Audio
    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip runSound;
    private AudioSource audioSource;

    //Canvas UI
    [Header("Canvas")]
    public Text spotText;

    //FX&PowerUps
    [Header("FX&PowerUps")]
    public GameObject stunStars;
    public GameObject powerUpRebote;
    public GameObject powerUpInvulnerabilidad;
    public GameObject powerUpSpeedUp;
    public GameObject powerUpOVNI;

    [SyncVar] public bool invulnerable = false;
    [SyncVar] public bool bounceOtherPlayers = false;
    [SyncVar] public bool speedUp = false;
    [SyncVar] public bool ovni = false;

    //Others
    [Header("Info&Others")]
    public Vector3 lastGroundPosition;
    public int spot = 1;
    public float distanceFromZero;
    Vector3 zero;
    [SyncVar] public bool in2DGame;
    CapFramerate capFramerate;

    #endregion

    #region POWERUP NETCODE
    [Command]
    public void SpeedUpToggle()
    {
        SpeedUpToggleClient();
    }

    [ClientRpc]
    public void SpeedUpToggleClient()
    {
        speedUp = !speedUp;
        powerUpSpeedUp.SetActive(speedUp);
    }

    [Command]
    public void InvulnerabilityToggle()
    {
        InvulnerabilityToggleClient();
    }

    [ClientRpc]
    public void InvulnerabilityToggleClient()
    {
        invulnerable = !invulnerable;
        powerUpInvulnerabilidad.SetActive(invulnerable);
    }


    [Command]
    public void BounceOtherPlayersToggle()
    {
        BounceOtherPlayersToggleClient();
    }

    [ClientRpc]
    public void BounceOtherPlayersToggleClient()
    {
        bounceOtherPlayers = !bounceOtherPlayers;
        powerUpRebote.SetActive(bounceOtherPlayers);
    }

    [Command]
    public void OVNIToggle()
    {
        OVNIToggleClient();
    }

    [ClientRpc]
    public void OVNIToggleClient()
    {
        ovni = !ovni;
        GetComponent<OVNIplayer>().enabled = ovni;
        powerUpOVNI.SetActive(ovni);
    }

    [ClientRpc]
    public void GetStunned()
    {
        if (!invulnerable)
            StartCoroutine("Stunned");
    }

    IEnumerator Stunned()
    {
        inControl = false;
        stunStars.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        inControl = true;
        stunStars.SetActive(false);

    }

    #endregion

    #region START, UPDATE, FIXED UPDATE

    void Start()
    {
        zero = GameObject.Find("Zero").transform.position;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        audioSource = GetComponent<AudioSource>();

        model = transform.Find("Character").gameObject;
        anim = model.GetComponent<Animator>();

        cam = gameObject.transform.Find("CM player").GetComponent<CinemachineVirtualCamera>();

        fram = cam.GetCinemachineComponent<CinemachineComposer>();

        capFramerate = GameObject.Find("CapFramerate").GetComponent<CapFramerate>();

        if (!localPlayer())
        {
            DisableFeatures();
        }

        if (focused && localPlayer())
        {
#if (UNITY_ANDROID || UNITY_EDITOR)
            cam.m_Lens.FarClipPlane = capFramerate.renderDistance;
#endif
            pi = GetComponent<PlayerInput>();
            GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.3f);
        }
    }

    void Update()
    {

        if (inControl)
        {
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
    }

    void FixedUpdate()
    {
        UpdateDistanceFromZero();

        if (inControl)
        {
            if (focused && localPlayer())
            {
                vel = inputWASD * speed;

                if (!grounded)
                {
                    vel = vel / 2;
                }
                UpdateVel(vel);
            }
        }

        Animation();

        if (inControl)
        {
            if (focused && localPlayer())
            {
                Move();
                Rotate();
                Jump();
            }
        }
    }
    #endregion

    #region ENABLE & DISABLE

    public void DisableFeatures()
    {
        gameObject.GetComponent<AudioSource>().enabled = false;
        inControl = false;

#if (UNITY_ANDROID || UNITY_EDITOR)
        gameObject.GetComponent<PlayerInput>().enabled = false;
#endif
        gameObject.transform.Find("Main Camera").gameObject.SetActive(false);
        gameObject.transform.Find("CM player").gameObject.SetActive(false);
        gameObject.transform.Find("Canvas").gameObject.SetActive(false);
        gameObject.transform.Find("PlayerSpot").gameObject.SetActive(false);
        gameObject.transform.Find("PowerUpCanvas").gameObject.SetActive(false);
    }

    public void EnableFeatures()
    {
        gameObject.GetComponent<AudioSource>().enabled = true;
        inControl = true;
#if (UNITY_ANDROID || UNITY_EDITOR)
        gameObject.GetComponent<PlayerInput>().enabled = true;
#endif
        gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
        gameObject.transform.Find("CM player").gameObject.SetActive(true);
        gameObject.transform.Find("Canvas").gameObject.SetActive(true);
        gameObject.transform.Find("PlayerSpot").gameObject.SetActive(true);
        gameObject.transform.Find("PowerUpCanvas").gameObject.SetActive(true);
    }

    public void DisableRB()
    {
        //col.enabled = false;
        //rb.useGravity = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void EnableRB()
    {
        //col.enabled = true;
        //rb.useGravity = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }

    public void DisableMeshRenderer()
    {
        transform.Find("Character").gameObject.SetActive(false);
    }

    public void EnableMeshRenderer()
    {
        transform.Find("Character").gameObject.SetActive(true);
    }

    [ClientRpc]
    public void DisableControls()
    {
        //DisableControlsClient();
        if (pi != null && pi.enabled)
            pi.enabled = false;
    }

    #endregion

    #region INFO

    public bool focused = true;
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
        return isLocalPlayer;
    }

    void UpdateDistanceFromZero()
    {
        distanceFromZero = Vector3.Distance(transform.position, zero);
    }

    public void updateSpotUI()
    {
        if (localPlayer())
            spotText.text = spot + "º";
    }

    //ground stuff
    bool grounded;
    public float isGroundedDist = .55f;

    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (col.bounds.extents.y + isGroundedDist), Color.red, 1 / 60);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, col.bounds.extents.y + isGroundedDist);


        if (hit.point != Vector3.zero && hit.transform.CompareTag("Ground")) lastGroundPosition = hit.point;

        grounded = (hit.collider != null);

        return grounded;
    }

    [Command]
    public void Update2DStatus(bool in2D)
    {
        if (in2DGame != in2D)
        {
            in2DGame = in2D;
        }
    }
    #endregion

    #region ACTIONS
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


    private Vector3 hor = new Vector3(1, 0, 1);

    void Move()
    {
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        rb.AddForce(forward * vel.y + right * vel.x);

        hor = rb.velocity;
        hor.y = 0;
        if (isGrounded() && inputWASD.magnitude > 0.5f && !audioSource.isPlaying)
        {
            audioSource.clip = runSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
        }

    }

    //space key buffer
    bool space, spaceConsumed = true;
    float spaceConsumeTimer;
    readonly float spaceConsumeMaxTime = 0.5f;

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

    //public bool DEBUG_adjustToSlope;
    [Command]
    void UpdateVel(Vector2 newVel)
    {
        if (vel != newVel)
        {
            //adjust to terrain
            //if (DEBUG_adjustToSlope) newVel = AdjustVelocityToSlope(newVel);
            vel = newVel;
        }
    }

    #endregion

    #region CAMERA_ROTATION

    float rotationSpeed = 2;
    float deadzone = 0.6f;
    public float vOffset = 0.7f, vUpThreshold = 1f, vDownThreshold = .5f;
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
    #endregion

    #region INPUT

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
    #endregion

    void OnCollisionEnter(Collision col)
    {
        //if another player with powerup active touches localplayer, bounce this player back
        var obj = col.gameObject;
        if (obj.CompareTag("Player") && obj.GetComponent<Player3D>().bounceOtherPlayers)
        {
            Vector3 dir = col.contacts[0].normal;
            dir.y = .4f;
            rb.AddForce(dir * 40, ForceMode.Impulse);
        }
    }

}