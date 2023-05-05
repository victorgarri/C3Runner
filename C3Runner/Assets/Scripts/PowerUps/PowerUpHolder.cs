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

        if (powerUp != null && inputHandlerButtonTrigger)
        {
            ActivatePowerUp();
        }
    }


    public void GetPowerUp(GameObject pu)
    {
        powerUp = pu;

        //activate UI
    }


    void ActivatePowerUp()
    {
        try
        {
            ///...
            if (powerUp.GetComponent<Shell>() != null)
            {
                var dir = localplayer.model.transform.forward;
                Instantiate(powerUp, transform.position + localplayer.model.transform.forward * 2, transform.rotation, null); //REPLACE WITH NETCODE
            }

        }
        catch (System.Exception) { }
        finally
        {
            powerUp = null;
            //deactivate UI
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
