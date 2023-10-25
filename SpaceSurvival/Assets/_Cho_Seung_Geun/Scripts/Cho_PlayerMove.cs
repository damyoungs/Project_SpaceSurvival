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

    public float speed = 0.0f;                          // ���� �ӵ�
    public float walkSpeed = 5.0f;                      // �ȴ� �ӵ�
    public float runSpeed = 8.0f;                       // �޸��� �ӵ�
    public float jumpHeight = 8.0f;                     // ���� ����
    public float rotateSensitiveX = 30.0f;              // ���콺 �ΰ��� ������
    public float rotateSensitiveY = 30.0f;              // ���콺 �ΰ��� ������
    public float gravity = 15.0f;                       // �߷� ��ġ
    public float cameraCulling = 1.0f;                  // ī�޶� �ø� ��ġ
    public CinemachineVirtualCamera virtualCamera;      // ��Ż �Ѿ �� ����� ���� ī�޶�

    Vector3 moveDir = Vector3.zero;                     // �����̴� ����
    float mouseCurrentRotateY = 0.0f;                   // ���콺�� ���Ʒ��� �����̴� ���� ����
    float onJumpSpeedSwitch = 0.0f;   // ���߿��� �뽬�� ������ �� ��Ƴ��� ���� ���ǵ� ����(���� �뽬�� ������ �������� �ٷ� �ٴ� ���·� ��ȯ)

    bool jumpCollisionCheck = false;                    // ���� �پ��ִ��� üũ�ϴ� �ø��� ������ ����� ����(���� �� �ٷ� üũ�Ǵ� �� ����)
    int jumpCount = 0;                                  // ���� Ƚ��(0�̸� ���� �ִ� ��)

    bool onPressMove = false;                           // ���� Ű�� ���� ��������
    bool onPressDash = false;                           // �뽬 Ű�� ���� ��������

    public PlayerState state = PlayerState.Idle;
    PlayerState State
    {
        get => state;
        set
        {
            if (state != value)                                                 // ���°� �ٸ� ���� ����
            {
                state = value;
                if (state == PlayerState.Walk || state == PlayerState.Run)      // �Ȱų� �ٴ� ���̸� �Ҹ� ���
                {
                    audios[(int)AudioIndex.Walk].Play();
                }
                else                                                            // �� �� �Ҹ� ����
                {
                    audios[(int)AudioIndex.Walk].Stop();
                }
            }
        }
    }

    InputKeyMouse inputActions;
    public InputKeyMouse InputActions => inputActions;
    Animator animator;
    Transform cameraRoot;                                               // ī�޶� ��� ���(ȸ���� �� ����)
    CharacterController controller;
    public CharacterController Controller => controller;

    CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera Cinemachine => cinemachine;
    public AudioSource[] audios;

    // �ؽ� �̸� ĳ���س���
    readonly int Hash_InputX = Animator.StringToHash("InputX");
    readonly int Hash_InputY = Animator.StringToHash("InputY");
    readonly int Hash_IsRun = Animator.StringToHash("IsRun");
    readonly int Hash_IsJumping = Animator.StringToHash("IsJumping");

    public Action interaction;                                          // ��ȣ�ۿ뿡 ���� ��������Ʈ

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        animator = GetComponent<Animator>();
        cameraRoot = transform.GetChild(21);
        controller = GetComponent<CharacterController>();
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;                       // Ŀ�� ����
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
        if (!IsGrounded())                                          // ���� �ִ� �� �ƴϸ� ��� �߷� ����
        {
            moveDir.y -= gravity * Time.deltaTime;
        }

        controller.Move(Time.deltaTime * speed * transform.TransformDirection(new Vector3(moveDir.x, 0.0f, moveDir.z)));    // x, z�� �̵�
        controller.Move(Time.deltaTime * new Vector3(0.0f, moveDir.y, 0.0f));                                               // y�� �̵�
    }

    private void LateUpdate()
    {
        // ī�޶��� �Ÿ��� ���� ���ϸ� �÷��̾� ���� �� ��
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

        moveDir.x = dir.x;                                  // ���� �̵��� x, y ��
        moveDir.z = dir.y;

        animator.SetFloat(Hash_InputX, dir.x);              // ���� Ʈ�� �Ķ���Ϳ� x, y �־��ֱ�(�ִϸ��̼� ����)
        animator.SetFloat(Hash_InputY, dir.y);

        if (context.performed)                              // �̵�Ű�� ����������
        {
            onPressMove = true;                                 // ���갡 ���� �ִٰ� ǥ���ϰ�
            if (speed > runSpeed - 0.01f)                   // ���� �ӵ��� �޸��� �ӵ� �̻��̸�
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Run;                    // ���� ���°� �ƴ� �� �޸��� ���·� ����(���� ������ �� �޸��� �������� ���ϴ� ���� ���� ����)
                }
                animator.SetBool(Hash_IsRun, true);             // �ȴ� ���� Ʈ������ �ٴ� ���� Ʈ���� ����
            }
            else if (speed > walkSpeed - 0.01f)             // ���� �ӵ��� �ȴ� �ӵ���
            {
                if (State != PlayerState.Jump)                  
                {
                    State = PlayerState.Walk;                   // ���� ���°� �ƴ� �� �ȴ� ���·� ����(���� ������ �� �ȴ� �������� ���ϴ� ���� ���� ����)
                }
                animator.SetBool(Hash_IsRun, false);            // �޸��� ���� Ʈ������ �ȴ� ���� Ʈ���� ����
            }
        }
        else                                                // �̵�Ű�� �� ���� ������
        {
            onPressMove = false;                                // ��ũ�� �� ���� �ִٰ� false�� �ٲ��
            State = PlayerState.Idle;                           // ���� ���� Idle�� ����
        }

    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.canceled)                   // �뽬 Ű�� ������
        {
            onPressDash = false;                        // �뽬 Ű�� �� ��������
            animator.SetBool(Hash_IsRun, false);        // �ٴ� ���� Ʈ������ �ȴ� ���� Ʈ���� ����
            if (State != PlayerState.Jump)              
            {
                speed = walkSpeed;                  // ���� ���°� �ƴϸ� ���� ���ǵ�� �ȴ� ���ǵ�� �ȴ�.
            }
            //else
            //{
            //    onJumpSpeedSwitch = walkSpeed;      // ���� ���¸� �ٸ� ������ �ȴ� ���ǵ带 �����صд�.
            //}
                onJumpSpeedSwitch = walkSpeed;      // ���� ���¸� �ٸ� ������ �ȴ� ���ǵ带 �����صд�.

            if (jumpCount == 0)                     // ���� �پ����� ���
            {
                if (State == PlayerState.Run)           // �ٴ� ������ ���(�����ó� ��� ������ �� ����Ǹ� �߰��� �Ҹ��� ����ȴ� - ������Ƽ)
                {
                    audios[(int)AudioIndex.Walk].pitch = 1.26f;         // �߰��� ��� �ð��� 1.26�谡 �ȴ�.(�ִϸ��̼� �ȴ� ���ۿ� ����)
                    State = PlayerState.Walk;                           // �ȴ� ���·� ����
                    //animator.SetFloat(Hash_Speed, animatorWalkSpeed);
                }
            }
        }
        else                                    // �뽬 Ű�� ��������
        {
            onPressDash = true;                     // �뽬 Ű�� ��������
            animator.SetBool(Hash_IsRun, true);     // �ȴ� ���� Ʈ������ �ٴ� ���� Ʈ���� ����
            if (State != PlayerState.Jump)
            {
                speed = runSpeed;                   // ���� ���°� �ƴϸ� ���� ���ǵ�� �ٴ� ���ǵ�� �ȴ�.
            }
            //else
            //{
            //    onJumpSpeedSwitch = runSpeed;       // ���� ���¸� �ٸ� ������ �ٴ� ���ǵ带 �����صд�.
            //}
                onJumpSpeedSwitch = runSpeed;       // ���� ���¸� �ٸ� ������ �ٴ� ���ǵ带 �����صд�.

            if (jumpCount == 0)                     // ���� �پ����� ���
            {
                if (State == PlayerState.Walk)          // �ٴ� ������ ���(�����ó� ��� ������ �� ����Ǹ� �߰��� �Ҹ��� ����ȴ� - ������Ƽ)
                {
                    audios[(int)AudioIndex.Walk].pitch = 2.0f;      // �߰��� ��� �ð��� 2�谡 �ȴ�.(�ִϸ��̼� �ȴ� ���ۿ� ����)
                    State = PlayerState.Run;                        // �ٴ� ���·� ����
                    //animator.SetFloat(Hash_Speed, animatorRunSpeed);
                }
            }
        }
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (jumpCount < 2)                      // ���� Ƚ���� �ִ�ġ(�� ������Ʈ���� 2)�� ���� ���� ���
        {
            moveDir.y = jumpHeight;             // �÷��̾� ���� �̵�����
            //if (jumpCount == 0)
            //{
            //    jumpCheckHeight = transform.position.y + controller.radius * 2;
            //}
            animator.SetBool(Hash_IsJumping, true);     // �ִϸ��̼� ����
            jumpCount++;                                // ���� Ƚ�� +1
            jumpCollisionCheck = true;              // �����ø���(���� �浹���ִ��� üũ�ϴ� ��)�� Ȱ��ȭ�Ǿ� ������ �ٷ� ���� ���·� �ǵ����� �� ����
            StartCoroutine(JumpCollisionTime());        // ���� �ø��� üũ�ϴ� �ڷ�ƾ ����
        }
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        interaction?.Invoke();                          // ��ȣ�ۿ� ��������Ʈ ����
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();                        // ���콺�� �� ������ ��� �̵��� �� ��������
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;    // ȸ���� x�� �� ���ϱ�
        transform.Rotate(Vector3.up, rotateX);                              // ĳ���� ȸ�������ֱ�

        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;    // ȸ���� y�� �� ���ϱ�
        mouseCurrentRotateY -= rotateY;                                     // y�� ���� �� ������ ����� ���ش�.
        mouseCurrentRotateY = Mathf.Clamp(mouseCurrentRotateY, -60.0f, 60.0f);      // ȸ���� ���� �ּ�, �ִ� ���� ����
        cameraRoot.rotation = Quaternion.Euler(mouseCurrentRotateY, cameraRoot.eulerAngles.y, cameraRoot.eulerAngles.z);    // ī�޶� ����Ű�� ����� ȸ����Ŵ
    }

    private bool IsGrounded()
    {
        Vector3 groundCheckPosition = new Vector3(transform.position.x, transform.position.y + controller.radius * 0.5f, transform.position.z); // �� üũ�ϴ� �ݶ��̴� ��ġ
        if (Physics.CheckSphere(groundCheckPosition, controller.radius, LayerMask.GetMask("Ground")))   // Ư�� ������ Gound ���̾ ������ = ���� �پ�������
        {
            if (!jumpCollisionCheck)            // ���� �ø����� ����������(�Ǵ� ���� �Ŀ� ��������)
            {
                //if (moveDir.y < jumpHeight)
                //{
                //    moveDir.y = -2.0f;
                //}
                if (State == PlayerState.Jump)      // ���� ������ ��(���߿��� ������ ���)
                {
                    animator.SetBool(Hash_IsJumping, false);            // �ִϸ��̼� ����
                    audios[(int)AudioIndex.JumpLanding].Play();         // ���� �Ҹ� �ѱ�
                    if (!onPressMove)             
                    {
                        State = PlayerState.Idle;       // ���갡 �� �����ִ� ���¸� ��� ���·� �����
                    }
                    else                                // ���갡 ���� ������
                    {
                        if (!onPressDash)               // �뽬�� �� ���� ���
                        {
                            State = PlayerState.Walk;       // �ȴ� ���·� �ٲ۴�
                        }
                        else                            // �뽬�� ���� ������
                        {
                            State = PlayerState.Run;        // �ٴ� ���·� �ٲ۴�.
                        }
                    }
                    speed = onJumpSpeedSwitch;      // ���� �ӵ��� ������ ��Ƶ״� �ӵ��� �����Ų��.
                }

                jumpCollisionCheck = false;         // ���� �ø��� üũ ����
                jumpCount = 0;                      // ���� Ƚ�� 0���� �ʱ�ȭ
                return true;
            }
        }

        State = PlayerState.Jump;                   // ���� �پ��ִ� �� �ƴϸ� ���� �����̴�.

        return false;
    }

    IEnumerator JumpCollisionTime()
    {
        yield return new WaitForSeconds(0.3f);
        jumpCollisionCheck = false;             // 0.3�� �� ���� �ø��� üũ�� ����.
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()         // �����Ϳ����� �׸��� �����(�� üũ �ø���)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 0.125f, transform.position.z), 0.25f);
    }

#endif
}
