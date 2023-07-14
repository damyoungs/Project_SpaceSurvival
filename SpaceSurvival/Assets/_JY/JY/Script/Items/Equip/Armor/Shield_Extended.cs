using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Extended : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.ShieldExtended;
        IsStackable = false;
        Name = "ShieldExtended";
    }
}
