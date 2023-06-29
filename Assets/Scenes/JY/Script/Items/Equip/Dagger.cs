using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : ItemBase
{
    protected override void Initailize()
    {
        itemType = ItemType.Equip;
        Name = name;
    }
}
