using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBetweenTwoTransforms : MonoBehaviour
{
    public Transform[] position;
    LineRenderer ln;

    void Start()
    {
        ln = GetComponent<LineRenderer>();
        ln.positionCount = 2;
        DrawLine();
    }

    void DrawLine()
    {
        for (int i = 0; i < position.Length; i++)
        {
            ln.SetPosition(i, position[i].position);
        }
    }

    void Update()
    {
        DrawLine();
    }
}
