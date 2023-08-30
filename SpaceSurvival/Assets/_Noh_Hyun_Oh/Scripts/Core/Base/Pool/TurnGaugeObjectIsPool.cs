using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGaugeObjectIsPool : ObjectIsPool
{
    Transform poolTransform;
    public Transform PoolTransform
    {
        get => poolTransform;
        set
        {
            if (poolTransform == null)
            {
                poolTransform = value;
            }
        }
    }
    private void Awake()
    {
        isPositionReset = false;
    }
}