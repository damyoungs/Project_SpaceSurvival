using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data - Armor", menuName = "Scriptable Object/Item Data/ItemData - Armor", order = 9)]
public class ItemData_Armor : ItemData, IEquippable
{
    [Header("장비 타입")]
    public EquipType EquipType;
    public uint attack_Point;
    public uint defence_Point;
    public uint STR;
    public uint INT;
    public uint LUK;
    public uint DEX;




    uint IEquippable.STR => STR;
    uint IEquippable.INT => INT;
    uint IEquippable.LUK => LUK;
    uint IEquippable.DEX => DEX;
    uint IEquippable.ATT => attack_Point;
    uint IEquippable.DP => defence_Point;
}
