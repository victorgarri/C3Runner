using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioEscena : MonoBehaviour
{
    private float speed = 0.04f;
    private bool isPlayerExit = false;
    public Image exitToScene2D;
    public Animator anim;

    void Start()
    {
        //DCM: no sé si hay que añadirle un poco de delay para que cargue el personaje en red
        var parent = transform.parent;
        if (parent != null)
        {

            if (transform.parent.GetComponent<Player3D>() != null)
            {
                transform.parent = null;
                transform.position = Vector3.zero;
            }
            else
            {
                //ESCENA 2D
                GameObject.Find("Canvas").GetComponent<Animator>().Play("FadeIn");
            }
        }
    }
    void FixedUpdate()
    {
        //if (isPlayerExit)
        //{
        //    SwitchSceneFade();
        //}
    }

    void SwitchSceneFade()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainScene":
                if (isPlayerExit)
                {
                    SceneManager.LoadScene("Level 1");
                }
                else
                {
                    StartCoroutine("Fade");
                }
                break;
            case "Level 1":
                if (isPlayerExit)
                {
                    SceneManager.LoadScene("MainScene");
                }
                else
                {
                    StartCoroutine("Fade");
                }
                break;


        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player3D>().isLocalPlayer)
            {
                SwitchSceneFade();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player3D>().isLocalPlayer)
            {
                //isPlayerExit = true;
                SwitchSceneFade();
            }
        }
    }

    IEnumerator Fade()
    {
        if (anim != null)
        {
            anim.Play("FadeOut");
        }
        yield return new WaitForSeconds(1);
        isPlayerExit = true;
        SwitchSceneFade();
    }


    //IEnumerator EndLevel3D(Image canvas)
    //{
    //    yield return new WaitForSeconds(3);

    //    canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);

    //    yield return new WaitForSeconds(2);

    //    SceneManager.LoadScene("Level 1");

    //}

    //IEnumerator EndLevel2D(Image canvas)
    //{
    //    canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);

    //    yield return new WaitForSeconds(2);

    //    SceneManager.LoadScene("MainScene");
    //}


}