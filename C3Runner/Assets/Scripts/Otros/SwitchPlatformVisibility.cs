using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlatformVisibility : MonoBehaviour
{
    public bool shouldBeActiveOnMobile = false;

    void Start()
    {
#if (USING_MOBILE || UNITY_EDITOR)
        gameObject.SetActive(shouldBeActiveOnMobile);
#endif
    }

}
