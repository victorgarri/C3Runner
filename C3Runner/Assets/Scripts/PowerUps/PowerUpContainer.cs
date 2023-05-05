using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpContainer : MonoBehaviour
{
    public GameObject powerUp; //prefab

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;
        if (obj.CompareTag("Player") && obj.GetComponent<Player3D>().isLocalPlayer)
        {
            //Play explosion FX

            //powerUp.transform.parent = null;
            other.GetComponent<PowerUpHolder>().GetPowerUp(powerUp);

            Destroy(gameObject);
        }
    }
}
