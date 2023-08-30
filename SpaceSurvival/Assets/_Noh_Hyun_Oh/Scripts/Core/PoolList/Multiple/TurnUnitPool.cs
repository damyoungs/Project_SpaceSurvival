using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUnitPool : MultipleObjectPool<TurnGaugeObjectIsPool>
{
    protected override void ReturnPoolTransformSetting(TurnGaugeObjectIsPool comp, Transform poolObj)
    {
        comp.PoolTransform = poolObj;
    }
}
