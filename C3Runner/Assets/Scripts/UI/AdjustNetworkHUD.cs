using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustNetworkHUD : MonoBehaviour
{
    NetworkManagerHUD hud;
    public int offset = 235;
    public float extraFactor = 1;

    void Start()
    {
        hud = GetComponent<NetworkManagerHUD>();
    }


    void FixedUpdate()
    {
        //hud.matrixFactor = extraFactor;
        hud.offsetX = (int)((Screen.width - offset));

        //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(CapFramerate.factor * extraFactor, CapFramerate.factor * extraFactor, 1));
    }
}
