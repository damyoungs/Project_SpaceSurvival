using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	public Animator unitAnimator;
	Camera mainCamera;
	
	float moveSpeed = 4.0f;
	float rotateSpeed = 10.0f;
	BoxCollider target = null;
    CameraOriginTarget cameraOrigin;
    InputKeyMouse inputClick;

	protected AudioSource audioSource;


	CameraOriginTarget originTarget;

	private void Awake()
	{
		cameraOrigin = FindObjectOfType<CameraOriginTarget>(true);
		inputClick = new InputKeyMouse();
		audioSource = GetComponent<AudioSource>();
	}
	
	private void OnEnable()
	{
		inputClick.Mouse.Enable();
		inputClick.Mouse.MouseClick.performed += onClick;
		inputClick.Mouse.MouseClickRight.performed += onClickRight;
        //inputClick.Test.Test3.performed += onUnitDie;

        originTarget = FindObjectOfType<CameraOriginTarget>(true);
    }

	private void OnDisable()
	{
		//inputClick.Test.Test3.performed -= onUnitDie;
		inputClick.Mouse.MouseClickRight.performed -= onClickRight;
		inputClick.Mouse.MouseClick.performed -= onClick;
		inputClick.Mouse.Disable();
	}

	
	private void onClick(InputAction.CallbackContext context)
	{
		if (mainCamera == null) return;
		Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());     
		Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);           

		if (Physics.Raycast(ray, out RaycastHit hitInfo))                       
		{
			if (hitInfo.transform.gameObject.CompareTag("Tile"))              
			{
				target = (BoxCollider)hitInfo.collider;
                Tile tile = target.gameObject.GetComponent<Tile>();            
				originTarget.onCameraOriginMove?.Invoke(tile.transform);
				
				
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
			//transform.parent.position = transform.position;
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
