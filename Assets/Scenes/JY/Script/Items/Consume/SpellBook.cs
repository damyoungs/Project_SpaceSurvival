using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellBook : ConsumeBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath._SpellBook;
        prefabName = ObjectPool.Pool.PrefabName.SpellBook;
        IsStackable = true;
        Name = name;
        RecoveryMpValue = 50;
    }
}
