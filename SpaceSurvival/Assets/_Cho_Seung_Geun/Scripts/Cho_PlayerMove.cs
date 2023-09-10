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
    public float rotateSensitiveX = 30.0f;
    public float rotateSensitiveY = 30.0f;

    Vector3 moveDir = Vector3.zero;
    float curRotateY = 0.0f;
    bool isRun = false;

    InputKeyMouse inputActions;
    Rigidbody rigid;
    Animator animator;
    Transform cameraPos;

    readonly int IsMove_Hash = Animator.StringToHash("IsMove");
    readonly int IsRun_Hash = Animator.StringToHash("IsRun");

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraPos = transform.GetChild(22);
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
        rigid.MovePosition(rigid.position + transform.TransformDirection(Time.fixedDeltaTime * speed * moveDir));
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        if (context.performed)
        {
            animator.SetBool(IsMove_Hash, true);
        }
        else if (context.performed && isRun)
        {
            animator.SetBool(IsRun_Hash, true);
        }
        else if (context.performed && !isRun)
        {
            animator.SetBool(IsRun_Hash, false);
        }
        else
        {
            animator.SetBool(IsMove_Hash, false);
        }

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
            isRun = false;
        }
        else
        {
            speed = runSpeed;
            isRun = true;
        }
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        float rotateX = temp.x * rotateSensitiveX * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateX);

        float rotateY = temp.y * rotateSensitiveY * Time.deltaTime;
        curRotateY -= rotateY;
        curRotateY = Mathf.Clamp(curRotateY, -60.0f, 60.0f);
        cameraPos.rotation = Quaternion.Euler(curRotateY, cameraPos.eulerAngles.y, cameraPos.eulerAngles.z);
    }
}
