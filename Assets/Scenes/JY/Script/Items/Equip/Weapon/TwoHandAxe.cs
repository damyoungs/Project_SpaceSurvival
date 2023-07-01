using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandAxe : EquipBase
{
    protected override void Initailize()
    {
        itemType = ItemType.Equip;
        prefabName = ObjectPool.Pool.PrefabName.TwoHandAxe;
    }
}
