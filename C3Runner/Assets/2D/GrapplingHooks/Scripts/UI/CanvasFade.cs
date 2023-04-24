using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    bool locked;
    public Animator canvasAnim;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!locked)
        {
            locked = true;
            canvasAnim.Play("FadeOut");
        }
    }
}
