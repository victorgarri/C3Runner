using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float force = 10;
    Transform direction;
    public Animator baseModelAnim;
    Animator mainAnim;
    float lengthBoing = 1;

    void Start()
    {
        mainAnim = GetComponent<Animator>();
        direction = transform.Find("Direction");
        force *= GameManager.gravityScale;


        //ASIGNACI�N DIN�MICA DE LA LONGITUD DE LA ANIMACI�N
        AnimationClip[] clips = baseModelAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Boing":
                    lengthBoing = clip.length;
                    break;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.CompareTag("Player"))
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero; //Reset velocity
            obj.GetComponent<Rigidbody>().AddForce(direction.up * force, ForceMode.Impulse);
            baseModelAnim.Play("Boing");
            StartCoroutine("PauseMainAnimator");
        }
    }

    IEnumerator PauseMainAnimator()
    {
        mainAnim.speed = 0;
        yield return new WaitForSeconds(lengthBoing); //Creo que la animaci�n del spring es de un segundo solo
        mainAnim.speed = 1;
    }
}
