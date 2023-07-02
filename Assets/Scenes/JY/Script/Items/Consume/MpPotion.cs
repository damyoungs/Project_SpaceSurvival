using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : ConsumeBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath._MpPotion;
        prefabName = ObjectPool.Pool.PrefabName.MpPotion;
        IsStackable = true;
        Name = name;
        RecoveryMpValue = 50;
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
