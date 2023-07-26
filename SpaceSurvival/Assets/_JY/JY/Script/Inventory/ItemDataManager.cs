using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    None,
    Cash,
    HpPotion,
    MpPotion,
    SecretPotion,
    SpellBook,
    Shield,
    Shield_Extended,
    Bat,
    Bow,
    Dagger,
    Hammer,
    Pistol,
    Rifle,
    Scythe,
    ShotGun,
    SwordLaser,
    SwordLaser_Advanced,
    TwoHandAxe,
    TwoHandSword,
    Wand
}
public enum ItemType
{
    None,
    Equip,
    Consume,
    Etc,
    Craft
}
public enum ItemSortBy
{
    Code,  
    Name,  
    Price, 
}

public class ItemDataManager : MonoBehaviour
{

    public ItemData[] itemDatas = null;

    public ItemData this[ItemCode code] => itemDatas[(int)code];

    public int length => itemDatas.Length;

}