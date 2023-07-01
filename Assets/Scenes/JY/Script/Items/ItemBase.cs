using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NewObjectPool;

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
    protected Pool.PrefabName prefabName;

    public delegate void ReturnToPoolDelegate(GameObject obj, Pool.PrefabName prefabName);
    public event ReturnToPoolDelegate returnPool;
    protected virtual void Start()
    {
        Initailize();
    }
    protected virtual void Initailize()
    {

    }
    protected override void OnDisable()
    {
        returnPool?.Invoke(this.gameObject, prefabName);
    }
}
