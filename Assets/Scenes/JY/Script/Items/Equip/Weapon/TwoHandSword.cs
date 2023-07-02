using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        prefabName = ObjectPool.Pool.PrefabName.TwoHandSword;
    }
}
