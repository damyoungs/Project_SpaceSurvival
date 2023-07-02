using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumeBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath._HpPotion;
        Name = name;
        RecoveryHpValue = 50;
        prefabName = ObjectPool.Pool.PrefabName.HpPotion;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }

    
}
