using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Potion", menuName = "Scriptable Object/Item Data/ItemData - Potion", order = 7)]
public class ItemData_Potion : ItemData, IConsumable
{
    [Header("포션 전용 데이터")]
    public float duration;
    public int recoveryValue;
    public void Consume(GameObject target)
    {
        PlayerDummy player = target.GetComponent<PlayerDummy>();
        player.RecoveryHP_(recoveryValue, duration);
    }

}
