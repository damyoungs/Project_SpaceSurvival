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

	protected override void onClickRight(InputAction.CallbackContext context)
	{
		unitAnimator.SetTrigger("IsFiring");
		Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
	}
}
