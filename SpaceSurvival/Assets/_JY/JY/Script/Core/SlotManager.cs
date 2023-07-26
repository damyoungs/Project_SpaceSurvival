using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour // invenSlot,invenSlotUI, SlotUIBase = Slot,     Inventory, InventoryUI = SlotManager
{
    public GameObject slot;
    Slot tempSlot;
    public Slot TempSlot => tempSlot;
    uint tempSlotIndex = 999999;

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
    public IsMovingChange isMovingChange; // Slot의 isMoving 과 이 클래스의 IsSlotMoving을 바꾸는 함수 호출


    public bool IsSlotMoving { get; set; } = false; // 외부에서 클릭했을 때 이 조건이 true이면 아이템을 버리는 로직 실행

    public Dictionary<Current_Inventory_State, List<Slot>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //슬롯 생성후 번호를 부여하기위한 Dic
    public void Initialize()//Inventory에서 호출
    {
        tempSlot = FindObjectOfType<Slot>();
        isMovingChange += () =>
        {
            IsSlotMoving = !IsSlotMoving;
        };
        slots = new Dictionary<Current_Inventory_State, List<Slot>>
        {
            { Current_Inventory_State.Equip, new List<Slot>() },
            { Current_Inventory_State.Consume, new List<Slot>() },
            { Current_Inventory_State.Etc, new List<Slot>() },
            { Current_Inventory_State.Craft, new List<Slot>() }
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
        Slot slotComp = newSlot.GetComponent<Slot>();
        Transform parentTransform = GetParentTransform();
        if (parentTransform != null)
        {
            slotCount[GameManager.Inventory.State]++;
            newSlot.name = $"{GameManager.Inventory.State}_{slotCount[GameManager.Inventory.State]}";           
            newSlot.transform.SetParent(parentTransform.transform, true);
            slots[GameManager.Inventory.State].Add(slotComp);

        
            slotComp.InitializeSlot(slotComp);
            //slotComp.onDragBegin += OnItemMoveBegin;
            //slotComp.onDragEnd += OnItemMoveEnd;
            //slotComp.onClick += OnSlotClick;
            //slotComp.onPointerEnter += OnItemDetailOn;
            //slotComp.onPointerExit += OnItemDetailOff;
            //slotComp.onPointerMove += OnSlotPointerMove;
            slotComp.Index = (uint)slots[GameManager.Inventory.State].Count - 1;  
        }
    }

    private Transform GetParentTransform()
    {
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
        return parentTransform;
    }


    public bool AddItem(ItemCode code)
    {
        bool result = false;
        ItemData data = GameManager.Itemdata[code];

        Slot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null)
        {
            // 같은 종류의 아이템이 있다.
            // 아이템 개수 1 증가시키기고 결과 받기
            result = sameDataSlot.IncreaseSlotItem(out _);  // 넘치는 개수가 의미 없어서 따로 받지 않음
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            Slot emptySlot = FindEmptySlot(data);
            if (emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); // 빈슬롯이 있으면 아이템 하나 할당
                result = true;
            }
            else
            {
                // 비어있는 슬롯이 없다.
                //Debug.Log("아이템 추가 실패 : 인벤토리가 가득 차있습니다.");
            }
        }

        return result;
    }
    public void RemoveItem(ItemData data, uint slotIndex, uint decreaseCount = 1)
    {
        List<Slot> slots = GetItemTab(data);
        if (IsValidIndex(data, slotIndex))
        {
            Slot invenSlot = slots[(int)slotIndex];
            invenSlot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            //Debug.Log($"아이템 감소 실패 : {slotIndex}는 없는 인덱스입니다.");
        }
    }
    public void ClearSlot(ItemData data, uint slotIndex)
    {
        List<Slot> slots = GetItemTab(data);
        if (IsValidIndex(data, slotIndex))
        {
            Slot invenSlot = slots[(int)slotIndex];
            invenSlot.ClearSlotItem();
        }
        else
        {
            //Debug.Log($"아이템 삭제 실패 : {slotIndex}는 없는 인덱스입니다.");
        }
    }
    public void ClearInventory(ItemData data)
    {
        List<Slot> slots = GetItemTab(data);
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    public void MoveItem(ItemData data, uint from, uint to)
    {
        List<Slot> slots = GetItemTab(data);
        // from지점과 to지점이 다르고 from과 to가 모두 valid해야 한다.
        if ((from != to) && IsValidIndex(data, from) && IsValidIndex(data, to))
        {
            Slot fromSlot = (from == tempSlotIndex) ? tempSlot : slots[(int)from];  // 임시 슬롯을 감안해서 삼항연산자로 처리
            if (!fromSlot.IsEmpty)
            {
                Slot toSlot = (to == tempSlotIndex) ? TempSlot : slots[(int)to];
                if (fromSlot.ItemData == toSlot.ItemData)  // 같은 종류의 아이템이면
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 일단 from이 가진 개수만큼 to 감소 시도
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 실제로 넘어간 숫자만큼 from 감소
                    //Debug.Log($"{from}번 슬롯에서 {to}번 슬롯으로 아이템 합치기");
                }
                else
                {
                    // 다른 종류의 아이템이면 서로 스왑
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssignSlotItem(tempData, tempCount);
                    //Debug.Log($"{from}번 슬롯과 {to}번 슬롯의 아이템 교체");
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리 특정 슬롯에서 아이템을 일정량 덜어내어 임시 슬롯으로 보내는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 덜어낼 슬롯</param>
    /// <param name="count">덜어낼 개수</param>
    public void SplitItem(ItemData data, uint slotIndex, uint count)
    {
        if (IsValidIndex(data, slotIndex))
        {
            List<Slot> slots = GetItemTab(data);
            Slot slot = slots[(int)slotIndex];
            tempSlot.AssignSlotItem(slot.ItemData, count);  // 임시 슬롯에 할당하기
            slot.DecreaseSlotItem(count);                   // 슬롯에서 덜어내고
        }
    }

    /// <summary>
    /// 인벤토리를 정렬하는 함수 
    /// </summary>
    /// <param name="sortBy">정렬 기준</param>
    /// <param name="isAcending">true면 오름차순, false면 내림차순</param>
    public void SlotSorting(ItemData data ,ItemSortBy sortBy, bool isAcending = true)
    {
        List<Slot> slots = GetItemTab(data);
        List<Slot> beforeSlots = new List<Slot>(slots);   // slots 배열을 이용해서 리스트 만들기

        switch (sortBy) // 정렬 기준에 따라 다르게 처리하기(Sort 함수의 파라메터로 들어갈 람다함수를 다르게 작성)
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) =>  // x, y는 서로 비교할 2개(beforeSlots에 들어있는 2개)
                {
                    if (x.ItemData == null) // itemData는 비어있을 수 있으니 비어있으면 비어있는 것이 뒤쪽으로 설정
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);  // enum이 가지는 CompareTo 함수로 비교(오름차순일 때)
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);  // enum이 가지는 CompareTo 함수로 비교(내림차순일 때)
                    }
                });
                break;
            case ItemSortBy.Name:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.price.CompareTo(y.ItemData.price);
                    }
                    else
                    {
                        return y.ItemData.price.CompareTo(x.ItemData.price);
                    }
                });
                break;
        }
        // beforeSlots은 정해진 기준에 따라 정렬 완료

        //// 아이템 종류와 개수를 따로 저장하기
        //List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(SlotCount);
        //foreach(var slot in beforeSlots)
        //{
        //    sortedData.Add((slot.ItemData, slot.ItemCount));
        //}

        //// 슬롯에 아이템 종류와 개수를 순서대로 할당하기
        //int index = 0;
        //foreach(var data in sortedData)
        //{
        //    slots[index].AssignSlotItem(data.Item1, data.Item2);
        //    index++;
        //}

        // 정렬 완료된 것을 다시 배열로 만들기
        slots = beforeSlots;
        RefreshInventory(data);
    }

    /// <summary>
    /// 모든 슬롯이 변경되었음을 알리는 함수
    /// </summary>
    void RefreshInventory(ItemData data)
    {
        List<Slot> slots = GetItemTab(data);
        foreach (var slot in slots)
        {
            slot.onValueChange?.Invoke();
        }
    }
    Slot FindSameItem(ItemData data)
    {
        List<Slot> slots = GetItemTab(data);
        Slot findSlot = null;
        foreach (var slot in slots)  // 모든 슬롯을 다 돌면서
        {
            if (slot.ItemData == data && slot.ItemCount < slot.ItemData.maxStackCount)  // itemData가 같고 여유 공간이 있으면 찾았다.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 인벤토리에서 비어있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 슬롯(첫번째)</returns>
    Slot FindEmptySlot(ItemData data)
    {
        Slot findSlot = null;
        List<Slot> slots = GetItemTab(data);
        foreach (var slot in slots)     // 모든 슬롯을 다 돌면서
        {
            if (slot.IsEmpty)            // 비어있는 슬롯이 있으면 찾았다.// Slot의 IsEmpty가 초기 false인 문제  ItemData가 null 인데 왜 false인지 잘 모르겠다.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스, false면 없는 인덱스</returns>
    bool IsValidIndex(ItemData data ,uint index)
    {
        List<Slot> slots = GetItemTab(data);
        if (index < slots.Count - 1 || index == tempSlotIndex)
        {
            return true;
        }
        return false;
    } 

    //public void DropItem()
    //{
    //    //이미지 emptySlot으로 바꾸기
    //    //slot.CurrentItem null;
    //    //List에서 지우기
    //    ItemData item = selectedSlot.ItemData;
    //    List<Slot> slotList = GetItemTab(item);
    //    UpdateSlot(item, slotList, false);
    //    selectedSlot = null;
    //}
    //public void OnSlotClicked(Slot clickedSlot)//리턴타입을 slot으로?
    //{
    //    isMovingChange?.Invoke();
    //    // 첫 클릭: 선택한 슬롯 저장
    //    if (selectedSlot == null)
    //    {
    //        if (!clickedSlot.IsEmpty)
    //        {
    //            selectedSlot = clickedSlot;
    //            firstClickImage = clickedSlot.transform.GetChild(0).GetComponent<Image>(); //이미지의 알파값을 바꾸기 위한 이미지 컴포넌트 알파값변경하는 함수를 따로빼서 옮기는게 좋을것 같다
    //            firstClickImage.raycastTarget = false;
    //            firstSlotPosition = clickedSlot.transform.position;
    //            imageTransform = clickedSlot.transform.GetChild(0).GetComponent<RectTransform>();

    //            ResetImageAlpha();
    //            StartCoroutine(ImageMovingCoroutine());//알파값을 moving코루틴에서 바꾼다? 뭔가 이상하다
    //        }
    //    }
    //    // 두 번째 클릭: 아이템 교환하고 선택한 슬롯 초기화
    //    // 외부에서 클릭했을때(버릴때) 는 ItemSpawner 에서 함수를 실행시킨다.
    //    else
    //    {
    //        secondSlotPosition = clickedSlot.transform.position;

    //        ResetImageAlpha(); // 이동중인 첫번째 슬롯 알파값 원상복구
    //        SwapItems(selectedSlot, clickedSlot);
    //        selectedSlot = null;
    //    }
    //}
    //private void UpdateSlot(ItemData item, List<Slot> slotList, bool getItem)
    //{
    //    if (item.maxStackCount > 1)//한 칸에 여러개 소지 가능한 아이템일 경우 
    //    {
    //        if (getItem)
    //        {
    //            foreach (GameObject slotObject in slotList) //리스트를 순회하면서 같은 아이템이 있으면 Count만 증가시키고 return;
    //            {
    //                //Slot slot = slotObject.GetComponent<Slot>();
    //                //if (item.itemName == slot.CurrentItem)
    //                //{
    //                //    slot.ItemCount++;
    //                //    return;
    //                //}
    //            }
    //        }
    //        else
    //        {
    //            CheckGetOrDrop(item, slotList, getItem);// 이부분에서 몇개를 버릴건지 팝업하고 굳이 CheckGetOrDrop을 호출할 필요없이 바로 ChangeSprite을 호출하면 되겠다.
    //            return;
    //        }
    //    }
    //    CheckGetOrDrop(item, slotList, getItem);
    //}

    private void CheckGetOrDrop(ItemData item, List<GameObject> slotList, bool getItem)
    {
        Slot slot;
        if (getItem)//획득
        {
            foreach (GameObject slotObject in slotList)
            {
                slot = slotObject.GetComponent<Slot>();
                if (slot.IsEmpty)
                {
                   // ChangeSprite(slot);
                    break;
                }         
            }
        }
        else
        {
           // ChangeSprite(selectedSlot);         
        }
    }

    //private void ChangeSprite(Slot slot)
    //{
    //    if (slot.IsEmpty)
    //    {
    //        slot.Item.itemIcon.
    //    }
    //}

    private List<Slot> GetItemTab(ItemData item)
    {
        List<Slot> slotList;
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
    private List<Slot> GetItemTab(Slot slot)
    {
        ItemData item = slot.ItemData;
        List<Slot> slotList = GetItemTab(item);
        return slotList;
    }

    void ResetImageAlpha()// 이미지 알파값 초기화 
    {
        var color = firstClickImage.color;
        color.a = IsSlotMoving? 0.5f : 1.0f;
        firstClickImage.color = color;
    }
    //void SwapItems(Slot firstSlot, Slot secondSlot)
    //{
    //    // 두 슬롯이 속한 리스트 가져오기
    //    List<GameObject> SlotList = GetItemTab(firstSlot);

    //    // 각 슬롯의 인덱스를 찾기.
    //    int firstSlotIndex = SlotList.IndexOf(firstSlot.gameObject);
    //    int secondSlotIndex = SlotList.IndexOf(secondSlot.gameObject);

    //    // 슬롯을 잠시 저장하고 리스트에서 삭제
    //    GameObject tempFirstSlot = SlotList[firstSlotIndex];
    //    GameObject tempSecondSlot = SlotList[secondSlotIndex];

    //    firstSlot.gameObject.transform.position = secondSlotPosition;
    //    secondSlot.transform.position = firstSlotPosition;


    //    SlotList.RemoveAt(firstSlotIndex);
    //    SlotList.Insert(firstSlotIndex, tempSecondSlot);
    //    SlotList.RemoveAt(secondSlotIndex);
    //    SlotList.Insert(secondSlotIndex, tempFirstSlot);
    //    // 삭제한 위치에 다른 슬롯을 추가하여 위치를 바꿈
    //}
    IEnumerator ImageMovingCoroutine()
    {
        while (selectedSlot != null)
        {
            imageTransform.position = Input.mousePosition;
            yield return null;
        }
        imageTransform.anchoredPosition = Vector2.zero;
        IsSlotMoving = false;// 알파값 초기화 를위해 변경
        ResetImageAlpha();
        yield break;
    }
}
