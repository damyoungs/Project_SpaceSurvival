using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data - Armor", menuName = "Scriptable Object/Item Data/ItemData - Armor", order = 9)]
public class ItemData_Armor : ItemData
{
    [Header("장비 타입")]
    public EquipType EquipType;
    public uint attack_Point;
    public uint defence_Point;
}
