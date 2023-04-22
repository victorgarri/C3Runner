using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenCanion : MonoBehaviour
{
    public GameObject[] canions;
    public List<Transform> targets = new List<Transform>();
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Add(c.gameObject.transform);
        }

        SwitchChildren();
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Remove(c.gameObject.transform);
        }

        SwitchChildren();
    }

    void SwitchChildren()
    {
        bool onOff = targets.Count > 0;
        foreach (var child in canions)
        {
            child.GetComponent<ShooterCanion>().enabled = onOff;
        }
    }

}
