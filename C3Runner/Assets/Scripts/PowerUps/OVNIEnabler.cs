using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVNIEnabler : MonoBehaviour
{
    public float timeToEnable = 180; //3 mins
    public GameObject child;
    void Start()
    {
        StartCoroutine("EnableOVNI");
    }

    IEnumerator EnableOVNI()
    {
        child.SetActive(false);
        yield return new WaitForSeconds(timeToEnable);
        child.SetActive(true);
    }


}
