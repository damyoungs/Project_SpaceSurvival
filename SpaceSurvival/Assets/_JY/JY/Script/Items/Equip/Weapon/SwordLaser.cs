using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLaser : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.SwordLaser;
        IsStackable = false;
        Name = "레이져 소드";
    }
}
