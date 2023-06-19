using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MpPotion : ConsumeBase
{
    protected override void InitializeValue()
    {
        ItemType = ItemType.Consume;
        hpValue = 50;
        mpValue = 0;
        darkForceValue = 0;
        fatigueValue = 0;
    }
}
