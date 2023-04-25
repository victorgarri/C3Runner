using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioAEscenaMinijuego : MonoBehaviour
{
    //public string nameOfNextScene = "Offline Scene"; //por defecto
    private bool inprogress = false;
    Animator anim;

    public GameObject Scene3D;
    public GameObject Scene2D;
    Player3D localplayer;

    public Transform nextSpawnPosition;

    void SwitchScene()
    {
        //SceneManager.LoadScene(nameOfNextScene);

        localplayer.DisableFeatures();
        localplayer.DisableRB();
        localplayer.DisableMeshRenderer();
        localplayer.transform.position = nextSpawnPosition.position;
        Scene2D.SetActive(true);
        Scene2D.GetComponent<PlayerHolder>().localplayer = localplayer;
        Scene3D.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!inprogress && col.gameObject.tag == "Player")
        {
            Player3D player = col.gameObject.GetComponent<Player3D>();
            if (player.isLocalPlayer)
            {
                localplayer = player;
                localplayer.Update2DStatus(true);
                anim = player.transform.Find("Canvas").GetComponent<Animator>();

                inprogress = true; //prevent executing twice
                StartCoroutine("Fade");
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
        SwitchScene();
    }



}