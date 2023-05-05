using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOtherPlayers : MonoBehaviour
{
    public float time = 15;
    public Player3D localplayer;

    public void Activate(Player3D p)
    {
        localplayer = p;
        StartCoroutine("PowerUp");
    }

    IEnumerator PowerUp()
    {
        //localplayer.bounceOtherPlayers = true;
        localplayer.BounceOtherPlayersToggle();
        yield return new WaitForSeconds(time);
        //localplayer.bounceOtherPlayers = false;
        localplayer.BounceOtherPlayersToggle();
        Destroy(gameObject);
    }
}
