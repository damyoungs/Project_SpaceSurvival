using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath._Shield;
        IsStackable = false;
        Name = name;

    }
}

