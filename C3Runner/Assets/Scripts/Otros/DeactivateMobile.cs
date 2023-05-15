using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !(USING_MOBILE||UNITY_EDITOR)
        gameObject.SetActive(false);    
#endif
    }

}
