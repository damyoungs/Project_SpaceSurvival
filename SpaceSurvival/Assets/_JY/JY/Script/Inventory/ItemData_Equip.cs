using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Equip", menuName = "Scriptable Object/Item Data/ItemData - Equip", order = 3)]
public class ItemData_Equip : ItemData
{
    [Header("장비아이템 데이터")]
    public uint attackPoint;
    public uint defencePoint;
}
