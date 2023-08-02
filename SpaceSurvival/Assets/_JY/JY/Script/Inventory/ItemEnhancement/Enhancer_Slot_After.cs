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
           // 비어있으면
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
       }
       else
       {
           // 아이템이 들어있으면
           itemIcon.sprite = ItemData.itemIcon;      // 아이콘에 이미지 설정
           itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
       }
    }
}