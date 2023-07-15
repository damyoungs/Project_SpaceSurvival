using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandAxe : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.TwoHandAxe;
        IsStackable = false;
        Name = "µÎ¼Õµµ³¢";
    }
}
