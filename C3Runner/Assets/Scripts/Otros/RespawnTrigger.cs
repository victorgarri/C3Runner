using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 xypos = new Vector3(other.transform.position.x, 3, other.transform.position.z);
            other.transform.position = xypos;
        }
    }
}
