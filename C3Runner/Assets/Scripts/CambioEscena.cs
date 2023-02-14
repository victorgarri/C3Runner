using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioEscena : MonoBehaviour
{
    private float speed = 0.008f;
    
    public GameObject player;
    //public GameObject cinemachine;
    //public GameObject timeline;
    
    private bool isPlayerExit = false;

    public Image exitToScene2D;

    void Update()
    {
        
        //Debug.Log(Application.loadedLevelName);

        if (Application.loadedLevelName == "MainScene")
        {
            
            if (isPlayerExit && Application.loadedLevelName == "MainScene")
            {
                //cinemachine.SetActive(true);
                //timeline.SetActive(true);
                StartCoroutine(EndLevel3D(exitToScene2D));
            }
            else if (exitToScene2D.color.a > 0)
            {
                exitToScene2D.color = new Color(exitToScene2D.color.r, exitToScene2D.color.g, exitToScene2D.color.b, exitToScene2D.color.a - speed);
            }

        }

        if (Application.loadedLevelName == "2D")
        {
            if (isPlayerExit && Application.loadedLevelName == "2D")
            {
                StartCoroutine(EndLevel2D(exitToScene2D));
            }
            else if (exitToScene2D.color.a > 0)
            {
                exitToScene2D.color = new Color(exitToScene2D.color.r, exitToScene2D.color.g, exitToScene2D.color.b, exitToScene2D.color.a - speed);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerExit = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            isPlayerExit = true;
        }
    }

    IEnumerator EndLevel3D(Image canvas)
    {
        yield return new WaitForSeconds(3);
        
        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);
        
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("2D");
        
    }
    
    IEnumerator EndLevel2D(Image canvas)
    {
        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, canvas.color.a + speed);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("MainScene");
    }
}