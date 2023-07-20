using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ItemBase
{
  
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath.HpPotion;
        IsStackable = true;
        Name = "HP Potion";
        itemDescription = "���ø� ������ ���� �� �� ���� �����̴�. \n HP�� 50 ȸ����Ų��.";
    }

    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }


}
