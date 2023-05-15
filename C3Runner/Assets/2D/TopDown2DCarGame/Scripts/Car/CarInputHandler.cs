﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    public int playerNumber = 1;
    public bool isUIInput = false;
    public PlayerInput pi;
    Vector2 inputVector = Vector2.zero;
    public VariableJoystick HorizontalJoystick;
    public VariableJoystick VerticalJoystick;

    //Components
    TopDownCarController topDownCarController;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame and is frame dependent
    void Update()
    {
        if (isUIInput)
        {

        }
        else
        {
            inputVector = Vector2.zero;
            
                    //Get input from Unity's input system.
                    
                    #if USING_MOBILE
                        if (HorizontalJoystick.Direction.x is > 0.4f or < -0.4f)
                        {
                            inputVector.x = HorizontalJoystick.Direction.normalized.x;
                        }
                    
                        if (VerticalJoystick.Direction.y is > 0.4f or < -0.4f)
                        {
                            inputVector.y = VerticalJoystick.Direction.normalized.y;
                        }
            
                    #else
        
                        if (pi.actions["move"].ReadValue<Vector2>().normalized.y is > 0.4f or < -0.4f)
                        {
                            inputVector.y = pi.actions["move"].ReadValue<Vector2>().normalized.y;
                        }
                    
                        if (pi.actions["ControlerDirection"].ReadValue<Vector2>().normalized.x is > 0.4f or < -0.4f)
                        {
                            inputVector.x = pi.actions["ControlerDirection"].ReadValue<Vector2>().normalized.x;
                        }
            
                    #endif
                    
                    
                    
                    
                    
        }

        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }

    public void SetInput(Vector2 newInput)
    {
        inputVector = newInput;
    }
}
