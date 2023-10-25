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
    readonly int Hash_InputX = Animator.StringToHash("InputX");
    readonly int Hash_InputY = Animator.StringToHash("InputY");
    readonly int Hash_IsRun = Animator.StringToHash("IsRun");
    readonly int Hash_IsJumping = Animator.StringToHash("IsJumping");

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
            onPressMove = true;                                 // 무브가 눌려 있다고 표시하고
            if (speed > runSpeed - 0.01f)                   // 현재 속도가 달리는 속도 이상이면
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Run;                    // 점프 상태가 아닐 시 달리는 상태로 변경(점프 상태일 때 달리는 동작으로 변하는 것을 막기 위해)
                }
                animator.SetBool(Hash_IsRun, true);             // 걷는 블렌드 트리에서 뛰는 블렌드 트리로 변경
            }
            else if (speed > walkSpeed - 0.01f)             // 현재 속도가 걷는 속도면
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Walk;                   // 점프 상태가 아닐 시 걷는 상태로 변경(점프 상태일 때 걷는 동작으로 변하는 것을 막기 위해)
                }
                animator.SetBool(Hash_IsRun, false);            // 달리는 블렌드 트리에서 걷는 블렌드 트리로 변경
            }
        }
        else                                                // 이동키가 안 눌려 있으면
        {
            onPressMove = false;                                // 무크가 안 눌려 있다고 false로 바뀌고
            State = PlayerState.Idle;                           // 현재 상태 Idle로 변경
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
            //else
            //{
            //    onJumpSpeedSwitch = walkSpeed;      // 점프 상태면 다른 변수에 걷는 스피드를 저장해둔다.
            //}
                onJumpSpeedSwitch = walkSpeed;      // 점프 상태면 다른 변수에 걷는 스피드를 저장해둔다.

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
            //else
            //{
            //    onJumpSpeedSwitch = runSpeed;       // 점프 상태면 다른 변수에 뛰는 스피드를 저장해둔다.
            //}
                onJumpSpeedSwitch = runSpeed;       // 점프 상태면 다른 변수에 뛰는 스피드를 저장해둔다.

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
        if (jumpCount < 2)                      // 점프 횟수가 최대치(이 프로젝트에선 2)를 넘지 않을 경우
        {
            moveDir.y = jumpHeight;             // 플레이어 수직 이동방향
            //if (jumpCount == 0)
            //{
            //    jumpCheckHeight = transform.position.y + controller.radius * 2;
            //}
            animator.SetBool(Hash_IsJumping, true);     // 애니메이션 변경
            jumpCount++;                                // 점프 횟수 +1
            jumpCollisionCheck = true;              // 점프컬리전(땅에 충돌해있는지 체크하는 구)이 활성화되어 점프시 바로 원래 상태로 되돌리는 것 방지
            StartCoroutine(JumpCollisionTime());        // 점프 컬리전 체크하는 코루틴 실행
        }
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        interaction?.Invoke();                          // 상호작용 델리게이트 실행
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();                        // 마우스의 전 프레임 대비 이동한 값 가져오기
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;    // 회전할 x축 값 구하기
        transform.Rotate(Vector3.up, rotateX);                              // 캐릭터 회전시켜주기

        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;    // 회전할 y축 값 구하기
        mouseCurrentRotateY -= rotateY;                                     // y축 값을 한 변수에 계속해 빼준다.
        mouseCurrentRotateY = Mathf.Clamp(mouseCurrentRotateY, -60.0f, 60.0f);      // 회전할 값의 최소, 최대 범위 지정
        cameraRoot.rotation = Quaternion.Euler(mouseCurrentRotateY, cameraRoot.eulerAngles.y, cameraRoot.eulerAngles.z);    // 카메라가 가리키는 대상을 회전시킴
    }

    private bool IsGrounded()
    {
        Vector3 groundCheckPosition = new Vector3(transform.position.x, transform.position.y + controller.radius * 0.5f, transform.position.z); // 땅 체크하는 콜라이더 위치
        if (Physics.CheckSphere(groundCheckPosition, controller.radius, LayerMask.GetMask("Ground")))   // 특정 범위에 Gound 레이어가 있으면 = 땅에 붙어있으면
        {
            if (!jumpCollisionCheck)            // 점프 컬리전이 꺼져있으면(또는 점프 후에 지났으면)
            {
                //if (moveDir.y < jumpHeight)
                //{
                //    moveDir.y = -2.0f;
                //}
                if (State == PlayerState.Jump)      // 점프 상태일 시(공중에서 내려온 경우)
                {
                    animator.SetBool(Hash_IsJumping, false);            // 애니메이션 변경
                    audios[(int)AudioIndex.JumpLanding].Play();         // 착지 소리 켜기
                    if (!onPressMove)             
                    {
                        State = PlayerState.Idle;       // 무브가 안 눌러있는 상태면 대기 상태로 만들고
                    }
                    else                                // 무브가 눌려 있으면
                    {
                        if (!onPressDash)               // 대쉬가 안 눌린 경우
                        {
                            State = PlayerState.Walk;       // 걷는 상태로 바꾼다
                        }
                        else                            // 대쉬가 눌려 있으면
                        {
                            State = PlayerState.Run;        // 뛰는 상태로 바꾼다.
                        }
                    }
                    speed = onJumpSpeedSwitch;      // 현재 속도는 변수에 담아뒀던 속도로 변경시킨다.
                }

                jumpCollisionCheck = false;         // 점프 컬리전 체크 끄기
                jumpCount = 0;                      // 점프 횟수 0으로 초기화
                return true;
            }
        }

        State = PlayerState.Jump;                   // 땅에 붙어있는 게 아니면 점프 상태이다.

        return false;
    }

    IEnumerator JumpCollisionTime()
    {
        yield return new WaitForSeconds(0.3f);
        jumpCollisionCheck = false;             // 0.3초 후 점프 컬리전 체크를 끈다.
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()         // 에디터에서만 그리는 기즈모(땅 체크 컬리전)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 0.125f, transform.position.z), 0.25f);
    }

#endif
}
