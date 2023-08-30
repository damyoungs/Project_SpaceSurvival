using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	int isWalkingHash = Animator.StringToHash("IsWalking");
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
						break;
					case MoveState.Field:
						Move = UnitOnMove;
                        break;
					default:
						break;
				}
			}
		}
	}
	Action Move;
	public Animator unitAnimator;
	Camera mainCamera;

	Vector3 moveDirection;
	/// <summary>
	/// 외부에서 수정할값
	/// </summary>
	Quaternion lookDir = Quaternion.identity;
	/// <summary>
	/// 이동할 값 
	/// </summary>
	Quaternion moveDir = Quaternion.identity;
	[SerializeField]
	float defaultMoveSpeed = 4.0f;
	float moveSpeed = 4.0f;
	float rotateSpeed = 10.0f;
	BoxCollider target = null;
	
	InputKeyMouse inputAction;

	protected AudioSource audioSource;

	public Transform targetObj;

	private void Awake()
	{
		inputAction = new InputKeyMouse();
		audioSource = GetComponent<AudioSource>();
	}
	private void OnEnable()
	{
		inputAction.Mouse.Enable();
		inputAction.Mouse.MouseClick.performed += onClick;
		inputAction.Mouse.MouseClickRight.performed += onClickRight;

		inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
		inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveMode_Change.performed += On_MoveMode_Change;
        //inputClick.Test.Test3.performed += onUnitDie;
        CameraOriginTarget battleFollowCamer = FindObjectOfType<CameraOriginTarget>(true); //회전값 받아오기위해 찾기 
        if (battleFollowCamer != null)
        {
           // battleFollowCamer.cameraRotation += SetCameraRotaion; //회전값받아오기위해 연결
        }
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
			default:
				break;
		}
    }

    private void OnDisable()
    {
        CameraOriginTarget battleFollowCamer = FindObjectOfType<CameraOriginTarget>(true); //회전값 받아오기위해 찾기
        if (battleFollowCamer != null)
        {
           // battleFollowCamer.cameraRotation -= SetCameraRotaion;//회전값받아오기위해 연결
        }
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
        //inputClick.Test.Test3.performed -= onUnitDie;
        inputAction.Mouse.MouseClickRight.performed -= onClickRight;
        inputAction.Mouse.MouseClick.performed -= onClick;
        inputAction.Mouse.Disable();
    }
    /// <summary>
    /// 배틀맵에서 카메라 돌아가면 캐릭터 방향도 같이 수정한다.
    /// </summary>
    /// <param name="quaternion">카메라 회전방향 </param>
    private void SetCameraRotaion(Quaternion quaternion)
    {
		lookDir = quaternion;
		Debug.Log($"카메라가 움직였네 값은: {quaternion}");
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
		if (!context.canceled)
		{
			moveDirection = lookDir * dir; //이동방향설정
			//Debug.Log(moveDirection);
			moveSpeed = defaultMoveSpeed; //이동속도 설정
            moveDir = Quaternion.LookRotation(lookDir * dir); //카메라 방향에 맞춰서 방향을 결정한다.
			//Debug.Log(lookDir);
            unitAnimator.SetBool(isWalkingHash, true);
        }
		else
		{
			moveSpeed = 0.0f;
            unitAnimator.SetBool(isWalkingHash, false);
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
	void MoveByKeyBoard()
	{
        transform.Translate(Time.fixedDeltaTime * moveSpeed * moveDirection, Space.World);
		transform.rotation = Quaternion.Slerp(transform.rotation, moveDir, Time.fixedDeltaTime * rotateSpeed);
		//transform.rotation = Quaternion.Slerp(transform.rotation,  Quaternion.LookRotation(targetObj.position - transform.position), Time.fixedDeltaTime * rotateSpeed);


	}
	private void UnitOnMove()
	{
		if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.1f)
		{
			Vector3 moveDirection = (target.gameObject.transform.position - transform.position).normalized;
			transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
			transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
			//transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
			unitAnimator.SetBool(isWalkingHash, true);
		}
		else
		{
			unitAnimator.SetBool(isWalkingHash, false);
		}
	}

	protected virtual void onClickRight(InputAction.CallbackContext context)
	{
	}

	//protected virtual void onUnitDie(InputAction.CallbackContext context)
	//{
	//}









	CameraOriginTarget followCamera;


}
