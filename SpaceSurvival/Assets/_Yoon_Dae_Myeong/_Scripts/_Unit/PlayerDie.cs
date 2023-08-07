using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDie : TestBase
{
	//protected override void onUnitDie(InputAction.CallbackContext context)
	//{
	//	unitAnimator.SetTrigger("IsDead");
	//}
	[SerializeField]
	private Animator unitAnimator;

	protected override void Test5(InputAction.CallbackContext context)
	{
		unitAnimator.SetTrigger("IsDead");
	}

	protected override void Test6(InputAction.CallbackContext context)
	{
		unitAnimator.SetTrigger("IsHit");
	}
}
