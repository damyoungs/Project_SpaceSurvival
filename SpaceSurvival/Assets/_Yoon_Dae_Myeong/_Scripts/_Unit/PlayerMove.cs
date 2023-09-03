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
    public Animator unitAnimator;
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

		inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
		inputAction.Player.Move.canceled += OnMove;
        //inputClick.Test.Test3.performed += onUnitDie;
       
    }
    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
        //inputClick.Test.Test3.performed -= onUnitDie;
        inputAction.Mouse.MouseClick.performed -= onClick;
        inputAction.Mouse.Disable();
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
			unitAnimator.SetBool(isWalkingHash, true);
		}
		else
		{
			unitAnimator.SetBool(isWalkingHash, false);
		}
	}




}
