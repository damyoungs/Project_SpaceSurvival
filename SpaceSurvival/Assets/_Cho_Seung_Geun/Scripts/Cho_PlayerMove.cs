using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cho_PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
        Idle = 0,
        Walk,
        Run,
        Jump
    }

    enum AudioIndex
    {
        Walk = 0,
        JumpLanding
    }

    public float speed = 0.0f;                          // 현재 속도
    public float walkSpeed = 5.0f;                      // 걷는 속도
    public float runSpeed = 8.0f;                       // 달리는 속도
    public float jumpHeight = 8.0f;                     // 점프 높이
    public float rotateSensitiveX = 30.0f;              // 마우스 민감도 가로축
    public float rotateSensitiveY = 30.0f;              // 마우스 민감도 세로축
    public float gravity = 15.0f;                       // 중력 수치
    public float cameraCulling = 1.0f;                  // 카메라 컬링 수치
    public CinemachineVirtualCamera virtualCamera;      // 포탈 넘어갈 때 사용할 가상 카메라

    Vector3 moveDir = Vector3.zero;                     // 움직이는 방향
    float mouseCurrentRotateY = 0.0f;                   // 마우스가 위아래로 움직이는 현재 각도
    float onJumpSpeedSwitch = 0.0f;   // 공중에서 대쉬를 눌렀을 때 담아놓을 현재 스피드 상태(만약 대쉬를 누르고 떨어지면 바로 뛰는 상태로 변환)

    //float jumpCheckHeight = 0.0f;
    bool jumpCollisionCheck = false;                    // 땅에 붙어있는지 체크하는 컬리전 범위는 벗어나기 위해(점프 시 바로 체크되는 것 방지)
    int jumpCount = 0;                                  // 점프 횟수(0이면 지상에 있는 것)

    bool onPressMove = false;                           // 무브 키를 누른 상태인지
    bool onPressDash = false;                           // 대쉬 키를 누른 상태인지

    public PlayerState state = PlayerState.Idle;
    PlayerState State
    {
        get => state;
        set
        {
            if (state != value)                                                 // 상태가 다를 때만 변경
            {
                state = value;
                if (state == PlayerState.Walk || state == PlayerState.Run)      // 걷거나 뛰는 중이면 소리 재생
                {
                    audios[(int)AudioIndex.Walk].Play();
                }
                else                                                            // 그 외 소리 멈춤
                {
                    audios[(int)AudioIndex.Walk].Stop();
                }
            }
        }
    }

    InputKeyMouse inputActions;
    public InputKeyMouse InputActions => inputActions;
    Animator animator;
    Transform cameraRoot;                                               // 카메라가 찍는 대상(회전할 때 쓰임)
    CharacterController controller;
    public CharacterController Controller => controller;

    CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera Cinemachine => cinemachine;
    public AudioSource[] audios;

    // 해시 미리 캐싱해놓기
    //readonly int Hash_Speed = Animator.StringToHash("Speed");
    readonly int Hash_IsJump = Animator.StringToHash("IsJump");
    readonly int Hash_InputX = Animator.StringToHash("InputX");
    readonly int Hash_InputY = Animator.StringToHash("InputY");
    readonly int Hash_IsRun = Animator.StringToHash("IsRun");
    readonly int Hash_IsJumping = Animator.StringToHash("IsJumping");

    //const float animatorWalkSpeed = 0.5f;
    //const float animatorRunSpeed = 1.0f;

    public Action interaction;                                          // 상호작용에 쓰일 델리게이트

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        animator = GetComponent<Animator>();
        cameraRoot = transform.GetChild(21);
        controller = GetComponent<CharacterController>();
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;                       // 커서 고정
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

    private void Update()
    {
        if (!IsGrounded())                                          // 땅에 있는 게 아니면 계속 중력 적용
        {
            moveDir.y -= gravity * Time.deltaTime;
        }

        movedir_y = moveDir.y;
        controller.Move(Time.deltaTime * speed * transform.TransformDirection(new Vector3(moveDir.x, 0.0f, moveDir.z)));    // x, z축 이동
        controller.Move(Time.deltaTime * new Vector3(0.0f, moveDir.y, 0.0f));                                               // y축 이동
    }

    private void LateUpdate()
    {
        // 카메라의 거리가 일정 이하면 플레이어 렌더 안 함
        if ((cameraRoot.position - virtualCamera.transform.position).sqrMagnitude < cameraCulling) 
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

        moveDir.x = dir.x;                                  // 실제 이동할 x, y 값
        moveDir.z = dir.y;

        animator.SetFloat(Hash_InputX, dir.x);              // 블렌더 트리 파라메터에 x, y 넣어주기(애니메이션 변경)
        animator.SetFloat(Hash_InputY, dir.y);

        if (context.performed)                              // 이동키가 눌려있으면
        {
            onPressMove = true;                                 // 무브 키가 눌려있고
            if (speed > runSpeed - 0.01f)                   // 현재 속도가 달리는 속도 이상이면
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Run;                    // 점프 상태가 아닐 시 달리는 상태로 변경(점프 상태일 때 달리는 동작으로 변하는 것을 막기 위해)
                }
                //animator.SetFloat(Hash_Speed, animatorRunSpeed);
                animator.SetBool(Hash_IsRun, true);             // 걷는 블렌드 트리에서 뛰는 블렌드 트리로 변경
            }
            else if (speed > walkSpeed - 0.01f)             // 현재 속도가 걷는 속도면
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Walk;                   // 점프 상태가 아닐 시 걷는 상태로 변경(점프 상태일 때 걷는 동작으로 변하는 것을 막기 위해)
                }
                //animator.SetFloat(Hash_Speed, animatorWalkSpeed);
                animator.SetBool(Hash_IsRun, false);            // 달리는 블렌드 트리에서 걷는 블렌드 트리로 변경
            }
        }
        else                                                // 이동키가 안 눌려 있으면
        {
            onPressMove = false;                                // 무크 키가 안 눌려있고
            State = PlayerState.Idle;                           // 현재 상태 Idle로 변경
            //animator.SetFloat(Hash_Speed, 0);
        }

    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.canceled)                   // 대쉬 키가 떼지면
        {
            onPressDash = false;                        // 대쉬 키가 안 눌려있음
            animator.SetBool(Hash_IsRun, false);        // 뛰는 블렌드 트리에서 걷는 블렌드 트리로 변경
            if (State != PlayerState.Jump)              
            {
                speed = walkSpeed;                  // 점프 상태가 아니면 현재 스피드는 걷는 스피드로 된다.
            }
            else
            {
                onJumpSpeedSwitch = walkSpeed;      // 점프 상태면 다른 변수에 걷는 스피드를 저장해둔다.
            }

            if (jumpCount == 0)                     // 지상에 붙어있을 경우
            {
                if (State == PlayerState.Run)           // 뛰는 상태일 경우(점프시나 대기 상태일 때 변경되면 발걸음 소리가 재생된다 - 프로퍼티)
                {
                    audios[(int)AudioIndex.Walk].pitch = 1.26f;         // 발걸음 재생 시간이 1.26배가 된다.(애니메이션 걷는 동작에 맞춤)
                    State = PlayerState.Walk;                           // 걷는 상태로 변경
                    //animator.SetFloat(Hash_Speed, animatorWalkSpeed);
                }
            }
        }
        else                                    // 대쉬 키가 눌렸으면
        {
            onPressDash = true;                     // 대쉬 키가 눌려있음
            animator.SetBool(Hash_IsRun, true);     // 걷는 블렌드 트리에서 뛰는 블렌드 트리로 변경
            if (State != PlayerState.Jump)
            {
                speed = runSpeed;                   // 점프 상태가 아니면 현재 스피드는 뛰는 스피드로 된다.
            }
            else
            {
                onJumpSpeedSwitch = runSpeed;       // 점프 상태면 다른 변수에 뛰는 스피드를 저장해둔다.
            }

            if (jumpCount == 0)                     // 지상에 붙어있을 경우
            {
                if (State == PlayerState.Walk)          // 뛰는 상태일 경우(점프시나 대기 상태일 때 변경되면 발걸음 소리가 재생된다 - 프로퍼티)
                {
                    audios[(int)AudioIndex.Walk].pitch = 2.0f;      // 발걸음 재생 시간이 2배가 된다.(애니메이션 걷는 동작에 맞춤)
                    State = PlayerState.Run;                        // 뛰는 상태로 변경
                    //animator.SetFloat(Hash_Speed, animatorRunSpeed);
                }
            }
        }
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (jumpCount < 2)
        {
            //audios[(int)AudioIndex.Jump].Play();
            moveDir.y = jumpHeight;
            //if (jumpCount == 0)
            //{
            //    jumpCheckHeight = transform.position.y + controller.radius * 2;
            //}
            jumpCollisionCheck = true;
            animator.SetTrigger(Hash_IsJump);
            animator.SetBool(Hash_IsJumping, true);
            jumpCount++;
            StartCoroutine(JumpCollisionTime());
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
        mouseCurrentRotateY -= rotateY;
        mouseCurrentRotateY = Mathf.Clamp(mouseCurrentRotateY, -60.0f, 60.0f);
        cameraRoot.rotation = Quaternion.Euler(mouseCurrentRotateY, cameraRoot.eulerAngles.y, cameraRoot.eulerAngles.z);
    }

    public float movedir_y;

    private bool IsGrounded()
    {
        //if (jumpChecking && transform.position.y > jumpCheckHeight)
        //{
        //    jumpChecking = false;
        //}
        Vector3 groundCheckPosition = new Vector3(transform.position.x, transform.position.y + controller.radius * 0.5f, transform.position.z);
        if (Physics.CheckSphere(groundCheckPosition, controller.radius, LayerMask.GetMask("Ground")))
        {
            if (!jumpCollisionCheck)
            {
                if (moveDir.y < jumpHeight)                 // 공중에서 내려올때 점프상태되는것 수정
                {
                    moveDir.y = -2.0f;
                }
                if (jumpCount > 0)
                {
                    animator.SetBool(Hash_IsJumping, false);
                    audios[(int)AudioIndex.JumpLanding].Play();
                    if (!onPressMove)
                    {
                        State = PlayerState.Idle;
                    }
                    else
                    {
                        if (!onPressDash)
                        {
                            State = PlayerState.Walk;
                        }
                        else
                        {
                            State = PlayerState.Run;
                        }
                    }

                    if (onJumpSpeedSwitch > 0.0f)
                    {
                        speed = onJumpSpeedSwitch;
                        onJumpSpeedSwitch = 0.0f;
                    }
                }

                jumpCollisionCheck = false;
                jumpCount = 0;
                return true;
            }
        }
        State = PlayerState.Jump;

        return false;
    }

    IEnumerator JumpCollisionTime()
    {
        yield return new WaitForSeconds(0.3f);
        jumpCollisionCheck = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 0.125f, transform.position.z), 0.25f);
    }

#endif
}
