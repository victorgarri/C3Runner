using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StuckHelper : MonoBehaviour
{
    public PlayerInput pi;
    public Player3D player;




    void Update()
    {
        if (pi.actions["HelpUp"].WasPressedThisFrame())
        {
            transform.position += Vector3.up * 5;
        }
        if (pi.actions["HelpLeft"].WasPressedThisFrame())
        {
            transform.position += Vector3.left * 5;
        }
        if (pi.actions["HelpRight"].WasPressedThisFrame())
        {
            transform.position += Vector3.right * 5;
        }
        if (pi.actions["HelpForward"].WasPressedThisFrame())
        {
            transform.position += Vector3.forward * 5;
        }
        if (pi.actions["HelpBackwards"].WasPressedThisFrame())
        {
            transform.position += Vector3.back * 5;
        }

        if (pi.actions["HelpRespawn"].WasPressedThisFrame())
        {
            transform.position = player.lastGroundPosition;
        }


    }




}
