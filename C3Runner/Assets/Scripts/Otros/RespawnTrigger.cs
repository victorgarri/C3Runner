using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    Vector3 offsetFromGround = new Vector3(0, 2, 0);
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Vector3 xypos = new Vector3(other.transform.position.x, 3, other.transform.position.z);
            //other.transform.position = xypos;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.transform.position = other.gameObject.GetComponent<Player3D>().lastGroundPosition + offsetFromGround;

        }
    }
}
