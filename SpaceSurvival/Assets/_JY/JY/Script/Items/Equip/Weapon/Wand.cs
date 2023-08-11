using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Wand;
        IsStackable = false;
        Name = "Wand";
    }
}
