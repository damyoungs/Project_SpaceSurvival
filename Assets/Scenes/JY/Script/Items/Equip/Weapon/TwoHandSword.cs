using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._TwoHandSword;
        prefabName = ObjectPool.Pool.PrefabName.TwoHandSword;
        IsStackable = false;
        Name = name;
    }
}
