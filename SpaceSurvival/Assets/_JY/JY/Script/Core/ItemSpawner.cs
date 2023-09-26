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
    private Dictionary<Type, List<(ItemCode, float)>> enemyDropTable = new Dictionary<Type, List<(ItemCode, float)>>();//드랍테이블 생성
    // Enemy의 타입에 따라 PrefabName(아이템) 을 float 확률로 드롭해라. 그 PrefabName에  dictionary로 GameObject를 바인드해서 연결해준다.

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
        SetDropTable();//몬스터 드롭정보 저장
    }
    private void Start()
    {
        StartCoroutine(TestInit());
    }

    IEnumerator TestInit() 
    {
        yield return null;
        player = GameManager.Player_;
        GetItem_For_Test();
        SetSkillData_For_Test();
    }

    private void SetSkillData_For_Test()
    {
        SkillBox skillBox = FindObjectOfType<SkillBox>();
        for (int  i = 0; i < 5; i++)
        {
            SkillData skillData = skillBox.transform.GetChild(i + 2).GetComponent<SkillData>();
            GameManager.QuickSlot_Manager.QuickSlots[i].SkillData = skillData;
        }
    }

    void SetDropTable()
    {
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length)
        {
            Debug.LogError("Enum 의 갯수와 프리팹배열의 갯수가 다릅니다.");
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
        GameManager.SlotManager.AddItem(itemCode);
    }
   
    public void ClearSlot()
    {
        ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.ClearSlot(data, index);
    }
    public void GetItem_For_Test()
    {
        int i = 0;
        while(i < 50)
        {
            GameManager.SlotManager.AddItem(ItemCode.HpPotion);
            GameManager.SlotManager.AddItem(ItemCode.MpPotion);
            i++;
        }
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_shotGun);
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_Pistol);
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_Rifle);
        GameManager.SlotManager.AddItem(ItemCode.Captains_Hat);
        GameManager.SlotManager.AddItem(ItemCode.Crews_Hat);
        GameManager.SlotManager.AddItem(ItemCode.Big_Space_Armor);
        GameManager.SlotManager.AddItem(ItemCode.Space_Armor);
        GameManager.SlotManager.AddItem(ItemCode.Intermidiate_Green_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Advanced_Red_Crystal);
    }
    public void GetItemH()
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
        GameManager.QuickSlot_Manager.TestSave();
        Debug.Log("저장완료");
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.QuickSlot_Manager.QuickSlots_Clear();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.QuickSlot_Manager.TestLoadData();
    }
    protected  void OpenInven(InputAction.CallbackContext _)
    {

    }

    protected override void Test4(InputAction.CallbackContext context)
    {
    }
    Player_ player;
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
