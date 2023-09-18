using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Psionic : Enemy_
{
    public int EnemyNumBe;

    private void Awake()
    {
        EnemyNumBe = GetComponentInChildren<Enemy_>().EnemyNumber;
    }
}
