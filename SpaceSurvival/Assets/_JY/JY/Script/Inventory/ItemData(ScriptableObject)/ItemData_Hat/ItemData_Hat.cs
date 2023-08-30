using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Hat", menuName = "Scriptable Object/Item Data/ItemData - Hat", order = 8)]
public class ItemData_Hat : ItemData
{
    [Header("��� ������ �з�")]
    public EquipType EquipType;
    public uint defence_Point;
    public uint attack_Point;
}
