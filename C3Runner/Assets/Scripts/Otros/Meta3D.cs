using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Meta3D : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    public GameObject confetti, location;
    public Text finishTimeTxt;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player3D player = col.gameObject.GetComponent<Player3D>();
            if (player.isLocalPlayer)
            {
                Instantiate(confetti, location.transform);
                player.Update2DStatus(true);
                player.GetComponent<PlayerInput>().enabled = false;
                player.transform.Find("WinnerScreen").Find("youwintext").GetComponent<Text>().text = "YOU WIN";

                CountdownTimer cdt = GameObject.FindObjectOfType<CountdownTimer>();
                float timerFinish = cdt.timeLeft;
                if (cdt.timerStarted)
                {
                    timerFinish = 120 - timerFinish; //reverse 
                    if (finishTimeTxt != null)
                        finishTimeTxt.text = "FinishTime: " + SecondsToFormattedTime(timerFinish); //show finish time.
                }
                else
                {
                    finishTimeTxt.text = "FinishTime: 0:00";
                }


            }
            countdownTimer.startCountdown();

        }
    }

    string SecondsToFormattedTime(float seconds)
    {
        float mins, secs;
        secs = seconds % 60;
        mins = (seconds - secs) / 60;
        string formattedTime = string.Format("{0:0}:{1:00}", mins, secs);
        return formattedTime;
    }

}
