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
	Quaternion lookDir;

	float moveSpeed = 4.0f;
	float rotateSpeed = 10.0f;
	BoxCollider target = null;
	
	InputKeyMouse inputAction;

	protected AudioSource audioSource;

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
		//inputClick.Test.Test3.performed += onUnitDie;
		
	}

    private void Start()
    {
        mainCamera = Camera.main;
		Move = MoveByKeyBoard ;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
		Vector3 dir = context.ReadValue<Vector3>();
		moveDirection = dir;
		if (dir != Vector3.zero)
		lookDir = Quaternion.LookRotation(dir);
    }

    private void OnDisable()
	{
		//inputClick.Test.Test3.performed -= onUnitDie;
		inputAction.Mouse.MouseClickRight.performed -= onClickRight;
		inputAction.Mouse.MouseClick.performed -= onClick;
		inputAction.Mouse.Disable();
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
		transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.fixedDeltaTime * rotateSpeed);
    }
	private void UnitOnMove()
	{
		if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.1f)
		{
			Vector3 moveDirection = (target.gameObject.transform.position - transform.position).normalized;
			transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
			transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
			//transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
			unitAnimator.SetBool("IsWalking", true);
		}
		else
		{
			unitAnimator.SetBool("IsWalking", false);
		}
	}

	protected virtual void onClickRight(InputAction.CallbackContext context)
	{
	}

	//protected virtual void onUnitDie(InputAction.CallbackContext context)
	//{
	//}
}
