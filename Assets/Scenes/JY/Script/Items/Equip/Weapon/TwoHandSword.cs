using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._TwoHandSword;
        IsStackable = false;
        Name = name;
    }
}
