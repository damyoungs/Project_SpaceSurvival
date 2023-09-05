using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAction : MonoBehaviour
{
	[SerializeField]
	private Transform bulletProjectilePrefab;
	[SerializeField]
	private Transform shootPointTransform;
    Animator unitAnimator;
    AudioSource audioSource;
    InputKeyMouse inputAction;

    public Action on_Attack;
    private void Awake()
    {
        inputAction = new();
        audioSource = GetComponent<AudioSource>();
        unitAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        inputAction.Mouse.Enable();
        inputAction.Mouse.MouseClickRight.performed += onClickRight;
    }
    private void OnDisable()
    {
        inputAction.Mouse.MouseClickRight.performed += onClickRight;
        inputAction.Mouse.Disable();
    }

    void onClickRight(InputAction.CallbackContext context)
	{
        on_Attack?.Invoke();

        unitAnimator.SetTrigger("IsFiring");
		Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
		audioSource.Play();
	}
}
