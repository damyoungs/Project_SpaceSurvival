using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer_Slot_Before : Enhancer_Slot_Base
{ 
    private void Start()
    {
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
           itemIcon.sprite = ItemData.itemIcon;      // �����ܿ� �̹��� ����
           itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
       }
    }
}
