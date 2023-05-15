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
        if (isLocalPlayer && localplayer.focused && !localplayer.in2DGame)
        {
            GetInputButtonTrigger();

            if (powerUp != null)
            {
                if (inputHandlerButtonTrigger || powerUp.GetComponent<OVNI>() != null)
                {
                    ActivatePowerUp();
                }


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
        //if it's currently using a powerup
        if (!startCounter)
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

                if (powerUp.GetComponent<OVNI>() != null)
                {
                    indicatorUIBackImage.sprite = powerUpImages[4];
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
    }


    public void UpdateUI(float factor)
    {
        //indicatorUIBackImage.material.SetFloat("_FillAmount", factor); //0 colored, 1 black
        indicatorUIBackImage.fillAmount = factor;
    }

    public void ActivatePowerUp()
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

                var p = Instantiate(powerUp, transform.position, transform.rotation, null);
                var su = p.GetComponent<SpeedUp>();
                su.Activate(localplayer);
                currentMaxFactor = su.time;
            }

            if (powerUp.GetComponent<Invulnerability>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null);
                var inv = p.GetComponent<Invulnerability>();
                inv.Activate(localplayer);
                currentMaxFactor = inv.time;
            }

            if (powerUp.GetComponent<BounceOtherPlayers>() != null)
            {
                var p = Instantiate(powerUp, transform.position, transform.rotation, null);
                var bop = p.GetComponent<BounceOtherPlayers>();
                bop.Activate(localplayer);
                currentMaxFactor = bop.time;
            }

            //if (powerUp.GetComponent<OVNI>() != null)
            //{
            //    var p = Instantiate(powerUp, transform.position + Vector3.up * 7, transform.rotation, null); 
            //    var ovni = p.GetComponent<OVNI>();
            //    ovni.Activate(localplayer);
            //    currentMaxFactor = ovni.time;
            //}

            if (powerUp.GetComponent<OVNI>() != null)
            {
                //var p = Instantiate(powerUp, transform.position + Vector3.up * 7, transform.rotation, null);
                localplayer.GetComponent<Player3D>().OVNIToggle();//llamar para mostrar ovni child
                localplayer.GetComponent<OVNIplayer>().enabled = true;
                currentMaxFactor = 1;
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
        var prefab = NetworkManager.singleton.spawnPrefabs[4]; //balon
        GameObject shell = Instantiate(prefab, transform.position + localplayer.model.transform.forward * 7, transform.rotation, null);
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
