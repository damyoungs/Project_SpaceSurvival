using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    public class EnemyBase : MonoBehaviour
    {
        public int EnemyCode { get; set; }
    }
    public class Enemy1 : EnemyBase
    {
        void Die()
        {
            GameManager.Item_Spawner.SpawnItem(this);
        }
    }
    public class Enemy2 : EnemyBase
    {
        void Die()
        {
            GameManager.Item_Spawner.SpawnItem(this);
        }
    }

    private Dictionary<Type, List<(Pool_Object_Type, float)>> enemyDropTable = new Dictionary<Type, List<(Pool_Object_Type, float)>>();//드랍테이블 생성 
    private void Start()
    {
        enemyDropTable.Add(typeof(Enemy1), new List<(Pool_Object_Type, float)>
        {
            (Pool_Object_Type.HpPotion, 0.5f),
            (Pool_Object_Type.Sword, 0.1f),
            (Pool_Object_Type.Cash,0.8f)
        }) ;
        enemyDropTable.Add(typeof(Enemy2), new List<(Pool_Object_Type, float)>
        {
            (Pool_Object_Type.MpPotion, 0.5f),
            (Pool_Object_Type.Gun, 0.1f),
            (Pool_Object_Type.Cash,0.8f)
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
