using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    public GameObject[] prefabs;
  //  public float dropRate;
    private Dictionary<ItemCode, GameObject> prefabDict = new Dictionary<ItemCode, GameObject>();
    private Dictionary<Type, List<(ItemCode, float)>> enemyDropTable = new Dictionary<Type, List<(ItemCode, float)>>();//������̺� ����
    // Enemy�� Ÿ�Կ� ���� PrefabName(������) �� float Ȯ���� ����ض�. �� PrefabName��  dictionary�� GameObject�� ���ε��ؼ� �������ش�.

    public uint index = 0;
    public ItemCode itemCode;
    public ItemSortBy sortBy;
    public bool IsAccending;
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void Initialize()
    {
        SetDropTable();//���� ������� ����
    }
    private void Start()
    {
        player = GameManager.playerDummy;
    }
    void SetDropTable()
    {
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length)
        {
            Debug.LogError("Enum �� ������ �����չ迭�� ������ �ٸ��ϴ�.");
            return;
        }
        // Make sure the prefabs array and the PrefabName enum have the same length
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length)
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
                (ItemCode.Enhancable_shotGun, 0.9f),
                (ItemCode.Enhancable_Rifle, 0.99f),
          
            });

        enemyDropTable.Add(typeof(Enemy2), new List<(ItemCode, float)>
            {
                (ItemCode.Enhancable_Bow, 0.9f),
            });
    }
    public void SpawnItem(EnemyBase enemy)//ū �������� �з��� �ƴ϶� ��Ȯ�� � ������ �˾ƾ��Ѵ�
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
        GameManager.SlotManager.AddItem(itemCode);
    }
    public void ClearInventory()//itemdata ���� ���ʿ� ���� ���õ� ���� �κ��丮�� �������� Ŭ����
    {
        GameManager.SlotManager.ClearInventory();
    }
    public void ClearSlot()
    {
        ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.ClearSlot(data, index);
    }
    public void GetItemBow()
    {
    }
    public void GetItemHpPotion()
    {
        GameManager.SlotManager.AddItem(ItemCode.HpPotion);
    }
    public void RemoveItem()
    {
        ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.RemoveItem(data, index);
    }
    public void GetItemMpPotion()
    {
        GameManager.SlotManager.AddItem(ItemCode.MpPotion);
    }
    public void SlotSorting()
    {
       // ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.SlotSorting(ItemSortBy.Price, false);
    }
    //protected override void TestClick(InputAction.CallbackContext context)
    //{
    //    if (GameManager.SlotManager.IsSlotMoving)
    //    {
    //        RectTransform inventoryRectTransform = GameManager.Inventory.GetComponent<RectTransform>();
    //        Vector2 localMousePosition;
    //        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRectTransform, Input.mousePosition, null, out localMousePosition))
    //        {
    //            if (!inventoryRectTransform.rect.Contains(localMousePosition))
    //            {
    //               // GameManager.SlotManager.DropItem();
    //            }
    //        }
    //    }
    //}

    public void SpawnItemPrefab()
    {
        ItemFactory.MakeItem(itemCode);
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
       
        SpawnItemPrefab();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        GetItem();
        // GetItemHpPotion();
    }
    protected  void OpenInven(InputAction.CallbackContext _)
    {

    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        int i = 0;
        while(i < 200)
        {
            GetItem();
            i++;
        }
       // GameManager.playerDummy.RecoveryHP_(300, 0.5f);
    }

    public EquipBox equipBox;
    protected override void Test4(InputAction.CallbackContext context)
    {
        equipBox.Set_ItemData_For_Drag(GameManager.Itemdata[ItemCode.HpPotion]);
    }
    PlayerDummy player;
    protected override void Test5(InputAction.CallbackContext context)
    {
        GameManager.SlotManager.Inven_Clear();

    }
    protected override void Test6(InputAction.CallbackContext context)
    {
    }
    //protected override void Test7(InputAction.CallbackContext context)
    //{
    //    ClearSlot();
    //}
    //protected override void Test8(InputAction.CallbackContext context)
    //{
    //    ClearInventory();
    //}
    //protected override void Test9(InputAction.CallbackContext context)
    //{
    //    SlotSorting();
    //}
}
