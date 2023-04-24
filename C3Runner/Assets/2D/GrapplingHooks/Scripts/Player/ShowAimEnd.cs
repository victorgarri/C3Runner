using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowAimEnd : MonoBehaviour
{
    public GameObject aimIcon;
    Transform start;
    Vector3 newPos;
    PlayerInput pi;

    public float ropeLegnth = 20;
    public LayerMask levelMask;

    void Start()
    {
        start = transform;
        pi = GetComponent<PlayerInput>();
    }

    RaycastHit2D hitLevel;
    void Update()
    {
        hitLevel = Physics2D.Raycast(start.position, DecideBetweenMoveAndAimBasedAiming(), ropeLegnth, levelMask);
        if (hitLevel)
        {


            aimIcon.transform.position = hitLevel.point;
            newPos = aimIcon.transform.localPosition;
            newPos.z = 0;
            aimIcon.transform.localPosition = newPos;

            aimIcon.SetActive(true);
        }
        else
        {
            aimIcon.SetActive(false);
        }
    }


    Vector2 DecideBetweenMoveAndAimBasedAiming()
    {
        if (GetInputAim() == Vector2.zero)
        {
            return GetInputMovement();
        }
        else
        {
            return inputHandlerAim;
        }
    }

    Vector2 inputHandlerMove = new Vector2();

    public Vector2 GetInputMovement()
    {
        //reset input
        inputHandlerMove = Vector2.zero;
        inputHandlerMove = pi.actions["Movement"].ReadValue<Vector2>();

        return inputHandlerMove;
    }


    Vector2 inputHandlerAim = new Vector2();
    public Vector2 GetInputAim()
    {
        //reset input
        inputHandlerAim = Vector2.zero;
        inputHandlerAim = pi.actions["Camera"].ReadValue<Vector2>().normalized;

        return inputHandlerAim;
    }
}
