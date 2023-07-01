using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public enum ItemType
{
    Equip,
    Consume,
    Etc,
    Craft
}
public class ItemBase : MonoBehaviour
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
    protected virtual void OnDisable()
    {
        returnPool?.Invoke(this.gameObject, prefabName);
    }
    protected IEnumerator LifeOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
