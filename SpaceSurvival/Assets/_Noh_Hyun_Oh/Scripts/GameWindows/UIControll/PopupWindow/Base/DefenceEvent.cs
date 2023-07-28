using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ŭ���̺�Ʈ�� �������� ��üȭ������ ��������� ����Ű �̺�Ʈ�� ���� esc�̺�Ʈ�� �߰��Ѵ�.
/// </summary>
public class DefenceEvent: MonoBehaviour
{
    
    InputKeyMouse inputSystem ;
    SaveLoadPopupButton targetWindow;

    private void Awake()
    {
        inputSystem = new InputKeyMouse();//esc�̺�Ʈ �߰��� ��Ʈ�ѷ�
        int index = transform.parent.childCount - 1;
        targetWindow = transform.parent.GetChild(index).GetChild(5).GetComponent<SaveLoadPopupButton>();// esc�������� ó���� ���� process ����Ŭ���� 
    }
    private void OnEnable()
    {
        WindowList.Instance.InputKeyEvent.Disable();// ����Ű ��Ȱ��ȭ
        inputSystem.Enable();
        inputSystem.KeyBoard.System.performed += Close;
    }


    private void OnDisable()
    {

        inputSystem.KeyBoard.System.performed -= Close;
        inputSystem.Disable();

        WindowList.Instance.InputKeyEvent.Enable(); //����Ű Ȱ��ȭ
    }
    /// <summary>
    /// esc �������� �˾�â�ݱ��������
    /// </summary>
    private void Close(InputAction.CallbackContext context)
    {
        targetWindow.CancelButton();//�ݱ�â�����°Ͱ� ���� 
    }
}
