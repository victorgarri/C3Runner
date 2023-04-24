using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class EndGame : MonoBehaviour
{
    
    public PlayerInput pi;
    
    public void End()
    {
        StartCoroutine("Fade");
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
