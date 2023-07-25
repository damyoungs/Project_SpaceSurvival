using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemImagePath
{
    EmptySlot,
    HpPotion,
    MpPotion,
    SecretPotion,
    SpellBook,
    Shield,
    ShieldExtended,
    Bat,
    Hammer,
    Bow,
    Dagger,
    Pistol,
    Rifle,
    ShotGun,
    SwordLaser,
    SwordLaserAdvanced,
    TwoHandAxe,
    TwoHandSword,
    Wand,
    Scythe
}
public class ItemBase 
{
    public ItemType ItemType { get; protected set; }
    public ItemImagePath ItemImagePath { get;  set; }
    public string Name { get; protected set; }
    public bool IsStackable { get; protected set; }
    public Coroutine lifeOverCoroutine;

    public string itemDescription;


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

}
