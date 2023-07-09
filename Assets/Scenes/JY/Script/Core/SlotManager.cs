using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform craft_Below;

    Slot selectedSlot;// 처음 클릭한 슬롯을 저장하기 위한 변수
    Image firstClickImage; //첫번째 클릭한 슬롯 하위의 아이템 이미지
    RectTransform imageTransform; 

    Vector2 firstSlotPosition;
    Vector2 secondSlotPosition;

    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //슬롯 생성후 번호를 부여하기위한 Dic
    public void Initialize()
    {
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Craft, new List<GameObject>() }
        };
        slotCount = new Dictionary<Current_Inventory_State, int>
        {
            { Current_Inventory_State.Equip, 0 },
            { Current_Inventory_State.Consume, 0},
            { Current_Inventory_State.Etc, 0},
            { Current_Inventory_State.Craft, 0}
        };
        GameManager.Inventory.State = Current_Inventory_State.Equip;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Make_Slot();
            }
            switch (GameManager.Inventory.State)
            {
                case Current_Inventory_State.Equip:
                    GameManager.Inventory.State = Current_Inventory_State.Consume;
                    break;
                case Current_Inventory_State.Consume:
                    GameManager.Inventory.State = Current_Inventory_State.Etc;
                    break;
                case Current_Inventory_State.Etc:
                    GameManager.Inventory.State = Current_Inventory_State.Craft;
                    break;
                default:
                    break;
            }
        }
    }

    public void Make_Slot()
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
            slotCount[GameManager.Inventory.State]++;
            newSlot.name = $"{GameManager.Inventory.State}_{slotCount[ GameManager.Inventory.State]}";
            newSlot.transform.SetParent(parentTransform, false);
            slots[GameManager.Inventory.State].Add(newSlot);
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
            if (item.IsStackable  && item.name == slot.CurrentItem)
            {
                slot.ItemCount++;
                break;
            }
            else if (slot.IsEmpty) //a+만약 슬롯이 비었다면
            {
                Image slotImage = slotObject.transform.GetChild(0).GetComponent<Image>();// 바꿔줄 이미지 컴포넌트 가져오기
                string spriteName = Enum.GetName(typeof(ItemImagePath), item.ItemImagePath);// enum의 이름을 string 변수에 넣어주기
                
                Sprite[] sprite = Resources.LoadAll<Sprite>($"ItemImage/Items");
                foreach (Sprite s in sprite)
                {
                    if (s.name == spriteName)
                    {
                        slotImage.sprite = s;
                        break;
                    }
                }
                slot.IsEmpty = false;
                slot.CurrentItem = item.name;
                // 아이템을 추가했으므로 loop를 중단
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

    public void OnSlotClicked(Slot clickedSlot)
    {

       
        // 첫 클릭: 선택한 슬롯 저장
        if (selectedSlot == null)
        {
            if (!clickedSlot.IsEmpty)
            {
                selectedSlot = clickedSlot;
                firstClickImage = clickedSlot.transform.GetChild(0).GetComponent<Image>(); //이미지의 알파값을 바꾸기 위한 이미지 컴포넌트
                firstSlotPosition = clickedSlot.transform.position;
                imageTransform = clickedSlot.transform.GetChild(0).GetComponent<RectTransform>();

                StartCoroutine(ImageMovingCoroutine());
            }
        }

        // 두 번째 클릭: 아이템 교환하고 선택한 슬롯 초기화
        else
        {
            secondSlotPosition = clickedSlot.transform.position;

            ResetImageAlpha();//이동중인 첫번째슬롯 알파값 원상복구
            SwapItems(selectedSlot, clickedSlot);
            selectedSlot = null;
           
        }
    }
    void ResetImageAlpha()
    {

        if (firstClickImage != null)
        {
            var color = firstClickImage.color;
            color.a = 1.0f;
            firstClickImage.color = color;
        }
    }
    void SwapItems(Slot firstSlot, Slot secondSlot)
    {
        // 먼저 두 슬롯이 어떤 리스트에 속해 있는지 알아냅니다.
        List<GameObject> SlotList = null;

        foreach (var slotList in slots)
        {
            if (slotList.Value.Contains(firstSlot.gameObject))
            {
                SlotList = slotList.Value;
            }
        }

        if (SlotList == null)
        {
            Debug.LogError("Slot lists not found.");
            return;
        }

        // 각 슬롯의 인덱스를 찾습니다.
        int firstSlotIndex = SlotList.IndexOf(firstSlot.gameObject);
        int secondSlotIndex = SlotList.IndexOf(secondSlot.gameObject);

        // 슬롯을 잠시 저장하고 리스트에서 삭제합니다.
        GameObject tempFirstSlot = SlotList[firstSlotIndex];
        GameObject tempSecondSlot = SlotList[secondSlotIndex];

        firstSlot.gameObject.transform.position = secondSlotPosition;
        secondSlot.transform.position = firstSlotPosition;


        SlotList.RemoveAt(firstSlotIndex);
        SlotList.RemoveAt(secondSlotIndex > firstSlotIndex ? secondSlotIndex - 1 : secondSlotIndex);
        // 삭제한 위치에 다른 슬롯을 추가하여 위치를 바꿉니다.
        SlotList.Insert(firstSlotIndex, tempSecondSlot);
        SlotList.Insert(secondSlotIndex, tempFirstSlot);
    }
    IEnumerator ImageMovingCoroutine()
    {
        var color = firstClickImage.color;
        color.a = 0.5f;//로컬변수 color의 알파값을 변경하는건 가능하지만  clickedItemImage.color.a = 0.5f; 이렇게 직접 값을 변경하는건 읽기전용이라 안된다
        firstClickImage.color = color;
        while (selectedSlot != null)
        {
            imageTransform.position = Input.mousePosition;
            yield return null;
        }
        imageTransform.anchoredPosition = Vector2.zero;
       // selectedSlot.transform.GetChild(0).position =  Vector2.zero;
        yield break;
    }
}
