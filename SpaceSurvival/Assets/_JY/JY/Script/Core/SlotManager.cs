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

    Slot selectedSlot;// ó�� Ŭ���� ������ �����ϱ� ���� ����
    Image firstClickImage; //ù��° Ŭ���� ���� ������ ������ �̹���

    Sprite[] sprite; //������ �̹����� �迭
    RectTransform imageTransform; 

    Vector2 firstSlotPosition;
    Vector2 secondSlotPosition;

    public delegate void IsMovingChange();
    public IsMovingChange isMovingChange; // Slot�� isMoving �� �� Ŭ������ IsSlotMoving�� �ٲٴ� �Լ� ȣ��


    public bool IsSlotMoving { get; set; } = false; // �ܺο��� Ŭ������ �� �� ������ true�̸� �������� ������ ���� ����

    public Dictionary<Current_Inventory_State, List<Slot>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //���� ������ ��ȣ�� �ο��ϱ����� Dic
    public void Initialize()//Inventory���� ȣ��
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

    /// <summary>
    /// ������ �ε������� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>true�� ������ �ε���, false�� ���� �ε���</returns>
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
    //    //�̹��� emptySlot���� �ٲٱ�
    //    //slot.CurrentItem null;
    //    //List���� �����
    //    ItemData item = selectedSlot.ItemData;
    //    List<Slot> slotList = GetItemTab(item);
    //    UpdateSlot(item, slotList, false);
    //    selectedSlot = null;
    //}
    //public void OnSlotClicked(Slot clickedSlot)//����Ÿ���� slot����?
    //{
    //    isMovingChange?.Invoke();
    //    // ù Ŭ��: ������ ���� ����
    //    if (selectedSlot == null)
    //    {
    //        if (!clickedSlot.IsEmpty)
    //        {
    //            selectedSlot = clickedSlot;
    //            firstClickImage = clickedSlot.transform.GetChild(0).GetComponent<Image>(); //�̹����� ���İ��� �ٲٱ� ���� �̹��� ������Ʈ ���İ������ϴ� �Լ��� ���λ��� �ű�°� ������ ����
    //            firstClickImage.raycastTarget = false;
    //            firstSlotPosition = clickedSlot.transform.position;
    //            imageTransform = clickedSlot.transform.GetChild(0).GetComponent<RectTransform>();

    //            ResetImageAlpha();
    //            StartCoroutine(ImageMovingCoroutine());//���İ��� moving�ڷ�ƾ���� �ٲ۴�? ���� �̻��ϴ�
    //        }
    //    }
    //    // �� ��° Ŭ��: ������ ��ȯ�ϰ� ������ ���� �ʱ�ȭ
    //    // �ܺο��� Ŭ��������(������) �� ItemSpawner ���� �Լ��� �����Ų��.
    //    else
    //    {
    //        secondSlotPosition = clickedSlot.transform.position;

    //        ResetImageAlpha(); // �̵����� ù��° ���� ���İ� ���󺹱�
    //        SwapItems(selectedSlot, clickedSlot);
    //        selectedSlot = null;
    //    }
    //}
    //private void UpdateSlot(ItemData item, List<Slot> slotList, bool getItem)
    //{
    //    if (item.maxStackCount > 1)//�� ĭ�� ������ ���� ������ �������� ��� 
    //    {
    //        if (getItem)
    //        {
    //            foreach (GameObject slotObject in slotList) //����Ʈ�� ��ȸ�ϸ鼭 ���� �������� ������ Count�� ������Ű�� return;
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
    //            CheckGetOrDrop(item, slotList, getItem);// �̺κп��� ��� �������� �˾��ϰ� ���� CheckGetOrDrop�� ȣ���� �ʿ���� �ٷ� ChangeSprite�� ȣ���ϸ� �ǰڴ�.
    //            return;
    //        }
    //    }
    //    CheckGetOrDrop(item, slotList, getItem);
    //}

    private void CheckGetOrDrop(ItemData item, List<GameObject> slotList, bool getItem)
    {
        Slot slot;
        if (getItem)//ȹ��
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

    void ResetImageAlpha()// �̹��� ���İ� �ʱ�ȭ 
    {
        var color = firstClickImage.color;
        color.a = IsSlotMoving? 0.5f : 1.0f;
        firstClickImage.color = color;
    }
    //void SwapItems(Slot firstSlot, Slot secondSlot)
    //{
    //    // �� ������ ���� ����Ʈ ��������
    //    List<GameObject> SlotList = GetItemTab(firstSlot);

    //    // �� ������ �ε����� ã��.
    //    int firstSlotIndex = SlotList.IndexOf(firstSlot.gameObject);
    //    int secondSlotIndex = SlotList.IndexOf(secondSlot.gameObject);

    //    // ������ ��� �����ϰ� ����Ʈ���� ����
    //    GameObject tempFirstSlot = SlotList[firstSlotIndex];
    //    GameObject tempSecondSlot = SlotList[secondSlotIndex];

    //    firstSlot.gameObject.transform.position = secondSlotPosition;
    //    secondSlot.transform.position = firstSlotPosition;


    //    SlotList.RemoveAt(firstSlotIndex);
    //    SlotList.Insert(firstSlotIndex, tempSecondSlot);
    //    SlotList.RemoveAt(secondSlotIndex);
    //    SlotList.Insert(secondSlotIndex, tempFirstSlot);
    //    // ������ ��ġ�� �ٸ� ������ �߰��Ͽ� ��ġ�� �ٲ�
    //}
    IEnumerator ImageMovingCoroutine()
    {
        while (selectedSlot != null)
        {
            imageTransform.position = Input.mousePosition;
            yield return null;
        }
        imageTransform.anchoredPosition = Vector2.zero;
        IsSlotMoving = false;// ���İ� �ʱ�ȭ ������ ����
        ResetImageAlpha();
        yield break;
    }
}
