using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPotion : ConsumeBase
{

    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath.SecretPotion;
        IsStackable = true;
        Name = "SecretPotion";
        RecoveryMpValue = 50;
    }
}
