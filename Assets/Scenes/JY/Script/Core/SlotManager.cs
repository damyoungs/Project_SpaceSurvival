using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform craft_Below;


    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
    public void Initialize()
    {
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Craft, new List<GameObject>() }
        };
        Add_Slot();
    }

    public void Add_Slot() //굳이 Make_Slot과 Add_Slot을 나눈 이유는  초기슬롯이 할당되지 않았을 때와 이후 슬롯을 추가하는 상황을 구분해야하기 때문이다.
    {
        if (GameManager.Inventory.is_Initialized == false)// 초기화가 아직 안되었을 경우(초기생성)
        {
            Make_Slot(5);
            GameManager.Inventory.State = Current_Inventory_State.Consume;
            Make_Slot(5);
            GameManager.Inventory.State = Current_Inventory_State.Etc;
            Make_Slot(5);
            GameManager.Inventory.State = Current_Inventory_State.Craft;
            Make_Slot(5);
            GameManager.Inventory.is_Initialized = true;
        }
        else// 초기화가 되어있다면 (게임 시작 후 이미 이전에 인벤토리를 활성화시킨적이 있다면)
        {
            Make_Slot(0);
        }
    }
    public void Make_Slot(int additional_Value )
    {
        int count = 5 + (additional_Value);
        for (int i = 0; i < count; i++)
        {
            GameObject newSlot = Instantiate(slot);
            Transform parentTransform;
            switch (GameManager.Inventory.State)
            {
                case Current_Inventory_State.Equip:
                    parentTransform = equip_Below;
                    break;
                case Current_Inventory_State.Consume:
                    parentTransform = consume_Below;
                    break;
                case Current_Inventory_State.Etc:
                    parentTransform = etc_Below;
                    break;
                case Current_Inventory_State.Craft:
                    parentTransform = craft_Below;
                    break;
                default:
                    parentTransform = null;
                    break;
            }
            if (parentTransform != null)
            {
                newSlot.name = $"{GameManager.Inventory.State}_{i}";
                newSlot.transform.SetParent(parentTransform, false);
                slots[GameManager.Inventory.State].Add(newSlot);
            }
        }
    }
    public void SetItemData(ItemName name)
    {

    }
}
