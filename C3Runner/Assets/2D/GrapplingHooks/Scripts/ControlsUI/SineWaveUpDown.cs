using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveUpDown : MonoBehaviour
{

    public float scale = 60;
    public float speed = 5;
    Vector3 newPos = new Vector3();

    void Update()
    {
        newPos = transform.position;
        newPos.y = transform.position.y + Mathf.Sin(Time.realtimeSinceStartup * speed) / scale;

        transform.position = newPos;
    }
}
