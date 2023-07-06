using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : EquipBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Equip;
        ItemImagePath = ItemImagePath.Bow;
        IsStackable = false;
        Name = name;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }

}
