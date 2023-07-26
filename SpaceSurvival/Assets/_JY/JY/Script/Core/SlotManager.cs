using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour // invenSlot,invenSlotUI, SlotUIBase = Slot,     Inventory, InventoryUI = SlotManager
{
    public GameObject slot;
    TempSlot tempSlot;
    public TempSlot TempSlot => tempSlot;
    uint tempSlotIndex = 999999;

    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform craft_Below;

    public delegate void IsMovingChange();
    public IsMovingChange isMovingChange; // Slot�� isMoving �� �� Ŭ������ IsSlotMoving�� �ٲٴ� �Լ� ȣ��


    public bool IsSlotMoving { get; set; } = false; // �ܺο��� Ŭ������ �� �� ������ true�̸� �������� ������ ���� ����

    public Dictionary<Current_Inventory_State, List<Slot>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //���� ������ ��ȣ�� �ο��ϱ����� Dic
    public void Initialize()//Inventory���� ȣ��
    {
        tempSlot = FindObjectOfType<TempSlot>();
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
        slotCount = new Dictionary<Current_Inventory_State, int> // ���� ������Ʈ�� ��ȣ�� �ο��ϱ� ���� Dic
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
            slotComp.onDragBegin += OnItemMoveBegin;
            slotComp.onDragEnd += OnItemMoveEnd;
            slotComp.onClick += OnSlotClick;
           // slotComp.onPointerEnter += OnItemDetailOn;
            //slotComp.onPointerExit += OnItemDetailOff;
            //slotComp.onPointerMove += OnSlotPointerMove;
            slotComp.Index = (uint)slots[GameManager.Inventory.State].Count - 1;  
        }
    }
    private void OnItemMoveBegin(ItemData data, uint index)
    {
        MoveItem(data ,index, tempSlot.Index);    // ���� ���Կ��� �ӽ� �������� ������ �ű��
        tempSlot.Open();                          // �ӽ� ���� ����
    }
    private void OnItemMoveEnd(ItemData data, uint index, bool isSuccess)
    {
        MoveItem(data, tempSlot.Index, index);    // �ӽ� ���Կ��� ���� �������� ������ �ű��
        if (tempSlot.IsEmpty)          // ����ٸ�(���� ������ �������� �� �Ϻθ� ���� ��찡 ���� �� �����Ƿ�)
        {
            tempSlot.Close();                     // �ӽ� ���� �ݱ�
        }

        //if (isSuccess)
        //{
        //    detail.Open(inven[index].ItemData);     // �巡�װ� ���������� �������� �� ����â�� ����
        //}
    }
    private void OnSlotClick(ItemData data, uint index)
    {
        if (tempSlot.IsEmpty)
        {
            //if (isShiftPress)
            //{
            //    OnSpliterOpen(index);   // �ӽ� ���Կ� �������� ���µ� ����Ʈ�� ������ü�� ������ Ŭ������ �� ������ �и�â�� �����
            //}
            //else
            //{
            //    // ������ ���, ��� ���
            //}
        }
        else
        {
            // �ӽ� ���Կ� �������� ���� �� Ŭ���� �Ǿ�����
            OnItemMoveEnd(data, index, true); // Ŭ���� �������� ������ �̵�
        }
    }
    //private void OnItemDetailOn(uint index)
    //{
    //    detail.Open(slotsUI[index].InvenSlot.ItemData); // ������â ����
    //}
    //private void OnItemDetailOff(uint index)
    //{
    //    detail.Close(); // ������â �ݱ�
    //}
    //private void OnSlotPointerMove(Vector2 screenPos)
    //{
    //    detail.MovePosition(screenPos);
    //}
    //private void OnDetailPause(bool isPause)
    //{
    //    detail.IsPause = isPause;
    //}
    //private void OnSpliterOpen(uint index)
    //{

    //    Slot target = slotsUI[index];
    //    spliter.transform.position = target.transform.position + Vector3.up * 100;
    //    spliter.Open(target.InvenSlot);
    //    detail.IsPause = true;
    //}

    /// <summary>
    /// ������ �и�â���� OK ��ư�� �������� �� ����� �Լ�
    /// </summary>
    /// <param name="index">�������� �и��� ����</param>
    /// <param name="count">�и��� ����</param>
    //private void OnSpliterOk(uint index, uint count)
    //{
    //    inven.SplitItem(index, count);
    //    tempSlotUI.Open();
    //}

    /// <summary>
    /// ����ƮŰ�� �������ų� ������ �� ����� �Լ�
    /// </summary>
    /// <param name="context"></param>
    //private void OnShiftPress(InputAction.CallbackContext context)
    //{
    //    isShiftPress = !context.canceled;   // ����ƮŰ ��Ȳ ���
    //}
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
            // ���� ������ �������� �ִ�.
            // ������ ���� 1 ������Ű��� ��� �ޱ�
            result = sameDataSlot.IncreaseSlotItem(out _);  // ��ġ�� ������ �ǹ� ��� ���� ���� ����
        }
        else
        {
            // ���� ������ �������� ����.
            Slot emptySlot = FindEmptySlot(data);
            if (emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); // �󽽷��� ������ ������ �ϳ� �Ҵ�
                result = true;
            }
            else
            {
                // ����ִ� ������ ����.
                //Debug.Log("������ �߰� ���� : �κ��丮�� ���� ���ֽ��ϴ�.");
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
            //Debug.Log($"������ ���� ���� : {slotIndex}�� ���� �ε����Դϴ�.");
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
            //Debug.Log($"������ ���� ���� : {slotIndex}�� ���� �ε����Դϴ�.");
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
        // from������ to������ �ٸ��� from�� to�� ��� valid�ؾ� �Ѵ�.
        if ((from != to) && IsValidIndex(data, from) && IsValidIndex(data, to))
        {
            Slot fromSlot = (from == tempSlotIndex) ? tempSlot : slots[(int)from];  // �ӽ� ������ �����ؼ� ���׿����ڷ� ó��
            if (!fromSlot.IsEmpty)
            {
                Slot toSlot = (to == tempSlotIndex) ? TempSlot : slots[(int)to];
                if (fromSlot.ItemData == toSlot.ItemData)  // ���� ������ �������̸�
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // �ϴ� from�� ���� ������ŭ to ���� �õ�
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from���� ������ �Ѿ ���ڸ�ŭ from ����
                    //Debug.Log($"{from}�� ���Կ��� {to}�� �������� ������ ��ġ��");
                }
                else
                {
                    // �ٸ� ������ �������̸� ���� ����
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssignSlotItem(tempData, tempCount);
                    //Debug.Log($"{from}�� ���԰� {to}�� ������ ������ ��ü");
                }
            }
        }
    }

    /// <summary>
    /// �κ��丮 Ư�� ���Կ��� �������� ������ ����� �ӽ� �������� ������ �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ��� ����</param>
    /// <param name="count">��� ����</param>
    public void SplitItem(ItemData data, uint slotIndex, uint count)
    {
        if (IsValidIndex(data, slotIndex))
        {
            List<Slot> slots = GetItemTab(data);
            Slot slot = slots[(int)slotIndex];
            tempSlot.AssignSlotItem(slot.ItemData, count);  // �ӽ� ���Կ� �Ҵ��ϱ�
            slot.DecreaseSlotItem(count);                   // ���Կ��� �����
        }
    }

    /// <summary>
    /// �κ��丮�� �����ϴ� �Լ� 
    /// </summary>
    /// <param name="sortBy">���� ����</param>
    /// <param name="isAcending">true�� ��������, false�� ��������</param>
    public void SlotSorting(ItemData data ,ItemSortBy sortBy, bool isAcending = true)
    {
        List<Slot> slots = GetItemTab(data);
        List<Slot> beforeSlots = new List<Slot>(slots);   // slots �迭�� �̿��ؼ� ����Ʈ �����

        switch (sortBy) // ���� ���ؿ� ���� �ٸ��� ó���ϱ�(Sort �Լ��� �Ķ���ͷ� �� �����Լ��� �ٸ��� �ۼ�)
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) =>  // x, y�� ���� ���� 2��(beforeSlots�� ����ִ� 2��)
                {
                    if (x.ItemData == null) // itemData�� ������� �� ������ ��������� ����ִ� ���� �������� ����
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);  // enum�� ������ CompareTo �Լ��� ��(���������� ��)
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);  // enum�� ������ CompareTo �Լ��� ��(���������� ��)
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
        // beforeSlots�� ������ ���ؿ� ���� ���� �Ϸ�

        //// ������ ������ ������ ���� �����ϱ�
        //List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(SlotCount);
        //foreach(var slot in beforeSlots)
        //{
        //    sortedData.Add((slot.ItemData, slot.ItemCount));
        //}

        //// ���Կ� ������ ������ ������ ������� �Ҵ��ϱ�
        //int index = 0;
        //foreach(var data in sortedData)
        //{
        //    slots[index].AssignSlotItem(data.Item1, data.Item2);
        //    index++;
        //}

        // ���� �Ϸ�� ���� �ٽ� �迭�� �����
        slots = beforeSlots;
        RefreshInventory(data);
    }

    /// <summary>
    /// ��� ������ ����Ǿ����� �˸��� �Լ�
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
        foreach (var slot in slots)  // ��� ������ �� ���鼭
        {
            if (slot.ItemData == data && slot.ItemCount < slot.ItemData.maxStackCount)  // itemData�� ���� ���� ������ ������ ã�Ҵ�.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// �κ��丮���� ����ִ� ������ ã�� �Լ�
    /// </summary>
    /// <returns>����ִ� ����(ù��°)</returns>
    Slot FindEmptySlot(ItemData data)
    {
        Slot findSlot = null;
        List<Slot> slots = GetItemTab(data);
        foreach (var slot in slots)     // ��� ������ �� ���鼭
        {
            if (slot.IsEmpty)            // ����ִ� ������ ������ ã�Ҵ�.// Slot�� IsEmpty�� �ʱ� false�� ����  ItemData�� null �ε� �� false���� �� �𸣰ڴ�.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// ������ �ε������� Ȯ���ϴ� �Լ�
    bool IsValidIndex(ItemData data ,uint index)
    {
        List<Slot> slots = GetItemTab(data);
        if (index < slots.Count - 1 || index == tempSlotIndex)
        {
            return true;
        }
        return false;
    } 

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
}
