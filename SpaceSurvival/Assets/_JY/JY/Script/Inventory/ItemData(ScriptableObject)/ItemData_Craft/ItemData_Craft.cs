using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Craft", menuName = "Scriptable Object/Item Data/ItemData - Craft", order = 5)]
public class ItemData_Craft : ItemData
{
    [Header("���� ���� ������")]
    public CraftType CraftType;
    public float successRate;
    public ItemData Critical_Success_Item;
}
