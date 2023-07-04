using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    _SecretPotion,
    _SpellBook,
    _Shield,
    _ShieldExtended,
    _Bat,
    _Hammer,
    _Bow,
    _Dagger,
    _Pistol,
    _Rifle,
    _ShotGun,
    _SwordLaser,
    _SwordLaserAdvanced,
    _TwoHandAxe,
    _TwoHandSword,
    _Wand,
    _Scythe
}
public class ItemBase : MonoBehaviour
{
    public ItemType ItemType { get; protected set; }
    public ItemImagePath ItemImagePath { get; protected set; }
    public string Name { get; protected set; }
    public bool IsStackable { get; protected set; }
    public Coroutine lifeOverCoroutine;


    protected virtual void Awake()
    {
        Initailize();
       // StartCoroutine( LifeOver(3.0f));
    }
    protected virtual void Initailize()
    {

    }
    protected virtual void OnDisable()
    {

    }
    protected IEnumerator LifeOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.SlotManager.GetItem(this);//테스트용
        Destroy(gameObject);
    }
}
