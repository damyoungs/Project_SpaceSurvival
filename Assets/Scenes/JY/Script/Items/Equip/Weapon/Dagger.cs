using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Dagger;
        IsStackable = false;
        Name = name;
    }
}
