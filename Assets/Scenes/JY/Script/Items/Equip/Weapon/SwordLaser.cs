using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLaser : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._SwordLaser;
        IsStackable = false;
        Name = name;
    }
}
