using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ObjectPool;

public class ItemSpawner : TestBase
{
    int value;
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
        switch (value)
        {
            case 0:
                objectPool.GetObject(Pool.PrefabName.HpPotion);
                value++;
                break;
            case 1:
                objectPool.GetObject(Pool.PrefabName.MpPotion);
                value++;
                break;
            case 2:
                objectPool.GetObject(Pool.PrefabName.SecretPotion);
                value++;
                break;
            case 3:
                objectPool.GetObject(Pool.PrefabName.SpellBook);
                value++;
                break;
            case 4:
                objectPool.GetObject(Pool.PrefabName.Shield);
                value++;
                break;
            case 5:
                objectPool.GetObject(Pool.PrefabName.Shield_Extended);
                value++;
                break;
            case 6:
                objectPool.GetObject(Pool.PrefabName.Bat);
                value++;
                break;
            case 7:
                objectPool.GetObject(Pool.PrefabName.Bow);
                value++;
                break;
            case 8:
                objectPool.GetObject(Pool.PrefabName.Dagger);
                value++;
                break;
            case 9:
                objectPool.GetObject(Pool.PrefabName.Pistol);
                value++;
                break;
            case 10:
                objectPool.GetObject(Pool.PrefabName.Rifle);
                value++;
                break;
            case 11:
                objectPool.GetObject(Pool.PrefabName.Scythe);
                value++;
                break;
            case 12:
                objectPool.GetObject(Pool.PrefabName.ShotGun);
                value++;
                break;
            case 13:
                objectPool.GetObject(Pool.PrefabName.SwordLaser);
                value++;
                break;
            case 14:
                objectPool.GetObject(Pool.PrefabName.SwordLaser_Advanced);
                value++;
                break;
            case 15:
                objectPool.GetObject(Pool.PrefabName.TwoHandAxe);
                value++;
                break;
            case 16:
                objectPool.GetObject(Pool.PrefabName.TwoHandSword);
                value++;
                break;
            case 17:
                objectPool.GetObject(Pool.PrefabName.Wand);
                value++;
                break;
            case 18:
                objectPool.GetObject(Pool.PrefabName.Pistol);
                value++;
                break;
            default:
      
                break;
        }
 
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        HpPotion hpPotion = objectPool.GetObject(Pool.PrefabName.HpPotion).GetComponent<HpPotion>();
        GameManager.SlotManager.GetItem(hpPotion);
        hpPotion.gameObject.SetActive(false);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        MpPotion mpPotion = objectPool.GetObject(Pool.PrefabName.MpPotion).GetComponent<MpPotion>();
        GameManager.SlotManager.GetItem(mpPotion);
        mpPotion.gameObject.SetActive(false);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        SecretPotion secretPotion = objectPool.GetObject(Pool.PrefabName.SecretPotion).GetComponentInParent<SecretPotion>();
        GameManager.SlotManager.GetItem(secretPotion);
        secretPotion.gameObject.SetActive(false);
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        SpellBook spellBook = objectPool.GetObject(Pool.PrefabName.SpellBook).GetComponentInParent<SpellBook>();
        GameManager.SlotManager.GetItem(spellBook);
        spellBook.gameObject.SetActive(false);
    }
    protected override void Test6(InputAction.CallbackContext context)
    {
        Scythe scythe = objectPool.GetObject(Pool.PrefabName.Scythe).GetComponent<Scythe>();
        GameManager.SlotManager.GetItem(scythe);
        scythe.gameObject.SetActive(false);
    }
    protected override void Test7(InputAction.CallbackContext context)
    {
        ShotGun shotGun = objectPool.GetObject(Pool.PrefabName.ShotGun).GetComponent<ShotGun>();
        GameManager.SlotManager.GetItem(shotGun);
        shotGun.gameObject.SetActive(false);
    }
}
