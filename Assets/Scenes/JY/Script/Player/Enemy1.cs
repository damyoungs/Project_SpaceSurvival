using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    private void Start()
    {
        StartCoroutine(DieOperator());
    }
    IEnumerator DieOperator()
    {
        yield return new WaitForSeconds(10.0f);
        OnDie();
    }
    void OnDie()
    {
        GameManager.Item_Spawner.SpawnItem(this);
        Destroy(gameObject);
    }
}
