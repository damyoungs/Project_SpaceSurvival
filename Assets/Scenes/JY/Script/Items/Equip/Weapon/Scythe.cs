using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scythe : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Scythe;
        prefabName = ObjectPool.Pool.PrefabName.Scythe;
        IsStackable = false;
        Name = name;
    }
}
