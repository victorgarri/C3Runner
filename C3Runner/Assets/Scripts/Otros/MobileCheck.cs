using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !USING_MOBILE
        gameObject.SetActive(false);    
#endif
    }

}
