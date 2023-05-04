using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Meta3D : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    public GameObject confetti, location;

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


            }
            countdownTimer.startCountdown();
        }
    }
}
