using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Bullet", menuName = "Scriptable Object/Item Data/ItemData - Bullet", order = 6)]
public class ItemData_Bullet : ItemData
{
    [Header("Bullet 전용 데이터")]
    public byte attckPoint;
    public byte successRate;
}
