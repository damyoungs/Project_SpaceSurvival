using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlot : Slot
{
    /// <summary>
    /// �� �κ��丮�� ���� �÷��̾�(������ ��� ������ �ʿ�)
    /// </summary>
    //Player owner;

    /// <summary>
    /// �ӽ� ������ ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        // �ӽ� ������ ��κ� ���� ���� �Ŷ� �δ��� ����
        transform.position = Mouse.current.position.ReadValue();    // �ӽ� ������ ���콺 ��ġ�� ���� ������
    }

    /// <summary>
    /// �ӽ� ���� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="slot">�� �ӽ� ���԰� ����� �κ� ����</param>
    private void Start()
    {
        Close();
        Refresh();
    }

    /// <summary>
    /// �ӽ� ������ ���� �Լ�
    /// </summary>
    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();    // ��ġ�� ���콺 ��ġ�� ����
        onTempSlotOpenClose?.Invoke(true);                          // ���ȴٰ� ��ȣ ������
        gameObject.SetActive(true);                                 // Ȱ��ȭ ��Ű��(���̰� �����)
    }

    /// <summary>
    /// �ӽ� ������ �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // �����ٰ� ��ȣ ������
        gameObject.SetActive(false);            // ��Ȱ��ȭ ��Ű��(�Ⱥ��̰� �����)
    }

    /// <summary>
    /// �ٴڿ� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="screenPos">���콺 Ŀ���� ��ũ�� ��ǥ</param>
    public void OnDrop(Vector2 screenPos)
    {
    }
}