using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scythe : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Scythe;
        IsStackable = false;
        Name = name;
    }
}
