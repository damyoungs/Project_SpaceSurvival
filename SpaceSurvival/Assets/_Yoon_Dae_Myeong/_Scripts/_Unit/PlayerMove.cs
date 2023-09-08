using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerMove : MonoBehaviour
{
	int isWalkingHash = Animator.StringToHash("IsWalking");
	int runHash = Animator.StringToHash("Run");
	int jumpHash = Animator.StringToHash("Jump");

	enum MoveState
	{
		Town,
		Field
	}
	MoveState state = MoveState.Field;
	MoveState State
	{
		get => state;
		set
		{
			if (state != value)
			{
				state = value;
				switch (state)
				{
					case MoveState.Town:
						Move = MoveByKeyBoard;
						Debug.Log("�̵���� ���� - KeyBoard");
						break;
					case MoveState.Field:
						Move = UnitOnMove;
                        Debug.Log("�̵���� ���� - ���콺 Ŭ��");
                        break;
					default:
						break;
				}
			}
		}
	}
	Action Move;
	 Animator anim;
	Camera mainCamera;

	Vector3 moveDirection;
	Vector3 fixedPos;
	/// <summary>
	/// �ܺο��� �����Ұ�
	/// </summary>
	Quaternion lookDir = Quaternion.identity;
	/// <summary>
	/// �̵��� �� 
	/// </summary>
	Quaternion moveDir = Quaternion.identity;

	public float defaultMoveSpeed = 2.0f;
	public float runSpeed = 5.0f;

	bool running = false;
	bool Running
	{
		get => running;
		set
		{
			running = value;
			if (running)
			{
				moveSpeed = runSpeed;
			}
			else
			{
				moveSpeed = defaultMoveSpeed;
			}
		}
	}

	float moveSpeed = 2.0f;
	float rotateSpeed = 10.0f;
	BoxCollider target = null;
	
	InputKeyMouse inputAction;
	Rigidbody rb;

	WaitForSeconds jump_Duration;

	private void Awake()
	{
		inputAction = new InputKeyMouse();
		anim = GetComponent<Animator>();
		jump_Duration = new WaitForSeconds(0.9f);
		rb = GetComponent<Rigidbody>();
    }
	private void OnEnable()
	{
		inputAction.Mouse.Enable();
		inputAction.Mouse.MouseClick.performed += onClick;

		inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
		inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveMode_Change.performed += On_MoveMode_Change;
        inputAction.Player.Run.performed += Run;
        inputAction.Player.Jump.performed += Jump;
        //inputClick.Test.Test3.performed += onUnitDie;
        CameraOriginTarget battleFollowCamera = FindObjectOfType<CameraOriginTarget>(true); //ȸ���� �޾ƿ������� ã�� 
        if (battleFollowCamera != null)
        {
            battleFollowCamera.cameraRotation += SetCameraRotaion; //ȸ�����޾ƿ������� ����
        }
    }

    private void Jump(InputAction.CallbackContext _)
    {
		anim.SetTrigger(jumpHash);
		rb.AddForce(5 * transform.up, ForceMode.Impulse);
		//StartCoroutine(ResetPos_Y());
    }
	IEnumerator ResetPos_Y()
	{
		yield return jump_Duration;
		fixedPos = transform.position;
		fixedPos.y = 0;
		transform.position = fixedPos;
    }
    private void Run(InputAction.CallbackContext _)
    {
		Running = !Running;
    }

    private void On_MoveMode_Change(InputAction.CallbackContext _)
    {
		switch (state)
		{
			case MoveState.Town:
				State = MoveState.Field;
				break;
			case MoveState.Field:
				State = MoveState.Town;
				break;
		}
    }

    private void OnDisable()
    {
        CameraOriginTarget battleFollowCamera = FindObjectOfType<CameraOriginTarget>(true); //ȸ���� �޾ƿ������� ã��
        if (battleFollowCamera != null)
        {
            battleFollowCamera.cameraRotation -= SetCameraRotaion;//ȸ�����޾ƿ������� ����
        }
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
        //inputClick.Test.Test3.performed -= onUnitDie;
        inputAction.Mouse.MouseClick.performed -= onClick;
        inputAction.Mouse.Disable();
    }
    /// <summary>
    /// ��Ʋ�ʿ��� ī�޶� ���ư��� ĳ���� ���⵵ ���� �����Ѵ�.
    /// </summary>
    /// <param name="quaternion">ī�޶� ȸ������ </param>
    private void SetCameraRotaion(Quaternion quaternion)
    {
		lookDir = quaternion;
		Debug.Log($"ī�޶� �������� ����: {quaternion}");
    }

    private void Start()
    {
        mainCamera = Camera.main;
		Move = MoveByKeyBoard ;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
		Vector3 dir = context.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0.0f;
		moveDirection = lookDir * dir; //�̵����⼳��
		
		if (!context.canceled)
		{
            moveDir = Quaternion.LookRotation(lookDir * dir); //ī�޶� ���⿡ ���缭 ������ �����Ѵ�.
            if (running)
			{
				anim.SetBool(runHash, true);
			}
			else
			{
                anim.SetBool(isWalkingHash, true);
            }
        }
		else
		{
            anim.SetBool(isWalkingHash, false);
			anim.SetBool(runHash, false);
        }

    }




    private void onClick(InputAction.CallbackContext context)
	{
		Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());     
		Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);           

		if (Physics.Raycast(ray, out RaycastHit hitInfo))                       
		{
			if (hitInfo.transform.gameObject.CompareTag("Tile"))              
			{
				target = (BoxCollider)hitInfo.collider;                     
				Tile tile = target.gameObject.GetComponent<Tile>();            
				
			}
		}
	}


	private void FixedUpdate()
	{
		//UnitOnMove();
		Move();
	}
	void MoveByKeyBoard()//update ȣ��
	{
        transform.Translate(Time.fixedDeltaTime * moveSpeed * moveDirection, Space.World);
		transform.rotation = Quaternion.Slerp(transform.rotation, moveDir, Time.fixedDeltaTime * rotateSpeed);
		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetObj.position - transform.position), Time.fixedDeltaTime * rotateSpeed);


	}

	private void UnitOnMove()
	{
		if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.1f)
		{
			Vector3 moveDirection = (target.gameObject.transform.position - transform.position).normalized;
			transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
			transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
			//transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
			anim.SetBool(isWalkingHash, true);
		}
		else
		{
			anim.SetBool(isWalkingHash, false);
		}
	}
}
