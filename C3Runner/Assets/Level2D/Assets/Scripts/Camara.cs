using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject jugador;
    public Vector2 minimo;
    public Vector2 maximo;
    public float suavizado;
    Vector2 velocity;

    public bool lockY = false;
    public bool rememberLastX = false;

    float posX, posY;
    float maxPlayerPos;
    public float maxFixedCameraOffset = 5;

    private void FixedUpdate()
    {


        posX = Mathf.SmoothDamp(transform.position.x, jugador.transform.position.x, ref velocity.x, suavizado);
        posY = Mathf.SmoothDamp(transform.position.y, jugador.transform.position.y, ref velocity.y, suavizado);

        if (!lockY)
        {
            transform.position = new Vector3(Mathf.Clamp(posX, minimo.x, maximo.x), Mathf.Clamp(posY, minimo.y, maximo.y), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(posX, minimo.x, maximo.x), transform.position.y, transform.position.z);
        }

        if (rememberLastX)
        {
            if (maxPlayerPos < posX)
            {
                maxPlayerPos = posX;
                minimo.x = maxPlayerPos - maxFixedCameraOffset;
            }


        }


    }
}
