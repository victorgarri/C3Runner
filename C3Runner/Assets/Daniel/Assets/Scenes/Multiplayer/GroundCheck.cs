using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        GroundCheckMethod();
    }

    private void GroundCheckMethod()
    {
        //Physics.Raycast(raycastOrigin.position, Vector3.down, 100f, layerMask);
        Physics.Raycast(raycastOrigin.position, Vector3.down, out RaycastHit hit, 100f, layerMask);

        if (hit.collider != null)
        {
            Vector3 temp = playerFeet.position;
            temp.y = hit.point.y;
            playerFeet.position = temp;
        }
    }
}
