using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta3D : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player3D player = col.gameObject.GetComponent<Player3D>();
            if (player.isLocalPlayer)
            {
                player.Update2DStatus(true);

            }
        }
    }
}
