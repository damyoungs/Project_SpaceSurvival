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
        prefabName = ObjectPool.Pool.PrefabName.HpPotion;
        IsStackable = true;
        Name = name;

        RecoveryHpValue = 50;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }

    protected override void Start()
    {
        base.Start();
        GameManager.SlotManager.GetItem(this);
    }
}
