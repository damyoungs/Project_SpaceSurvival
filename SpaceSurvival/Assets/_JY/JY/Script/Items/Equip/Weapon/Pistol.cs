using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Pistol;
        IsStackable = false;
        Name = "ÇÇ½ºÅç";
    }
}
