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
        private void Awake()
        {
            EnemyCode = 0;
        }
        void Die()
        {
            GameManager.Item_Spawner.SpawnItem(this);
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
    public void SpawnItem(EnemyBase enemy)//큰 범위에서 분류가 아니라 정확히 어떤 적인지 알아야한다
    {
        switch (enemy.EnemyCode)
        {
            case 0:

                break;
        }
    }
}
