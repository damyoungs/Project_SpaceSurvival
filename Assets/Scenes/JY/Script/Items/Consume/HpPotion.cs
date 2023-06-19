using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumeBase
{
    protected override void InitializeValue()
    {
        ItemType = ItemType.Consume;
        hpValue = 50;
        mpValue = 0;
        darkForceValue = 0;
        fatigueValue = 0;
    }
    private void Start()
    {
        PrintValue();
    }
    void PrintValue()
    {
        Debug.Log($"{ ItemType} \n {hpValue}\n {mpValue}\n {darkForceValue}\n {fatigueValue}");
    }
}
