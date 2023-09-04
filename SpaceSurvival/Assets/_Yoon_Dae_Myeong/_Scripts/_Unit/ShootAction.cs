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
    //���� �ٲ� ������ shootPointTransform ����
	//�Ѿ� ������ ���⼭ �ϴ°� �´°ǰ�?
	//����������
    protected override void onClickRight(InputAction.CallbackContext context)
	{
		onAttack?.Invoke();

        unitAnimator.SetTrigger("IsFiring");
		Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
		audioSource.Play();
	}
}
