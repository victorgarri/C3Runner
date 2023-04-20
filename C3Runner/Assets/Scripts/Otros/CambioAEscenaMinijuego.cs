using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioAEscenaMinijuego : MonoBehaviour
{
    public string nameOfNextScene = "Offline Scene"; //por defecto

    private bool inprogress = false;
    Animator anim;

    void SwitchScene()
    {
        SceneManager.LoadScene(nameOfNextScene);

    }

    private void OnTriggerEnter(Collider col)
    {
        if (!inprogress && col.gameObject.tag == "Player")
        {
            Player3D player = col.gameObject.GetComponent<Player3D>();
            if (player.isLocalPlayer)
            {
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