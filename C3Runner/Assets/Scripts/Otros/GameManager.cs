using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static float gravityScale = 3;
    private static Vector3 initGravityScale;
    public bool DEBUG;


    private void Start()
    {
        //if (initGravityScale == Vector3.zero)
        //{
        //    initGravityScale = Physics.gravity; //save it
        //}

        //Physics.gravity = initGravityScale;

        //if (isClient)
        //{
        //    Physics.gravity = Physics.gravity * gravityScale;
        //}
    }

}
