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
        GameManager.SlotManager.setEnhanceItem += (itemData) => ItemData = itemData;
    }
    protected override void Refresh()
    {
        //UI에 보여지는 부분 업데이트
        //EnhancerDetail 업데이트
        base.Refresh();
    }
}