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

    //public GameObject player;
    //public GameObject cinemachine;
    //public GameObject timeline;

    private bool isPlayerExit = false;

    public Image exitToScene2D;
    public CanvasGroup mainSceneControls;

    //private bool couroutineStarted = false;
    private bool hiddenMainSceneControls;

    PlayerInput pi;

    bool isLocalPlayer;

    void Start()
    {
        //DCM: no sé si hay que añadirle un poco de delay para que cargue el personaje en red
        var parent = transform.parent;
        if (parent != null)
        {

            if (transform.parent.GetComponent<Player3D>() != null)
            {
                //ESCENA 3D
                Player3D p = transform.parent.GetComponent<Player3D>();
                isLocalPlayer = p.isLocalPlayer;

                if (isLocalPlayer)
                {
                    var players = GameObject.FindObjectsOfType(p.GetType());
                    foreach (var item in players)
                    {
                        if (item.GetComponent<Player3D>()._isLocalPlayer)
                        {
                            pi = item.GetComponent<Player3D>().pi;
                        }
                    }

                }
                else
                {
                    gameObject.SetActive(false);
                }
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


    void Update()
    {
        if (isLocalPlayer)
        {
            //Debug.Log(Application.loadedLevelName);
            //switch (Application.loadedLevelName)

            //SwitchScene();

            //if (!couroutineStarted)
            if (focused && GetInputButtonStart() && !hiddenMainSceneControls)
            {
                hiddenMainSceneControls = true;
                Debug.Log("pepe");
                StartCoroutine("exitControls");
            }
        }
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            SwitchSceneFade();
        }
    }

    void SwitchSceneFade()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainScene":
                if (isPlayerExit)
                {
                    StartCoroutine(EndLevel3D(exitToScene2D));
                }
                else if (exitToScene2D.color.a > 0)
                {
                    exitToScene2D.color = new Color(exitToScene2D.color.r, exitToScene2D.color.g, exitToScene2D.color.b, exitToScene2D.color.a - speed);
                }
                break;
            case "Level 1":
                if (isPlayerExit)
                {
                    StartCoroutine(EndLevel2D(exitToScene2D));
                }
                else if (exitToScene2D.color.a > 0)
                {
                    exitToScene2D.color = new Color(exitToScene2D.color.r, exitToScene2D.color.g, exitToScene2D.color.b, exitToScene2D.color.a - speed);
                }
                break;


        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player3D>().isLocalPlayer)
                isPlayerExit = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player3D>().isLocalPlayer)
                isPlayerExit = true;
        }
    }

    IEnumerator EndLevel3D(Image canvas)
    {
        yield return new WaitForSeconds(3);

        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Level 1");

    }

    IEnumerator EndLevel2D(Image canvas)
    {
        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("MainScene");
    }

    IEnumerator exitControls()
    {
        if (isLocalPlayer)
        {
            gameObject.GetComponent<Animator>().Play("Fade");
            yield return new WaitForSeconds(2);
            mainSceneControls.gameObject.SetActive(false);
        }
    }

    bool inputHandlerButtonStart;
    public bool GetInputButtonStart()
    {
        //reset input
        inputHandlerButtonStart = false;
        inputHandlerButtonStart = pi.actions["Menu"].WasPressedThisFrame();

        return inputHandlerButtonStart;
    }


    bool focused = true;
    private void OnApplicationFocus(bool focus)
    {
        focused = focus;
    }
}