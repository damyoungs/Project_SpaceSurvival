using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;

    TextMeshProUGUI itemCountText;

    protected virtual void Awake()
    {
        // ��ӹ��� Ŭ�������� �߰����� �ʱ�ȭ�� �ʿ��ϱ� ������ �����Լ��� ����
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCountText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ���� �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="slot">�� UI�� ������ ����</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // ���� ����
        slot.onItemChange = Refresh;   // ���Կ� ��ȭ�� ���� �� ����� �Լ� ���
        Refresh();                              // �ʱ� ��� ����
    }

    /// <summary>
    /// ������ ���̴� ����� �����ϴ� �Լ�
    /// </summary>
    protected virtual void Refresh()
    {
        if (slotComp.IsEmpty)
        {
            // ���������
            itemIcon.color = Color.clear;   // ������ �Ⱥ��̰� ����ȭ
            itemIcon.sprite = null;         // �����ܿ��� �̹��� ����
            itemCountText.text = string.Empty;  // ������ �Ⱥ��̰� ���� ����
        }
        else
        {
            // �������� ���������
            itemIcon.sprite = slotComp.ItemData.itemIcon;      // �����ܿ� �̹��� ����
            itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
            itemCountText.text = slotComp.ItemCount.ToString();    // ������ ���� ����
        }

        OnRefresh();        // ��ӹ��� Ŭ�������� ������ �����ϰ� ���� �ڵ� ����
    }

    /// <summary>
    /// ��ӹ��� Ŭ�������� ���������� �����ϰ� ���� �ڵ带 ��Ƴ��� �Լ�
    /// </summary>
    protected virtual void OnRefresh()
    {
    }
}