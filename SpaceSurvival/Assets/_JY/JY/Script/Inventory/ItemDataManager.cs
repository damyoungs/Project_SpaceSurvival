using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode// 밑의 ItemData 배열과 같은 순서를 유지해야 알맞은 프리팹을 소환한다.
{
    Cash,
    HpPotion,
    MpPotion,
    SecretPotion,
    SpellBook,
    Shield,
    Shield_Extended,
    Bat,
    Bow,
    Enhancable_Bow,
    Dagger,
    Hammer,
    Pistol,
    Enhancable_Pistol,
    Rifle,
    Scythe,
    ShotGun,
    Enhancable_shotGun,
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
public enum EnhanceType
{
    attack,
    defence

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

    public ItemData this[ItemCode code] => Instantiate(itemDatas[(int)code]);//cash 가 itemDatas배열에 없어서 null

    public int length => itemDatas.Length;

}