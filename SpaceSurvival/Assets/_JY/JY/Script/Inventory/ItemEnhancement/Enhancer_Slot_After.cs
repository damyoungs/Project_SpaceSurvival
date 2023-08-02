using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enhancer_Slot_After : Enhancer_Slot_Base
{
  
    protected override void Refresh()
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