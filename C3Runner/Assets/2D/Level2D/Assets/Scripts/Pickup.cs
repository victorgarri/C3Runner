using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Level2DGameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Level2DGameManager>();

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateScore(100);
            Destroy(gameObject);
        }
    }
}
