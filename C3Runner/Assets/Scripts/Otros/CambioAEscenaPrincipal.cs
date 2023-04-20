using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioAEscenaPrincipal : MonoBehaviour
{
    public string nameOfNextScene = "OfflineScene"; //por defecto

    private bool inprogress = false;
    Animator anim;

    void SwitchScene()
    {
        SceneManager.LoadScene(nameOfNextScene);

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
            SwitchScene();
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