using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Pistol;
        prefabName = ObjectPool.Pool.PrefabName.Pistol;
        IsStackable = false;
        Name = name;
    }
}
