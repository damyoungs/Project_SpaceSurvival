using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Alien : MonoBehaviour
{
    private void Awake()
    {
        Enemy_ enemy = GetComponent<Enemy_>();
        enemy.mType = Enemy_.Monster_Type.Size_S;
        enemy.HP = 100;
        enemy.MaxHp = 100;
    }
}
