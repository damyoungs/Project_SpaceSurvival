using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandAxe : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._TwoHandAxe;
        prefabName = ObjectPool.Pool.PrefabName.TwoHandAxe;
        IsStackable = false;
        Name = name;
    }
}
