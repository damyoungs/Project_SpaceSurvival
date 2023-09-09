using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionController : ChildComponentSingeton<InputActionController>
{
    /// <summary>
    /// ��ü������ ��ǲ�ý����� ������ ���� 
    /// </summary>
    InputKeyMouse inputSystem;
    [SerializeField]
    InputAction temp;
    public InputKeyMouse InputSystem => inputSystem;
    /// <summary>
    /// ���� ��Ʈ���� ������ �Ǵ��� �̳Ѱ�
    /// </summary>
    HotKey_Use isHotKey = HotKey_Use.None;

    protected override void Awake()
    {
        base.Awake();
        inputSystem = new();
        inputSystem.Test.Enable();
        inputSystem.Test.Test1.performed += Test1;
        inputSystem.Test.Test2.performed += Test2;
        inputSystem.Player.Enable();
        inputSystem.Player.Move.performed += Test;
        inputSystem.Player.Disable();
        Debug.Log("1");
    }

    public void PlayerControlOpen()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Move.performed += Test;
    }

    private void Test2(InputAction.CallbackContext context)
    {
        Debug.Log("2");
        PlayerControlClose();
    }

    private void Test1(InputAction.CallbackContext context)
    {
        Debug.Log("1");
        PlayerControlOpen();
    }

    private void Test(InputAction.CallbackContext context)
    {
        Debug.Log("��ǲ �ý��� �����ִ� Ŭ���̾�");
    }

    public void PlayerControlClose()
    {
        inputSystem.Player.Disable();
    }

}
