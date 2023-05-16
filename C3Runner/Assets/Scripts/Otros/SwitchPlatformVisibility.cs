using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlatformVisibility : MonoBehaviour
{
    public bool shouldBeActiveOnMobile = false;

    void Start()
    {
#if (UNITY_ANDROID || UNITY_EDITOR)
        gameObject.SetActive(shouldBeActiveOnMobile);
#else   
    gameObject.SetActive(!shouldBeActiveOnMobile);
#endif
    }

}
