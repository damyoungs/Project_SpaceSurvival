using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Equip", menuName = "Scriptable Object/Item Data/ItemData - Equip", order = 3)]
public class ItemData_Equip : ItemData, IEquippable
{
    [Header("�������� ������")]
    public uint attackPoint;
    public uint defencePoint;

    uint IEquippable.ATT => attackPoint;
    uint IEquippable.DP => defencePoint;
}
