using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equip,
    Consume,
    Etc,
    Craft
}
public class ItemBase : PooledObject
{
    public ItemType itemType;
    public string Name { get; protected set; }
    public bool IsStackable { get; protected set; }

    protected virtual void Start()
    {
        Initailize();
    }
    protected virtual void Initailize()
    {

    }
    
}
