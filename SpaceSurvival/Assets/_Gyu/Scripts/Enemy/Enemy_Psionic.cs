using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Psionic : MonoBehaviour
{
    private void Awake()
    {
        Enemy_ enemy = GetComponent<Enemy_>();
        enemy.mType = Enemy_.Monster_Type.Size_M;
        enemy.HP = 200;
        enemy.MaxHp = 200;
    }
}
