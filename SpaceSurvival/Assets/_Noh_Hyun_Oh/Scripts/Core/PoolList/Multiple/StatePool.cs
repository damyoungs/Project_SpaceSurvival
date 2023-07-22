using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePool : MultipleObjectPool<StateObjectIsPool>
{
    protected override void StartInitialize()
    {
    }

    protected override void ReturnPoolTransformSetting(StateObjectIsPool comp,Transform poolObj)
    {
        comp.PoolTransform = poolObj;
    }
}
