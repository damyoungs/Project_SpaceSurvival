using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPotion : ConsumeBase
{

    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath._SecretPotion;
        prefabName = ObjectPool.Pool.PrefabName.SecretPotion;
        IsStackable = true;
        Name = name;
        RecoveryMpValue = 50;
    }
}
