using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Bat;
        IsStackable = false;
        Name = "Bat";
    }
}
