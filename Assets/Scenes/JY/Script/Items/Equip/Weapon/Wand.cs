using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Wand;
        prefabName = ObjectPool.Pool.PrefabName.Wand;
        IsStackable = false;
        Name = name;
    }
}
