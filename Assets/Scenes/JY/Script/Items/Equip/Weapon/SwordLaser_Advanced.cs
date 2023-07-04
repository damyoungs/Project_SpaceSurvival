using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLaser_Advanced : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._SwordLaserAdvanced;
        IsStackable = false;
        Name = name;
    }
}
