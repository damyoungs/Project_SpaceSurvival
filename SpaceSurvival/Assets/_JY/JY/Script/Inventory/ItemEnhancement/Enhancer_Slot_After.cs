using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enhancer_Slot_After : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;


    public Action onValueChange;
    public bool IsEmpty => ItemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ// �ʱ� 
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager��  GetItem �Լ��� ����ɶ� Item�� ������ �޾ƿ������� ������Ƽ
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                onValueChange?.Invoke();
            }
        }
    }
    private void Awake()
    {
        // ��ӹ��� Ŭ�������� �߰����� �ʱ�ȭ�� �ʿ��ϱ� ������ �����Լ��� ����
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        onValueChange = Refresh;
        Refresh();
    }

    /// <summary>
    /// ���� �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="slot">�� UI�� ������ ����</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // ���� ����
        onValueChange = Refresh;   // ���Կ� ��ȭ�� ���� �� ����� �Լ� ���
        ItemData = null;
        Refresh();                              // �ʱ� ��� ����
    }

    /// <summary>
    /// ������ ���̴� ����� �����ϴ� �Լ�
    /// </summary>
    protected virtual void Refresh()
    {
        if (IsEmpty)
        {
            // ���������
            itemIcon.color = Color.clear; 
            itemIcon.sprite = null;         
        }
        else
        {
            // �������� ���������
            itemIcon.sprite = ItemData.itemIcon;      // �����ܿ� �̹��� ����
            itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
        }

        OnRefresh();        // ��ӹ��� Ŭ�������� ������ �����ϰ� ���� �ڵ� ����
    }

    protected virtual void OnRefresh()
    {
    }
}