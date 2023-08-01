using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Food : ItemData, IConsumable
{
    public uint AmountTick;
    public float duration;
    public float recoveryValue;
    public void Consume(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
         
        }
    }

}
