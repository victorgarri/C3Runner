using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CapFramerate : MonoBehaviour
{
    public int vsync = 0;
    public int fps = 60;
    public int resolutionX = 1366, resolutionY = 768;
    public float renderDistance = 1500;
    public bool active;
    public static float factor = 1;


    void Awake()
    {
        if (active)
            ApplySettings();

    }

    void Update()
    {
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        //Application.targetFrameRate = fps;
    }

    public void ApplySettings()
    {
#if (UNITY_ANDROID || UNITY_EDITOR)

        factor = 1;
        if (Screen.height > resolutionY || Screen.width > Screen.width)
        {
            factor = ((float)resolutionY / (float)Screen.height);
        }
        int finalResX = (int)(Screen.width * factor);
        int finalResY = (int)(Screen.height * factor);
        if (finalResX % 2 != 0) { finalResX++; }
        if (finalResY % 2 != 0) { finalResY++; }

        Screen.SetResolution(finalResX, finalResY, FullScreenMode.ExclusiveFullScreen, 60);
        print("Screen:" + Screen.width + "x" + Screen.height + "; Factor: " + factor);


#else
        //QualitySettings.vSyncCount = vsync;  // VSync must be disabled
        Application.targetFrameRate = fps;
#endif
    }
}
