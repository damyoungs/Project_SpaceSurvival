using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    public void GetItem(ItemBase item)
    {
        // itemType에 따른 리스트를 가져오기
        List<GameObject> slotList = GetItemTab(item);//item.itemtype에 따른 리스트(장비, 소비, 기타 중 어느곳에 연결된 리스트인지) 연결하기
        UpdateSlotImage(item, slotList);
    }
    private void UpdateSlotImage(ItemBase item, List<GameObject> slotList)
    {
        foreach (GameObject slotObject in slotList)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (slot.IsEmpty) //a+만약 슬롯이 비었다면
            {
                Image slotImage = slotObject.transform.GetChild(0).GetComponent<Image>();
                SpriteAtlas atlas = Resources.Load<SpriteAtlas>("ItemImage/ItemImages");
                string spriteName = Enum.GetName(typeof(ItemImagePath), item.ItemImagePath);
                slotImage.sprite = atlas.GetSprite(spriteName);
                // slotImage.sprite = item.image;

                // 슬롯이 빈 상태가 아님을 표시합니다.
                slot.IsEmpty = false;

                // 아이템을 추가했으므로 loop를 중단합니다.
                break;
            }
        }
    }
    private List<GameObject> GetItemTab(ItemBase item)
    {
        List<GameObject> slotList;
        switch (item.ItemType)
        {
            case ItemType.Equip:
                slotList = slots[Current_Inventory_State.Equip];
                break;
            case ItemType.Consume:
                slotList = slots[Current_Inventory_State.Consume];
                break;
            case ItemType.Etc:
                slotList = slots[Current_Inventory_State.Etc];
                break;
            case ItemType.Craft:
                slotList = slots[Current_Inventory_State.Craft];
                break;
            default:
                slotList = null;
                break;
        }
        return slotList;
    }

}
