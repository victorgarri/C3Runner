using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVNIawake : MonoBehaviour
{
    public GameObject explosionFX;

    private void OnEnable()
    {
        var explosion = Instantiate(explosionFX, transform.position, transform.rotation, null);
        explosion.transform.localScale *= 20;
    }

    private void OnDisable()
    {
        var explosion = Instantiate(explosionFX, transform.position, transform.rotation, null);
        explosion.transform.localScale *= 20;
    }
}
