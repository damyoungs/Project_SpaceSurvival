using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellBook : ConsumeBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Consume;
        ItemImagePath = ItemImagePath.SpellBook;
        IsStackable = true;
        Name = "SpellBook";
        RecoveryMpValue = 50;
    }
}
