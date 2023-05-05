using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpHolder : MonoBehaviour
{
    public GameObject powerUp;
    public Player3D localplayer;
    PlayerInput pi;

    void Start()
    {
        if (localplayer == null)
        {
            localplayer = GetComponent<Player3D>();
        }

        pi = GetComponent<PlayerInput>();
    }


    void Update()
    {
        GetInputButtonTrigger();

        if (powerUp != null)
        {


        }
    }


    public void GetPowerUp(GameObject pu)
    {
        powerUp = pu;
    }


    void ActivatePowerUp()
    {
        try
        {
            ///...
            if (powerUp.GetComponent<Shell>() != null)
            {

            }

        }
        catch (System.Exception){}
        finally
        {
            powerUp = null;
        }


    }



    bool inputHandlerButtonTrigger = false;

    public bool GetInputButtonTrigger()
    {
        //reset input
        inputHandlerButtonTrigger = false;
        inputHandlerButtonTrigger = pi.actions["ActivatePowerUp"].WasPressedThisFrame();

        return inputHandlerButtonTrigger;
    }
}
