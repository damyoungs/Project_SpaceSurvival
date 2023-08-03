using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enhancer_Slot_Base : MonoBehaviour
{
    protected Item_Enhancer item_Enhancer;
    protected Image itemIcon;
    public Action onValueChange;
    public bool IsEmpty => ItemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ// �ʱ� 
    private ItemData_Enhancable itemData = null;
    public ItemData_Enhancable ItemData//SlotManager��  GetItem �Լ��� ����ɶ� Item�� ������ �޾ƿ������� ������Ƽ
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
     //   Refresh();
    }
    protected virtual void Start()
    {
        item_Enhancer = GameManager.Item_Enhancer;
        item_Enhancer.onSetItem += (itemData) => ItemData = itemData;
        Refresh();
    }

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
    }
}