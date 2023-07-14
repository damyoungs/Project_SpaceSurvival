using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PrefabName // ������ prefab variant
{
    None,
    Cash,
    HpPotion,
    MpPotion,
    SecretPotion,
    Bat,
    Bow,
    Dagger,
    Hammer,
    Pistol,
    Rifle,
    Scythe,
    Shield,
    Shield_Extended,
    ShotGun,
    SpellBook,
    SwordLaser,
    SwordLaser_Advanced,
    TwoHandAxe,
    TwoHandSword,
    Wand,
}
public class ItemSpawner : TestBase
{
    int value;
    public GameObject[] prefabs;
  //  public float dropRate;
    private Dictionary<PrefabName, GameObject> prefabDict = new Dictionary<PrefabName, GameObject>();
    private Dictionary<Type, List<(PrefabName, float)>> enemyDropTable = new Dictionary<Type, List<(PrefabName, float)>>();//������̺� ����
    // Enemy�� Ÿ�Կ� ���� PrefabName(������) �� float Ȯ���� ����ض�. �� PrefabName��  dictionary�� GameObject�� ���ε��ؼ� �������ش�.

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void Initialize()
    {
        if (prefabs.Length != Enum.GetValues(typeof(PrefabName)).Length - 1)
        {
            Debug.LogError("Enum �� ������ �����չ迭�� ������ �ٸ��ϴ�.");
            return;
        }
        // Make sure the prefabs array and the PrefabName enum have the same length
        if (prefabs.Length != Enum.GetValues(typeof(PrefabName)).Length - 1)
        {
            Debug.LogError("The prefabs array and the PrefabName enum do not match!");
            return;
        }

        // Initialize the prefab dictionary
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabDict.Add((PrefabName)i + 1, prefabs[i]);
        }

        // Initialize the enemy drop table
        enemyDropTable.Add(typeof(Enemy1), new List<(PrefabName, float)>
        {
            (PrefabName.ShotGun, 0.9f),
            (PrefabName.Rifle, 0.99f),
            (PrefabName.Cash,0.9f)
        });

        enemyDropTable.Add(typeof(Enemy2), new List<(PrefabName, float)>
        {
            (PrefabName.Bow, 0.9f),
            (PrefabName.TwoHandAxe, 0.9f),
            (PrefabName.Cash,0.9f),
            (PrefabName.TwoHandSword,0.9f)
        });
    }
    public void SpawnItem(EnemyBase enemy)//ū �������� �з��� �ƴ϶� ��Ȯ�� � ������ �˾ƾ��Ѵ�
    {
        List<(PrefabName, float)> dropTable = enemyDropTable[enemy.GetType()];

        foreach (var (itemtype, droprate)in dropTable)
        {
            if (UnityEngine.Random.value <= droprate)
            {
                Instantiate(prefabDict[itemtype], enemy.transform.position, Quaternion.identity);
            }
        }
    }
 
    public void GetItem()
    {
        GameObject obj = Instantiate(prefabDict[PrefabName.Hammer]);
        ItemBase item = obj.GetComponent<ItemBase>();
        GameManager.SlotManager.GetItem(item);
        Destroy(obj);
    }
    public void GetItemBow()
    {
        GameObject obj = Instantiate(prefabDict[PrefabName.Bow]);
        ItemBase item = obj.GetComponent<ItemBase>();
        GameManager.SlotManager.GetItem(item);
        Destroy(obj);
    }
    public void GetItemHpPotion()
    {
        GameObject obj = Instantiate(prefabDict[PrefabName.HpPotion]);
        ItemBase item = obj.GetComponent<ItemBase>();
        GameManager.SlotManager.GetItem(item);
        Destroy(obj);
    }
    public void GetItemMpPotion()
    {
        GameObject obj = Instantiate(prefabDict[PrefabName.MpPotion]);
        ItemBase item = obj.GetComponent<ItemBase>();
        GameManager.SlotManager.GetItem(item);
        Destroy(obj);
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
        GetItem();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {

        GetItemBow();

    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        GetItemHpPotion();
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        GetItemMpPotion();
    }
    protected override void Test5(InputAction.CallbackContext context)
    {

    }
    protected override void Test6(InputAction.CallbackContext context)
    {
 
    }
    protected override void Test7(InputAction.CallbackContext context)
    {

    }
}
