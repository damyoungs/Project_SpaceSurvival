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

    Sprite[] sprite; //아이템 이미지의 배열
    RectTransform imageTransform; 

    Vector2 firstSlotPosition;
    Vector2 secondSlotPosition;

    public delegate void IsMovingChange();
    public IsMovingChange isMovingChange; // Slot의 isMoving을 바꾸는 함수 호출

    public bool IsSlotMoving { get; set; } = false; // 외부에서 클릭했을 때 이 조건이 true이면 아이템을 버리는 로직 실행

    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //슬롯 생성후 번호를 부여하기위한 Dic
    public void Initialize()//Inventory에서 호출
    {
        sprite = Resources.LoadAll<Sprite>($"ItemImage/Items");
        isMovingChange += () =>
        {
            IsSlotMoving = !IsSlotMoving;
        };
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Craft, new List<GameObject>() }
        };
        slotCount = new Dictionary<Current_Inventory_State, int> // 슬롯 오브젝트에 번호를 부여하기 위한 Dic
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
            newSlot.transform.SetParent(parentTransform.transform, true);
            slots[GameManager.Inventory.State].Add(newSlot);
        } 
    }
  
    public void GetItem(ItemBase item)
    {
        // itemType에 따른 리스트를 가져오기
        List<GameObject> slotList = GetItemTab(item);//item.itemtype에 따른 리스트(장비, 소비, 기타 중 어느곳에 연결된 리스트인지) 연결하기
        UpdateSlot(item, slotList);
    }
    public void DropItem()
    {
        //이미지 emptySlot으로 바꾸기
        //slot.CurrentItem null;
        selectedSlot = null;
    }
    private void UpdateSlot(ItemBase item, List<GameObject> slotList)
    {
        if (item.IsStackable)//한 칸에 여러개 소지 가능한 아이템일 경우 
        {
            foreach(GameObject slotObject in slotList) //리스트를 순회하면서 같은 아이템이 있으면 Count만 증가시키고 return;
            {
                Slot slot = slotObject.GetComponent<Slot>();
                if (item.name == slot.CurrentItem)
                {
                    slot.ItemCount++;
                    return;
                }
            }
        }
        foreach (GameObject slotObject in slotList)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (slot.IsEmpty)
            {
                Image slotImage = slotObject.transform.GetChild(0).GetComponent<Image>();
                string spriteName = Enum.GetName(typeof(ItemImagePath), item.ItemImagePath);          
                foreach (Sprite s in sprite)
                {
                    if (s.name == spriteName)
                    {
                        slotImage.sprite = s;
                        slot.Item = item;
                        break;
                    }
                }
                slot.IsEmpty = false;
                slot.CurrentItem = item.name;
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

    public void OnSlotClicked(Slot clickedSlot)//리턴타입을 slot으로?
    {
        isMovingChange?.Invoke();
        // 첫 클릭: 선택한 슬롯 저장
        if (selectedSlot == null)
        {
            if (!clickedSlot.IsEmpty)
            {
                selectedSlot = clickedSlot;
                firstClickImage = clickedSlot.transform.GetChild(0).GetComponent<Image>(); //이미지의 알파값을 바꾸기 위한 이미지 컴포넌트 알파값변경하는 함수를 따로빼서 옮기는게 좋을것 같다
                firstClickImage.raycastTarget = false;
                firstSlotPosition = clickedSlot.transform.position;
                imageTransform = clickedSlot.transform.GetChild(0).GetComponent<RectTransform>();

                StartCoroutine(ImageMovingCoroutine());//알파값을 moving코루틴에서 바꾼다? 뭔가 이상하다
            }
        }
        // 두 번째 클릭: 아이템 교환하고 선택한 슬롯 초기화
        // 외부에서 클릭했을때(버릴때) 는 ItemSpawner 에서 함수를 실행시킨다.
        else
        {
            secondSlotPosition = clickedSlot.transform.position;

            ResetImageAlpha(); // 이동중인 첫번째 슬롯 알파값 원상복구
            SwapItems(selectedSlot, clickedSlot);
            selectedSlot = null;  
        }
    }
    void ResetImageAlpha()// 이미지 알파값 초기화 
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
        // 두 슬롯이 속한 리스트 가져오기
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

        // 각 슬롯의 인덱스를 찾기.
        int firstSlotIndex = SlotList.IndexOf(firstSlot.gameObject);
        int secondSlotIndex = SlotList.IndexOf(secondSlot.gameObject);

        // 슬롯을 잠시 저장하고 리스트에서 삭제
        GameObject tempFirstSlot = SlotList[firstSlotIndex];
        GameObject tempSecondSlot = SlotList[secondSlotIndex];

        firstSlot.gameObject.transform.position = secondSlotPosition;
        secondSlot.transform.position = firstSlotPosition;


        SlotList.RemoveAt(firstSlotIndex);
        SlotList.Insert(firstSlotIndex, tempSecondSlot);
        SlotList.RemoveAt(secondSlotIndex);
        SlotList.Insert(secondSlotIndex, tempFirstSlot);
        // 삭제한 위치에 다른 슬롯을 추가하여 위치를 바꿈
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

        yield break;
    }
}
