using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{
    public bool win;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Win
            win = true;
            GameObject.Find("GameManager").GetComponent<Level2DGameManager>().Win();
        }
    }
}
