using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    private PlayerInput _playerInput;
    //private Rigidbody2D _rigidbody2D;

    public GameObject ball;

    private float topeLeft = -5.3f;
    private float topeRight = 5.3f;

    bool canJump;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector2 movimiento = _playerInput.actions["Move"].ReadValue<Vector2>();
        //Debug.Log(movimiento);

        
        if (movimiento.x == 1)
        {
            
            Debug.Log("Derecha" + movimiento);
            _playerInput.transform.Translate(5 * Time.deltaTime, movimiento.y, 0);

            if (transform.position.x > topeRight)
            {
                this.transform.position = new Vector3(topeRight, transform.position.y, 0);
            }
            
        }
        
        if (movimiento.x == -1)
        {
            
            Debug.Log("Izquierda" + movimiento);
            _playerInput.transform.Translate(-5 * Time.deltaTime, movimiento.y, 0);

            if (transform.position.x < topeLeft)
            {
                this.transform.position = new Vector3(topeLeft, transform.position.y, 0);
            }
            
        }
        
        if (_playerInput.actions["Fire"].WasPressedThisFrame())
        {
            Instantiate(ball, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, this.transform.position.z), ball.transform.rotation);
            //Debug.Log("Mando");
        }

    }

/*
    IEnumerator TimeRainbow()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
*/
}