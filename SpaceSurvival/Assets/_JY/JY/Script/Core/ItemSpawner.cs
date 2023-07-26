using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    int value;
    public GameObject[] prefabs;
  //  public float dropRate;
    private Dictionary<ItemCode, GameObject> prefabDict = new Dictionary<ItemCode, GameObject>();
    private Dictionary<Type, List<(ItemCode, float)>> enemyDropTable = new Dictionary<Type, List<(ItemCode, float)>>();//드랍테이블 생성
    // Enemy의 타입에 따라 PrefabName(아이템) 을 float 확률로 드롭해라. 그 PrefabName에  dictionary로 GameObject를 바인드해서 연결해준다.

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void Initialize()
    {
        SetDropTable();//몬스터 드롭정보 저장
    }
    void SetDropTable()
    {
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length - 1)
        {
            Debug.LogError("Enum 의 갯수와 프리팹배열의 갯수가 다릅니다.");
            return;
        }
        // Make sure the prefabs array and the PrefabName enum have the same length
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length - 1)
        {
            Debug.LogError("The prefabs array and the PrefabName enum do not match!");
            return;
        }

        // Initialize the prefab dictionary
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabDict.Add((ItemCode)i + 1, prefabs[i]);
        }

        // Initialize the enemy drop table
        enemyDropTable.Add(typeof(Enemy1), new List<(ItemCode, float)>
            {
                (ItemCode.ShotGun, 0.9f),
                (ItemCode.Rifle, 0.99f),
          
            });

        enemyDropTable.Add(typeof(Enemy2), new List<(ItemCode, float)>
            {
                (ItemCode.Bow, 0.9f),
                (ItemCode.TwoHandAxe, 0.9f),
                (ItemCode.TwoHandSword,0.9f)
            });
    }
    public void SpawnItem(EnemyBase enemy)//큰 범위에서 분류가 아니라 정확히 어떤 적인지 알아야한다
    {
        List<(ItemCode, float)> dropTable = enemyDropTable[enemy.GetType()];

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
       // GameObject obj = Instantiate(prefabDict[ItemCode.Hammer]);

        GameManager.SlotManager.AddItem(ItemCode.Hammer);

    }
    public void GetItemBow()
    {
        GameManager.SlotManager.AddItem(ItemCode.Bow);
    }
    public void GetItemHpPotion()
    {
        GameManager.SlotManager.AddItem(ItemCode.HpPotion);
    }
    public void GetItemMpPotion()
    {
        GameManager.SlotManager.AddItem(ItemCode.MpPotion);
    }
    protected override void TestClick(InputAction.CallbackContext context)
    {
        if (GameManager.SlotManager.IsSlotMoving)
        {
            RectTransform inventoryRectTransform = GameManager.Inventory.GetComponent<RectTransform>();
            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRectTransform, Input.mousePosition, null, out localMousePosition))
            {
                if (!inventoryRectTransform.rect.Contains(localMousePosition))
                {
                   // GameManager.SlotManager.DropItem();
                }
            }
        }
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
