using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : ConsumeBase
{
    protected override void Initailize()
    {
        itemType = ItemType.Consume;
        Name = name;
        RecoveryMpValue = 50;
        prefabName = ObjectPool.Pool.PrefabName.mpPotion;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }

}
