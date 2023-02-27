using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static float gravityScale = 3;
    public bool DEBUG;
    //private static Vector2 inputWASD = new Vector2();
    //private static Vector2 inputArrows = new Vector2();
    //private static bool buttonSouth;
    //static PlayerInput pi;

    private void Update()
    {
        if (DEBUG)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                
            }
        }
    }

    private void Start()
    {
        if (isClient)
        {
            Physics.gravity = Physics.gravity * gravityScale;
            //pi = GetComponent<PlayerInput>();
        }
    }

    //public static Vector2 GetInputMovement()
    //{
    //    //reset input
    //    inputWASD = Vector2.zero;
    //    inputWASD = pi.actions["Movement"].ReadValue<Vector2>().normalized;

    //    return inputWASD;
    //}

    //public static Vector2 GetInputCamera()
    //{
    //    //reset input
    //    inputArrows = Vector2.zero;
    //    inputArrows = pi.actions["Camera"].ReadValue<Vector2>().normalized;

    //    return inputArrows;
    //}

    //public static bool GetInputButtonSouth()
    //{
    //    //reset input
    //    buttonSouth = false;
    //    buttonSouth = pi.actions["Jump"].WasPressedThisFrame();

    //    return buttonSouth;
    //}
}
