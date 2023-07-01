using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumeBase
{
    protected override void Initailize()
    {
        itemType = ItemType.Consume;
        Name = name;
        RecoveryHpValue = 50;
        prefabName = NewObjectPool.Pool.PrefabName.hpPotion;
    }
}
