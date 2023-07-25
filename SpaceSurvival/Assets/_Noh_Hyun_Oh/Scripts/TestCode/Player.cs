using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

/// <summary>
/// TestCode ¿‘¥œ¥Ÿ
/// </summary>
public class Player : MonoBehaviour
{
    InputKeyMouse inputSystem;
    [SerializeField]
    private float defaultSpeed = 1.0f;

    public WeatherLightCameraBase weatherLightCamera;

    private void Awake()
    {
        inputSystem = new InputKeyMouse();

        
    }

    
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Move.performed += OnMoveAction;
        inputSystem.Player.Jump.performed += OnJumpAction;

    }

    private void OnDisable()
    {
        inputSystem.Player.Jump.performed -= OnJumpAction;
        inputSystem.Player.Move.performed -= OnMoveAction;
        inputSystem.Disable();
    }
   
    private void MoveAndJump(UnityEngine.InputSystem.InputAction.CallbackContext context) {
    }
    private void OnJumpAction(InputAction.CallbackContext context)
    {

    }

    private void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.performed) {
            transform.Translate(Time.deltaTime * defaultSpeed *  context.ReadValue<Vector3>());
           
        }

    }

}

