using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFramerate : MonoBehaviour
{
    public int vsync = 0;
    public int fps = 60;
    public int resolutionX = 1920, resolutionY = 1080;
    public float renderDistance = 1500;


    void Awake()
    {
        //ApplySettings();

    }

    void Update()
    {
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        //Application.targetFrameRate = fps;
    }

    public void ApplySettings()
    {
#if !(UNITY_ANDROID || UNITY_EDITOR)
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        Application.targetFrameRate = fps;

#else
        Screen.SetResolution(resolutionX, resolutionY, FullScreenMode.ExclusiveFullScreen, 60);
#endif
    }
}
