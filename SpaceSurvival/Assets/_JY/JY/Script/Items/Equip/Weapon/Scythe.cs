using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scythe : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Scythe;
        IsStackable = false;
        Name = "Scythe";
    }
}
