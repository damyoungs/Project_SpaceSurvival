using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapUnitPool : MultipleObjectPool<BattleMapUnitIsPool>
{
    protected override void ReturnPoolTransformSetting(BattleMapUnitIsPool comp, Transform poolObj)
    {
        comp.PoolTransform = poolObj;
    }
}

