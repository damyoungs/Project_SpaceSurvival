using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private Animator unitAnimator;
	Camera mainCamera;
	
	float moveSpeed = 4.0f;
	float rotateSpeed = 10.0f;
	BoxCollider target = null;
	
	InputKeyMouse inputClick;

	private void Awake()
	{
		inputClick = new InputKeyMouse();
	}
	private void OnEnable()
	{
		inputClick.Mouse.Enable();
		inputClick.Mouse.MouseClick.performed += onClick;
	}

	private void OnDisable()
	{
		inputClick.Mouse.MouseClick.performed -= onClick;
		inputClick.Mouse.Disable();
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
				//Debug.Log($"Ÿ�� ��ġ : {tile.Width}, {tile.Length}");
			}
		}
	}

	private void Start()
	{
		mainCamera = Camera.main;         
	}

	private void FixedUpdate()
	{
		UnitOnMove();
		
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
}
