using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float gravityScale = 3;
    private static Vector2 inputWASD = new Vector2();
    private static Vector2 inputArrows = new Vector2();

    public static Vector2 GetInputWASD()
    {
        //reset input
        inputWASD = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputWASD.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputWASD.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputWASD.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputWASD.x = -1;
        }

        return inputWASD;
    }

    public static Vector2 GetInputArrows()
    {
        //reset input
        inputArrows = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputArrows.y = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            inputArrows.y = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputArrows.x = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputArrows.x = -1;
        }

        return inputArrows;
    }
}
