using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DisableMobileUI : MonoBehaviour
{
    public PlayerInput pi;
    public Image[] canvasElements;

    void Update()
    {
        if (pi.actions["Menu"].WasPressedThisFrame())
        {
            foreach (Image img in canvasElements)
            {
                img.enabled = !img.enabled;
            }


        }
    }
}
