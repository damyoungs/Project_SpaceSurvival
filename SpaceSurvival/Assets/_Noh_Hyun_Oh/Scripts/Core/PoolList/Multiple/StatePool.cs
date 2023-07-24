using UnityEngine;

public class StatePool : MultipleObjectPool<StateObjectIsPool>
{


    protected override void ReturnPoolTransformSetting(StateObjectIsPool comp,Transform poolObj)
    {
        comp.PoolTransform = poolObj;
    }
}
