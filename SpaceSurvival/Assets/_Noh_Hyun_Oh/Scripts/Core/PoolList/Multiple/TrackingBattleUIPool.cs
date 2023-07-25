using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBattleUIPool : MultipleObjectPool<TrackingBattleUIObjectIsPool>
{
    protected override void ReturnPoolTransformSetting(TrackingBattleUIObjectIsPool comp,Transform poolObj)
    {
        comp.PoolTransform = poolObj;
    }
}
