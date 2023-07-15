using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    private void Start()
    {
        StartCoroutine(DieOperator());
    }
    IEnumerator DieOperator()
    {
        yield return new WaitForSeconds(5.0f);
        OnDie();
    }
    void OnDie()
    {
        GameManager.Item_Spawner.SpawnItem(this);
        Destroy(gameObject);
    }
}
