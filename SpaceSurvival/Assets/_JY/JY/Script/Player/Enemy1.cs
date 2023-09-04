using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    public IEnumerator DieCoroutine;
    private void Awake()
    {
        DieCoroutine = DieOperator();
    }
    private void Start()
    {

        StartCoroutine(DieOperator());
    }
    public IEnumerator DieOperator()
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
