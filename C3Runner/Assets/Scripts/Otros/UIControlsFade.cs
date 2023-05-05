using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControlsFade : MonoBehaviour
{
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
                        if (item.GetComponent<Player3D>().isLocalPlayer)
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
    IEnumerator exitControls()
    {
        if (isLocalPlayer)
        {
            gameObject.GetComponent<Animator>().Play("FadeIn");
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
