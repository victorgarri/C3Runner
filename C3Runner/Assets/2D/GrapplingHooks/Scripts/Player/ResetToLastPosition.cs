using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToLastPosition : MonoBehaviour
{
    Vector3 pos = new Vector3();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            PlayerSpider ps = collision.gameObject.GetComponent<PlayerSpider>();
            pos.x = ps.lastGroundPosition.x;
            pos.y = ps.lastGroundPosition.y;
            pos.z = collision.transform.position.z;

            collision.gameObject.transform.position = pos;

            ps.PlaySquishSound();

        }
    }
}
