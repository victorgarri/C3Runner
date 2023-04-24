using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class EndGame : MonoBehaviour
{
    
    public PlayerInput pi;
    
    public void End()
    {
        if (gameObject.activeSelf)
        {
            if (GetInputButtonSouth())
            {
                StartCoroutine("Fade");
            }
            
        }
    }

    public void buttonEnd()
    {
        if (gameObject.activeSelf)
        {

                StartCoroutine("Fade");
            
            
        }
    }
    
    //public string nameOfNextScene = "OfflineScene"; //por defecto

    Animator anim;

    public GameObject Scene3D;
    public GameObject Scene2D;
    //public Transform nextSpawnPosition;

    Player3D localplayer;

    void SwitchScene()
    {
        //SceneManager.LoadScene(nameOfNextScene);

        localplayer = Scene2D.GetComponent<PlayerHolder>().localplayer;
        //localplayer.transform.position = nextSpawnPosition.position;
        localplayer.EnableFeatures();
        localplayer.EnableRB();
        localplayer.Update2DStatus(false);

        Scene3D.SetActive(true);
        Scene2D.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (/*!inprogress && */col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
            //if (player.isLocalPlayer)
            //{
            //    anim = player.transform.Find("Canvas").GetComponent<Animator>();

            //    inprogress = true; //prevent executing twice
            //    StartCoroutine("Fade");
            //}
            StartCoroutine("Fade");
            //SwitchScene();
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3);
        if (anim != null)
        {
            anim.Play("FadeOut");
        }
        yield return new WaitForSeconds(1);
        SwitchScene();
    }

    private bool inputHandlerButtonSouth;
    
    public bool GetInputButtonSouth()
    {
        //reset input
        inputHandlerButtonSouth = false;
        inputHandlerButtonSouth = pi.actions["Jump"].WasPressedThisFrame();

        return inputHandlerButtonSouth;
    }
}
