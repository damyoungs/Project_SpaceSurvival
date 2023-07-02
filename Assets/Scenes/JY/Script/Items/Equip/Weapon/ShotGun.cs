using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._ShotGun;
        prefabName = ObjectPool.Pool.PrefabName.ShotGun;
        IsStackable = false;
        Name = name;
    }
}
