using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountdownTimer : NetworkBehaviour
{
    public bool startCountingOnStart = false;
    public Text textCountdown;
    public float maxTime = 120;
    [SyncVar] public float timeLeft;
    [SyncVar] public bool timerStarted;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K) && focused)
        //{
        //    startCountdown();
        //}
    }

    [ClientRpc]
    void Start()
    {
        if (!startCountingOnStart && !timerStarted)
        {
            timerStarted = true;
            timeLeft = maxTime;
            updateText();
            InvokeRepeating("countDown", 1, 1);
        }
    }


    [ClientRpc]
    public void startCountdown()
    {
        if (!timerStarted)
        {
            timerStarted = true;
            timeLeft = maxTime;
            updateText();
            InvokeRepeating("countDown", 1, 1);
        }
    }

    void countDown()
    {
        timeLeft -= 1;
        updateText();
        if (timeLeft == 0)
        {
            CancelInvoke("countDown");
            //do something else
            NetworkRoomManager.singleton.ServerChangeScene("OfflineScene");
        }
    }

    float mins, secs;
    void updateText()
    {
        secs = timeLeft % 60;
        mins = (timeLeft - secs) / 60;
        if (textCountdown != null)
            textCountdown.text = string.Format("{0:0}:{1:00}", mins, secs);
    }
    //quitar luego
    bool focused;
    private void OnApplicationFocus(bool focus)
    {
        focused = focus;
    }

    void OnDestroy()
    {
        CancelInvoke("countDown");
    }

}
