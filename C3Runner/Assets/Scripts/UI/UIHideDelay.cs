using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHideDelay : MonoBehaviour
{
    public PlayerInput pi;

    void Start()
    {
        StartCoroutine("Hide");
    }


    IEnumerator Hide()
    {
        yield return new WaitForSeconds(5);
        pi.enabled = true;
        gameObject.SetActive(false);
    }
}
