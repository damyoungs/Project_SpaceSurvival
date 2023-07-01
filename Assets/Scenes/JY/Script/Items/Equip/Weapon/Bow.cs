using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : EquipBase
{
    protected override void Initailize()
    {
        itemType = ItemType.Equip;
        prefabName = ObjectPool.Pool.PrefabName.Bow;
    }
}
