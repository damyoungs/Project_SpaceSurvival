using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Rifle;
        IsStackable = false;
        Name = name;
    }
}
