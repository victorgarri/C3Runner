using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFramerate : MonoBehaviour
{
    public int vsync = 0;
    public int fps = 60;


    void Awake()
    {
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        Application.targetFrameRate = fps;
    }

    void Update()
    {
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        //Application.targetFrameRate = fps;
    }
}
