using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustNetworkHUD : MonoBehaviour
{
    NetworkManagerHUD hud;
    public int offset = 235;

    void Start()
    {
        hud = GetComponent<NetworkManagerHUD>();
    }


    void FixedUpdate()
    {
        hud.offsetX = Screen.width - offset;
    }
}
