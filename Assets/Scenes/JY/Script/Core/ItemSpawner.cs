using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ObjectPool;

public class ItemSpawner : TestBase
{
    private Dictionary<Type, List<(Pool.PrefabName, float)>> enemyDropTable = new Dictionary<Type, List<(Pool.PrefabName, float)>>();//드랍테이블 생성
    private void Start()//팩토리에서 enum 그대로 가져오기
    {
        enemyDropTable.Add(typeof(Enemy1), new List<(Pool.PrefabName, float)>// 적 타입별 드롭 아이템 목록
        {
            (Pool.PrefabName.ShotGun, 0.9f),
            (Pool.PrefabName.Rifle, 0.99f),
            (Pool.PrefabName.Cash,0.9f)
        }) ;
        enemyDropTable.Add(typeof(Enemy2), new List<(Pool.PrefabName, float)>
        {
            (Pool.PrefabName.Bow, 0.9f),// 틱타임 추가해야함 현재 똑같은 드랍률이 나옴
            (Pool.PrefabName.TwoHandAxe, 0.9f),
            (Pool.PrefabName.Cash,0.9f),
            (Pool.PrefabName.TwoHandSword,0.9f)
        });
    }
    public void SpawnItem(EnemyBase enemy)//큰 범위에서 분류가 아니라 정확히 어떤 적인지 알아야한다
    {
        List<(Pool.PrefabName, float)> dropTable = enemyDropTable[enemy.GetType()];

        foreach (var (itemtype, droprate)in dropTable)
        {
            if (UnityEngine.Random.value <= droprate)
            {
                objectPool.GetObject(itemtype,enemy.transform.position);
            }
        }
    }
    List<GameObject> spawnedObjects = new List<GameObject>();
    public Player player;
    protected override void Test1(InputAction.CallbackContext _)
    {
         objectPool.GetObject(Pool.PrefabName.MpPotion);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        objectPool.GetObject(Pool.PrefabName.HpPotion, player.transform.position);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        Vector3 newPos = new Vector3(1,0,3);
        objectPool.GetObject(Pool.PrefabName.Bow, newPos);
    }
}
