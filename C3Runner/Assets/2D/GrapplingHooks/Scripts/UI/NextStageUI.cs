using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageUI : MonoBehaviour
{

    bool locked;
    public GameObject nextStageTxt;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!locked)
        {
            locked = true;
            nextStageTxt.SetActive(true);
        }
    }
}
