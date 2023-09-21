using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Cho_PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        Idle = 0,
        Walk,
        Run,
        Jump
    }

    public float speed = 0.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpHeight = 5.0f;
    public float rotateSensitiveX = 30.0f;
    public float rotateSensitiveY = 30.0f;

    Vector3 moveDir = Vector3.zero;
    float curRotateY = 0.0f;
    float gravity = 0.0f;
    bool isJump = false;

    PlayerState state = PlayerState.Idle;
    PlayerState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
            }
        }
    }


    InputKeyMouse inputActions;
    //Rigidbody rigid;
    Animator animator;
    Transform cameraPos;
    CharacterController controller;

    readonly int Speed_Hash = Animator.StringToHash("Speed");

    const float animatorWalkSpeed = 0.5f;
    const float animatorRunSpeed = 1.0f;

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        //rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraPos = transform.GetChild(21);
        controller = GetComponent<CharacterController>();
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

    private void FixedUpdate()
    {
        //rigid.MovePosition(rigid.position + transform.TransformDirection(Time.fixedDeltaTime * speed * moveDir));
        //rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * moveDir);

        //controller.Move(Time.fixedDeltaTime * speed * transform.TransformDirection(moveDir));
        controller.SimpleMove(speed * transform.TransformDirection(moveDir));
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        moveDir.x = dir.x;
        moveDir.z = dir.y;

        if (context.performed)
        {
            if (speed > runSpeed - 0.01f)
            {
                State = PlayerState.Run;
                animator.SetFloat(Speed_Hash, animatorRunSpeed);
            }
            else if (speed > walkSpeed - 0.01f)
            {
                State = PlayerState.Walk;
                animator.SetFloat(Speed_Hash, animatorWalkSpeed);
            }
            
        }
        else
        {
            State = PlayerState.Idle;
            animator.SetFloat(Speed_Hash, 0);
        }

    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //rigid.velocity = Vector3.zero;
        //rigid.AddForce(jumpHeight * transform.up, ForceMode.Impulse);
        animator.SetTrigger("IsJump");
    }

    private void onDash(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            State = PlayerState.Walk;
            speed = walkSpeed;
            animator.SetFloat(Speed_Hash, animatorWalkSpeed);
        }
        else
        {
            State = PlayerState.Run;
            speed = runSpeed;
            animator.SetFloat(Speed_Hash, animatorRunSpeed);
        }
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        Debug.Log(temp);
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;
        //rigid.MoveRotation(rigid.rotation * Quaternion.AngleAxis(rotateX, Vector3.up));
        transform.Rotate(Vector3.up, rotateX);

        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;
        curRotateY -= rotateY;
        curRotateY = Mathf.Clamp(curRotateY, -60.0f, 60.0f);
        cameraPos.rotation = Quaternion.Euler(curRotateY, cameraPos.eulerAngles.y, cameraPos.eulerAngles.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

        }
    }
}