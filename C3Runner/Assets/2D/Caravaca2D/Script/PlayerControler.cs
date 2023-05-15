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
    public VariableJoystick VariableJoystick;
    private float topeLeft = -5.3f;
    private float topeRight = 5.3f;

    private AudioSource audioSource;

    bool canJump;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        #if !(USING_MOBILE||UNITY_EDITOR)
            Vector2 movimiento = _playerInput.actions["Move"].ReadValue<Vector2>(); 
        #else
            Vector2 movimiento = VariableJoystick.Direction;
        #endif
        
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
        #if !(USING_MOBILE||UNITY_EDITOR)
            if (_playerInput.actions["Fire"].WasPressedThisFrame())
            {
                Shoot();
            }
        #endif
        

    }

    public void Shoot()
    {
        Instantiate(ball, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, this.transform.position.z), ball.transform.rotation);
        //Debug.Log("Mando");
        audioSource.Play();
    }
/*
    IEnumerator TimeRainbow()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
*/
}