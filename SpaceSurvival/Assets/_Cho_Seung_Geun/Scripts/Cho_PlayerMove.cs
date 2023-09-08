using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Cho_PlayerMove : MonoBehaviour
{
    public float speed = 0.0f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 5.0f;
    public float jumpHeight = 5.0f;
    public float rotateSensitiveX = 8.0f;

    float rotateDir = 0.0f;
    Vector3 moveDir = Vector3.zero;

    InputKeyMouse inputActions;
    Rigidbody rigid;

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = walkSpeed;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Dash.performed += onDash;
        inputActions.Player.Dash.canceled += onDash;
        inputActions.Mouse.Enable();
        inputActions.Mouse.MouseVector2.performed += OnMouseDelta;
        //inputActions.Mouse.MouseX.performed += valueMouseX;
    }

    private void valueMouseX(InputAction.CallbackContext context)
    {
        mouseX = context.ReadValue<float>();
    }

    private void OnDisable()
    {
        inputActions.Mouse.MouseVector2.performed -= OnMouseDelta;
        inputActions.Mouse.Disable();
        inputActions.Player.Dash.canceled -= onDash;
        inputActions.Player.Dash.performed -= onDash;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }


    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * moveDir);
    }

    public float mouseX = 0.0f;


    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        moveDir.x = dir.x;
        moveDir.z = dir.y;

    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        rigid.AddForce(jumpHeight * transform.up, ForceMode.Impulse);
    }

    private void onDash(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            speed = walkSpeed;
        }
        else
        {
            speed = runSpeed;
        }
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        rotateDir = temp.x * rotateSensitiveX * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateDir);
    }
}
