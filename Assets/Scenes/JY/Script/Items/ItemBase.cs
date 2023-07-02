using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;//ObjectPool을 불러와서 사용할 때  ObjectPool을 앞에 써주지 않아도 사용할 수  있게 한다.

public enum ItemType
{
    None,
    Equip,
    Consume,
    Etc,
    Craft
}
public enum ItemImagePath
{
    Empty,
    _HpPotion,
    _MpPotion,
    _Shield,
    _Shield_Extended,
    _Bat,
    _Bow,
    _Dagger,
    _Pistol,
    _Rifle,
    _ShotGun,
    _SwordLaser,
    _SwordLaser_Advanced,
    _TwoHandAxe,
    _TwoHandSword,
    _Wand
}
public class ItemBase : MonoBehaviour
{
    public ItemType ItemType { get; protected set; }
    public ItemImagePath ItemImagePath { get; protected set; }
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
