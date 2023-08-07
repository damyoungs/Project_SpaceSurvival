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

    public ItemData this[ItemCode code] => Instantiate(itemDatas[(int)code]);//cash �� itemDatas�迭�� ��� null

    public int length => itemDatas.Length;

}