using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Equip", menuName = "Scriptable Object/Item Data/ItemData - Equip", order = 3)]
public class ItemData_Equip : ItemData, IEquippable
{
    [Header("�������� ������")]
    public uint attackPoint;
    public uint defencePoint;
    public uint STR;
    public uint INT;
    public uint LUK;
    public uint DEX;




    uint IEquippable.STR => STR;
    uint IEquippable.INT => INT;
    uint IEquippable.LUK => LUK;
    uint IEquippable.DEX => DEX;
    uint IEquippable.ATT => attackPoint;
    uint IEquippable.DP => defencePoint;
}
