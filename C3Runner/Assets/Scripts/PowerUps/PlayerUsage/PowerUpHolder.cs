using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerUpHolder : NetworkBehaviour
{
    public GameObject powerUp;
    public Player3D localplayer;
    PlayerInput pi;

    public Image indicatorUIBackImage;
    Color oldColor;

    public Sprite[] powerUpImages;//shell, speedup, invuln, bounce
    public float currentMaxFactor = 1;
    public float currentFactorValue = 1;
    public float currentFactor = 0;
    public bool startCounter = false;

    void Start()
    {
        if (localplayer == null)
        {
            localplayer = GetComponent<Player3D>();
        }

        pi = GetComponent<PlayerInput>();
        UpdateUI(0);
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

        if (startCounter)
        {
            if (currentFactorValue < currentMaxFactor)
            {
                currentFactorValue += Time.deltaTime;
                currentFactor = currentFactorValue / currentMaxFactor;
                UpdateUI(1 - currentFactor);
            }
            else
            {
                startCounter = false;
                UpdateUI(0);
                //oldColor = powerUpImage.color;
                //oldColor.a = 0;
                //powerUpImage.color = oldColor;
            }
        }
    }


    public void GetPowerUp(GameObject pu)
    {
        powerUp = pu;

        try
        {

            if (powerUp.GetComponent<Shell>() != null)
            {
                indicatorUIBackImage.sprite = powerUpImages[0];
            }

            if (powerUp.GetComponent<SpeedUp>() != null)
            {
                indicatorUIBackImage.sprite = powerUpImages[1];
            }

            if (powerUp.GetComponent<Invulnerability>() != null)
            {
                indicatorUIBackImage.sprite = powerUpImages[2];
            }

            if (powerUp.GetComponent<BounceOtherPlayers>() != null)
            {
                indicatorUIBackImage.sprite = powerUpImages[3];
            }

        }
        catch (System.Exception e)
        {
            print(e);
        }
        finally
        {
            //oldColor = powerUpImage.color;
            //oldColor.a = 1;
            //powerUpImage.color = oldColor;
        }

        startCounter = false;
        UpdateUI(1);
    }


    public void UpdateUI(float factor)
    {
        //indicatorUIBackImage.material.SetFloat("_FillAmount", factor); //0 colored, 1 black
        indicatorUIBackImage.fillAmount = factor;
    }

    void ActivatePowerUp()
    {
        try
        {
            currentFactorValue = 0;
            UpdateUI(1);

            if (powerUp.GetComponent<Shell>() != null)
            {
                ShellThrow();
                currentMaxFactor = 1;
            }

            if (powerUp.GetComponent<SpeedUp>() != null)
            {

                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                var su = p.GetComponent<SpeedUp>();
                su.Activate(localplayer);
                currentMaxFactor = su.time;
            }

            if (powerUp.GetComponent<Invulnerability>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                var inv = p.GetComponent<Invulnerability>();
                inv.Activate(localplayer);
                currentMaxFactor = inv.time;
            }

            if (powerUp.GetComponent<BounceOtherPlayers>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null); //REPLACE WITH NETCODE
                var bop = p.GetComponent<BounceOtherPlayers>();
                bop.Activate(localplayer);
                currentMaxFactor = bop.time;
            }

        }
        catch (System.Exception e)
        {
            print(e);
        }
        finally
        {
            powerUp = null;
            startCounter = true;
            //oldColor = powerUpImage.color;
            //oldColor.a = 0;
            //powerUpImage.color = oldColor;
            //powerUpImage.sprite = null;

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
