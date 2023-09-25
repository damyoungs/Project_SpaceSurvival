using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Hat", menuName = "Scriptable Object/Item Data/ItemData - Hat", order = 8)]
public class ItemData_Hat : ItemData, IEquippable
{
    [Header("��� ������ �з�")]
    public EquipType EquipType;
    public uint defence_Point;
    public uint attack_Point;
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
