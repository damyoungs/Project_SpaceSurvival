using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionTest : TestBase
{
    [SerializeField]
    [Range(0.0f,1.0f)]
    float test = 0.0f;
    [SerializeField]
    float max = 0.0f;
    [SerializeField]
    float now = 0.0f;
    TrackingBattleUI testUI;
    Action action;
    event Action testAction
    {
        add => testAction += value;         //�̺�Ʈ �Լ��� += -= �θ� ��� �ȴٰ� �Ѵ�. 
        remove => testAction -= value;      //��ǲ �ý��� �� �̺�Ʈ�Լ��� ����ؼ� += -= �θ� ��밡���ϴ�.
    }
    InputKeyMouse keyTest;
    protected override void Awake()
    {
        base.Awake();   
        keyTest = new();
        keyTest.Test.Enable();
        keyTest.Player.Enable();
        testUI = FindObjectOfType<TrackingBattleUI>();
    }

    private void OnMouseDown()
    {
        //action += () => { Debug.Log("�ߺ� ��� Ȯ�ο�"); };                    //�ߺ����� ��ϵȴ�.
        //action += ActionTestFunc;                                              //�ߺ����� ��ϵȴ�.
        //testAction += () => { Debug.Log("�̺�Ʈ �׼� �ߺ���� Ȯ�ο�"); };     //�ߺ� ��ϵȴ�       
        //testAction += ActionEventTestFunc;                                     //�ߺ� ��ϵȴ�
        keyTest.Test.Test1.performed += inputTest;                               //�ߺ� ��� �ȵȴ�
        keyTest.Test.Test1.performed += (_) =>                                   //�ߺ� ��� �ȵȴ�
        {
            Debug.Log("��ǲ �ý��� �ߺ� ��� �׽�Ʈ ���ٽ� ");
        };
        keyTest.Player.Move.performed += (_) => { Debug.Log($"{_} : �׽�Ʈ"); };   // �����Լ��� �ɹ��Լ��� ���� �Լ��� ����� �ѹ����ȴ� 
        keyTest.Player.Move.performed += (_) => { Debug.Log($"{_} : ����"); };     // �̰� �̺�Ʈ�ý����� �⺻��Ŀ���� �̶���Ѵ�.
                                                                                    // �ش系�� �ҽ��ڵ� ã�ƺ������ߴ��� �����̾ȵȴ� ���ְ�緣��.. 
        //inputSystem.Player.Enable(); //Enable �Լ��� enabled������ üũ�� ���Ѵ�.. �׷��� ���⼭ üũ�� ���༭ �ʱ�ȭ��������������  2021.3.25f1 ���� ����                                                            
        //inputSystem.Player.Disable(); //Disable �Լ��� enable��(Ȱ��ȭ����Ȯ�ο� ������Ƽ)�� false �� �ƹ��۵������ϰ� �����Ѵ�.  2021.3.25f1 ���� ����
    }


    private void OnMouseUp()
    {
        //action.Invoke();   
        //testAction.Invoke();
    }
    private void ActionTestFunc()
    {
        Debug.Log("�ߺ��̳�?");
    }
    private void ActionEventTestFunc()
    {
        Debug.Log("�̺�Ʈ �ߺ��̳�?");
    }
    private void inputTest(InputAction.CallbackContext context)
    {
        Debug.Log("��ǲ�ý��� �ߺ� ��� �׽�Ʈ ");
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        testUI.hpGaugeSetting(now,max);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        testUI.stmGaugeSetting(now,max);

    }
}
