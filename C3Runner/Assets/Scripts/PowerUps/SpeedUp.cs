using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float speedMultiplier = 2;
    public float time = 8;
    public Player3D localplayer;

    public void Activate(Player3D p)
    {
        localplayer = p;
        StartCoroutine("PowerUp");
    }

    IEnumerator PowerUp()
    {
        localplayer.speed *= speedMultiplier;
        localplayer.SpeedUpToggle();
        //localplayer.SpeedUpToggleClient();
        yield return new WaitForSeconds(time);
        localplayer.speed /= speedMultiplier;
        localplayer.SpeedUpToggle();
        //localplayer.SpeedUpToggleClient();
        Destroy(gameObject);
    }
}
