using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAction : PlayerMove
{


	public Action onAttack;
    //���� �ٲ� ������ shootPointTransform ����
	//�Ѿ� ������ ���⼭ �ϴ°� �´°ǰ�?
	//����������
    protected override void onClickRight(InputAction.CallbackContext context)
	{
		onAttack?.Invoke();


	}
}
