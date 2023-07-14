using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumeBase
{
  
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath.HpPotion;
        IsStackable = true;
        Name = "HP Potion";
        itemDescription = "마시면 조금은 힘이 날 것 같은 포션이다. \n HP를 50 회복시킨다.";
    }

    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }


}
