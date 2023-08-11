using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode// ���� ItemData �迭�� ���� ������ �����ؾ� �˸��� �������� ��ȯ�Ѵ�.
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
    Enhancable_Rifle,
    Scythe,
    ShotGun,
    Enhancable_shotGun,
    SwordLaser,
    SwordLaser_Advanced,
    Enhancable_Sword,
    TwoHandAxe,
    TwoHandSword,
    Wand,
    AquaCrystal,
    BlueCrystal,
    DarkCrystal,
    Green_Crystal,
    Pink_Crystal,
    Purple_Crystal,
    Red_Crystal,
    Unknown_Crystal,
    Yellow_Crystal
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
public enum CraftType
{
    Blue_Crystal = 0,
    Dark_Crystal,
    Aqua_Crystal,
    Green_Crystal,
    Pink_Crystal,
    Purple_Crystal,
    Red_Crystal,
    Unknown_Crystal,
    Yellow_Crystal
}

public class ItemDataManager : MonoBehaviour
{

    public ItemData[] itemDatas = null;

    public ItemData this[ItemCode code] => Instantiate(itemDatas[(int)code]);//cash �� itemDatas�迭�� ��� null

    public int length => itemDatas.Length;

}