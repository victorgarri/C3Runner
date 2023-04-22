using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideDelay : MonoBehaviour
{

    void Start()
    {
        StartCoroutine("Hide");
    }


    IEnumerator Hide()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
