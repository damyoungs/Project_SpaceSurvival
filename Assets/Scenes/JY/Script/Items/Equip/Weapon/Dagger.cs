using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Dagger;
        IsStackable = false;
        Name = "´Ü°Ë";
    }
}
