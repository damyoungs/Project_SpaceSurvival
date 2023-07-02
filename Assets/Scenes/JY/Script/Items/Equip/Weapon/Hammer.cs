using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hammer : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Hammer;
        prefabName = ObjectPool.Pool.PrefabName.Hammer;
        IsStackable = false;
        Name = name;
    }
}
