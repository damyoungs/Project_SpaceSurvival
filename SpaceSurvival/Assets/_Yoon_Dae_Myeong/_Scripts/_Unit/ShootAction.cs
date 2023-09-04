using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAction : PlayerMove
{
	[SerializeField]
	private Transform bulletProjectilePrefab;
	[SerializeField]
	private Transform shootPointTransform;

	public Action onAttack;
    //무기 바뀔 때마다 shootPointTransform 수정
	//총알 생성을 여기서 하는게 맞는건가?
	//장착까지만
    protected override void onClickRight(InputAction.CallbackContext context)
	{
		onAttack?.Invoke();

        unitAnimator.SetTrigger("IsFiring");
		Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
		audioSource.Play();
	}
}
