using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpHolder : NetworkBehaviour
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
        if (isLocalPlayer && localplayer.focused)
        {
            GetInputButtonTrigger();

            if (powerUp != null && inputHandlerButtonTrigger)
            {
                ActivatePowerUp();
            }
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
                //Instantiate(powerUp, transform.position + localplayer.model.transform.forward * 2, transform.rotation, null); //REPLACE WITH NETCODE
                ShellThrow();
            }

            if (powerUp.GetComponent<SpeedUp>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                p.GetComponent<SpeedUp>().Activate(localplayer);

            }

            if (powerUp.GetComponent<Invulnerability>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                p.GetComponent<Invulnerability>().Activate(localplayer);

            }

            if (powerUp.GetComponent<BounceOtherPlayers>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                p.GetComponent<BounceOtherPlayers>().Activate(localplayer);

            }

        }
        catch (System.Exception e)
        {
            print(e);
        }
        finally
        {
            powerUp = null;
            //deactivate UI
        }


    }

    [Command]
    void ShellThrow()
    {
        var a = NetworkManager.singleton.spawnPrefabs[4]; //balon
        GameObject shell = Instantiate(a, transform.position + localplayer.model.transform.forward * 7, transform.rotation, null);
        NetworkServer.Spawn(shell);

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
