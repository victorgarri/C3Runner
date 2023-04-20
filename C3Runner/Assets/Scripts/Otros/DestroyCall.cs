using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCall : MonoBehaviour
{

    public void DestroyThisGameObject(float time)
    {
        Destroy(gameObject, time);
    }
}
