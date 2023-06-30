using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    private Dictionary<Type, List<(Pool_Object_Type, float)>> enemyDropTable = new Dictionary<Type, List<(Pool_Object_Type, float)>>();//드랍테이블 생성 
    private void Start()
    {
        enemyDropTable.Add(typeof(Enemy1), new List<(Pool_Object_Type, float)>// 적 타입별 드롭 아이템 목록
        {
            (Pool_Object_Type.HpPotion, 0.9f),
            (Pool_Object_Type.TwoHandSword, 0.99f),
            (Pool_Object_Type.Cash,0.9f)
        }) ;
        enemyDropTable.Add(typeof(Enemy2), new List<(Pool_Object_Type, float)>
        {
            (Pool_Object_Type.MpPotion, 0.9f),// 틱타임 추가해야함 현재 똑같은 드랍률이 나옴
            (Pool_Object_Type.TwoHandAxe, 0.9f),
            (Pool_Object_Type.Cash,0.9f),
            (Pool_Object_Type.TwoHandSword,0.9f)
        });
    }
    public void SpawnItem(EnemyBase enemy)//큰 범위에서 분류가 아니라 정확히 어떤 적인지 알아야한다
    {
        List<(Pool_Object_Type, float)> dropTable = enemyDropTable[enemy.GetType()];

        foreach (var (itemtype, droprate)in dropTable)
        {
            if (UnityEngine.Random.value <= droprate)
            {
                GameObject go = Factory.I.GetObject(itemtype,enemy.transform.position);
            }
        }
    }
    List<GameObject> spawnedObjects = new List<GameObject>();
    Player player;
    protected override void Test1(InputAction.CallbackContext _)
    {
        GameObject go = Factory.I.GetObject(Pool_Object_Type.HpPotion);
        spawnedObjects.Add(go);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        while(spawnedObjects.Count > 0)
        {
            GameObject del = spawnedObjects[0];
            spawnedObjects.RemoveAt(0);
            del.SetActive(false);
        }
        spawnedObjects.Clear();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {

    }
}
