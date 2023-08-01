using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer_Slot_Before : Enhancer_Slot_Base
{ 
    private void Start()
    {
        GameManager.SlotManager.setEnhanceItem += (itemData) => ItemData = itemData;
    }
}
