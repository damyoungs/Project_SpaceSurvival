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
    public ItemType Type { get; set; }
    protected string Name { get; set; }
    protected bool Is_Stackable { get; }
}
