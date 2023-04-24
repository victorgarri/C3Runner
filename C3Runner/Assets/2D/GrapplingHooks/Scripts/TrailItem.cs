using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailItem : MonoBehaviour
{
    public SpriteRenderer sr;
    void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && sr.enabled)
        {
            StartCoroutine("TurnOffAndOn");
        }
    }

    IEnumerator TurnOffAndOn()
    {
        sr.enabled = false;
        yield return new WaitForSeconds(2);
        sr.enabled = true;
    }
}
