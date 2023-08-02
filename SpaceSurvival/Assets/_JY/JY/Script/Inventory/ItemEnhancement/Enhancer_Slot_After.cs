using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enhancer_Slot_After : Enhancer_Slot_Base
{
    private void Start()
    {
        //GameManager.SlotManager.setEnhanceItem += (itemData) => ItemData = itemData;
        GameManager.Item_Enhancer.onSetItem += Refresh;
    }
    void Refresh(ItemData data)
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
           itemIcon.sprite = data.itemIcon;      // �����ܿ� �̹��� ����
           itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
       }
    }
}