using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : ConsumeBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath.MpPotion;
        IsStackable = true;
        Name = "MpPotion";
        RecoveryMpValue = 50;
    }

}
