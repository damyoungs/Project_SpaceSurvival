using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public float jumpHeight = 8.0f;
    public float rotateSensitiveX = 30.0f;
    public float rotateSensitiveY = 30.0f;
    public float gravity = 15.0f;
    public float cameraCulling = 1.0f;
    public CinemachineVirtualCamera virtualCamera;

    Vector3 moveDir = Vector3.zero;
    float curRotateY = 0.0f;
    bool isJumping = false;
    int jumpCount = 0;

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
    Animator animator;
    Transform cameraPos;
    CharacterController controller;
    public CharacterController Controller => controller;

    CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera Cinemachine => cinemachine;
    Transform cam;

    readonly int Hash_Speed = Animator.StringToHash("Speed");
    readonly int Hash_IsJump = Animator.StringToHash("IsJump");
    readonly int Hash_InputX = Animator.StringToHash("InputX");
    readonly int Hash_InputY = Animator.StringToHash("InputY");
    readonly int Hash_IsRun = Animator.StringToHash("IsRun");
    readonly int Hash_IsJumping = Animator.StringToHash("IsJumping");

    const float animatorWalkSpeed = 0.5f;
    const float animatorRunSpeed = 1.0f;

    public Action interaction;

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        animator = GetComponent<Animator>();
        cameraPos = transform.GetChild(21);
        controller = GetComponent<CharacterController>();
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        cam = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
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
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Dash.canceled += OnDash;
        inputActions.Player.Action.performed += OnInteract;
        inputActions.Mouse.Enable();
        inputActions.Mouse.MouseVector2.performed += OnMouseDelta;
    }

    private void OnDisable()
    {
        inputActions.Mouse.MouseVector2.performed -= OnMouseDelta;
        inputActions.Mouse.Disable();
        inputActions.Player.Action.performed -= OnInteract;
        inputActions.Player.Dash.canceled -= OnDash;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (!IsGrounded())
        {
            moveDir.y -= gravity * Time.fixedDeltaTime;
        }

        controller.Move(Time.fixedDeltaTime * speed * transform.TransformDirection(new Vector3(moveDir.x, 0.0f, moveDir.z)));
        controller.Move(Time.fixedDeltaTime * new Vector3(0.0f, moveDir.y, 0.0f));
    }

    private void LateUpdate()
    {
        if ((cameraPos.position - virtualCamera.transform.position).sqrMagnitude < cameraCulling)
        {
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
        }
        else
        {
            Camera.main.cullingMask = -1;
        }
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        moveDir.x = dir.x;
        moveDir.z = dir.y;

        if (!isJumping)
        {

        animator.SetFloat(Hash_InputX, dir.x);
        animator.SetFloat(Hash_InputY, dir.y);
        }

        if (context.performed)
        {
            if (speed > runSpeed - 0.01f)
            {
                State = PlayerState.Run;
                animator.SetFloat(Hash_Speed, animatorRunSpeed);
            }
            else if (speed > walkSpeed - 0.01f)
            {
                State = PlayerState.Walk;
                animator.SetFloat(Hash_Speed, animatorWalkSpeed);
            }
            
        }
        else
        {
            State = PlayerState.Idle;
            animator.SetFloat(Hash_Speed, 0);
        }

    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isJumping && jumpCount < 2)
        {
            moveDir.y = jumpHeight;
            isJumping = true;
            animator.SetTrigger(Hash_IsJump);
            animator.SetBool(Hash_IsJumping, true);
            jumpCount++;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            speed = walkSpeed;
            if (State == PlayerState.Run)
            {
                State = PlayerState.Walk;
                animator.SetFloat(Hash_Speed, animatorWalkSpeed);
                animator.SetBool(Hash_IsRun, false);
            }
        }
        else
        {
            speed = runSpeed;
            if (State == PlayerState.Walk)
            {
                State = PlayerState.Run;
                animator.SetFloat(Hash_Speed, animatorRunSpeed);
                animator.SetBool(Hash_IsRun, true);
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interaction?.Invoke();
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up, rotateX);

        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;
        curRotateY -= rotateY;
        curRotateY = Mathf.Clamp(curRotateY, -60.0f, 60.0f);
        cameraPos.rotation = Quaternion.Euler(curRotateY, cameraPos.eulerAngles.y, cameraPos.eulerAngles.z);
        //cam.rotation = Quaternion.Euler(curRotateY, cam.eulerAngles.y, cam.eulerAngles.z);
    }

    private bool IsGrounded()
    {
        Vector3 groundCheckPosition = new Vector3(transform.position.x, transform.position.y + controller.radius * 0.5f, transform.position.z);
        if (Physics.CheckSphere(groundCheckPosition, controller.radius, LayerMask.GetMask("Ground")))
        {
            if (moveDir.y < jumpHeight)
            {
                moveDir.y = -0.01f;
            }
            if (isJumping)
            {
                animator.SetBool(Hash_IsJumping, false);
            }
            isJumping = false;
            jumpCount = 0;
            return true;
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 0.25f * 0.5f, transform.position.z), 0.25f);
    }

#endif
}
