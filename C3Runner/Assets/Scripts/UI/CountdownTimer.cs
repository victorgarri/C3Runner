using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (startCountingOnStart && !timerStarted)
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
        if (timeLeft <= 0)
        {
            CancelInvoke("countDown");
            //do something else
            //NetworkRoomManager.singleton.ServerChangeScene("OfflineScene");

            List<Player3D> players = FindObjectsOfType<Player3D>().ToList();
            //print(players.Count);
            foreach (Player3D p in players)
            {
                if (p.GetComponent<Spectator>() == null || !p.GetComponent<Spectator>().isSpectator)
                {
                    p.DisableControls();
                }

            }
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

    void OnDestroy()
    {
        CancelInvoke("countDown");
    }

}
