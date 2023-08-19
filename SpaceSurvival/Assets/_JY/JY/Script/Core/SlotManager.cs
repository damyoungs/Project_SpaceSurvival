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
    InputKeyMouse input;

    public GameObject slot;
    public TempSlot tempSlot;
    public ItemDescription itemDescription;
    public RectTransform inventoryRectTransform;

    public TempSlot TempSlot => tempSlot;

    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform craft_Below;

   
    RectTransform beforeSlotRectTransform;
    RectTransform enhancerUIRectTransform;

    RectTransform mixer_Left_slot_Transform;
    RectTransform mixer_Middle_Slot_Transform;
    RectTransform mixerUI_Transform;
    Item_Mixer_UI mixer_UI;

    public ItemSplitter spliter;
    bool isShiftPress = false;

    public Action<ItemData_Potion, uint> onDetectQuickSlot;

    public Dictionary<Current_Inventory_State, List<Slot>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //���� ������ ��ȣ�� �ο��ϱ����� Dic

 
    public byte Index_JustChange_Slot { get; set; }
  

    private void Awake()
    {
        input = new InputKeyMouse();
    }
    private void OnEnable()
    {
        input.UI.Enable();
        input.UI.Click.canceled += OnItemDrop;
        input.UI.Shift.performed += OnShiftPress;
        input.UI.Shift.canceled += OnShiftPress;
    }
    private void OnDisable()
    {
        input.UI.Click.canceled -= OnItemDrop;
        input.UI.Shift.performed -= OnShiftPress;
        input.UI.Shift.canceled -= OnShiftPress;
        input.UI.Disable();
    }
    public void Initialize()//Inventory���� StartŸ�ֿ̹� ȣ��
    {
        itemDescription.Close();
        TempSlot.InitializeSlot(TempSlot);
        TempSlot.onTempSlotOpenClose += OnDetailPause; // TempSlot�� Open�Ҷ� true�� ȣ���ϰ� Close�Ҷ� false�� ȣ��
        spliter.onCancel += () => itemDescription.IsPause = false;   // ĵ������ ������ ������â �Ͻ����� ����
        spliter.Close();
        spliter.onOkClick += OnSpliterOk;

        beforeSlotRectTransform = GameManager.Enhancer.EnhancerUI.BeforeSlot.GetComponent<RectTransform>();
        enhancerUIRectTransform = GameManager.Enhancer.EnhancerUI.AfterSlot.GetComponent<RectTransform>();
   
        mixer_Left_slot_Transform = GameManager.Mixer.MixerUI.Left_Slot.GetComponent<RectTransform>();
        mixer_Middle_Slot_Transform = GameManager.Mixer.MixerUI.Middle_Slot.GetComponent<RectTransform>();
        mixerUI_Transform = GameManager.Mixer.GetComponent<RectTransform>();
        mixer_UI = GameManager.Mixer.MixerUI;

        mixer_UI.onEndSession_Success += Add_Reward_Item;

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
            slotComp.onPointerEnter += OnItemDetailOn;
            slotComp.onPointerExit += OnItemDetailOff;
            slotComp.onPointerMove += OnSlotPointerMove;
            slotComp.Index = (uint)slots[GameManager.Inventory.State].Count - 1;  
        }
    }
    private void OnItemMoveBegin(ItemData data, uint index)
    {
        Index_JustChange_Slot = (byte)index;
        MoveItem(data ,index, tempSlot.Index);    // ���� ���Կ��� �ӽ� �������� ������ �ű��
        TempSlot.Open();                          // �ӽ� ���� ����
    }
    private void OnItemMoveEnd(ItemData data, uint index, bool isSuccess)
    {
        MoveItem(data, tempSlot.Index, index);    // �ӽ� ���Կ��� ���� �������� ������ �ű��
        if (tempSlot.IsEmpty)          // ����ٸ�(���� ������ �������� �� �Ϻθ� ���� ��찡 ���� �� �����Ƿ�)
        {
            TempSlot.Close();                     // �ӽ� ���� �ݱ�
        }

        if (isSuccess)
        {
            List<Slot> slots = GetItemTab();
            itemDescription.Open(slots[(int)index].ItemData);     // �巡�װ� ���������� �������� �� ����â�� ����
        }
    }
    private void OnSlotClick(ItemData data, uint index)
    {
        ItemData_Potion potion = data as ItemData_Potion;
        if (potion != null)
        {
            potion.Consume(GameManager.playerDummy);
            RemoveItem(data, index);
        }
        else if (TempSlot.IsEmpty)
        {
            if (isShiftPress)
            {
                OnSpliterOpen(index);   // �ӽ� ���Կ� �������� ���µ� ����Ʈ�� ������ü�� ������ Ŭ������ �� ������ �и�â�� �����
            }
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
    private void OnItemDetailOn( ItemData data, uint index)
    {
        List<Slot> slots = GetItemTab(data); //�󽽷� ���� Pointer Enter�� data�� null �̵Ǽ� ����Ʈ�� �����ö� ����

        itemDescription.Open(slots[(int)index].ItemData); // ������â ����
    }
    private void OnItemDetailOff(uint index)
    {
        itemDescription.Close(); // ������â �ݱ�
    }
    private void OnSlotPointerMove(Vector2 screenPos)//���콺 �����϶����� ȣ��
    {
        itemDescription.MovePosition(screenPos);
    }
    private void OnDetailPause(bool isPause)
    {
        itemDescription.IsPause = isPause;
    }
    /// <summary>
    /// ������ �и�â�� ���� �Լ�
    /// </summary>
    /// <param name="index">�������� �и��� ������ �ε���</param>
    private void OnSpliterOpen(uint index)
    {
        List<Slot> slots = GetItemTab();
        Slot target = slots[(int)index];
        spliter.transform.position = target.transform.position + Vector3.up * 100;
        spliter.Open(target);
        itemDescription.IsPause = true;
    }

    /// <summary>
    /// ������ �и�â���� OK ��ư�� �������� �� ����� �Լ�
    /// </summary>
    /// <param name="index">�������� �и��� ����</param>
    /// <param name="count">�и��� ����</param>
    private void OnSpliterOk(uint index, uint count)
    {
        SplitItem(index, count);
        TempSlot.Open();
    }

    /// <summary>
    /// ����ƮŰ�� �������ų� ������ �� ����� �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void OnShiftPress(InputAction.CallbackContext context)
    {
        isShiftPress = !context.canceled;   // ����ƮŰ ��Ȳ ���
    }


    /// <summary>
    /// ���콺 Ŭ���� �������� �� ����Ǵ� �Լ�(������ �����)
    /// </summary>
    private void OnItemDrop(InputAction.CallbackContext _)
    {
        if (tempSlot == null)
            return;
        if (!tempSlot.IsEmpty)
        {
            //���̸� ��� ���� ������ ��ǥ�� ��ũ����ǥ�� �ٲ������ �׷��� ���� ��ġ�� ���ϴ� �� ���� ���̸� ���� ���� ������ ����.
            //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            //if (Physics.Raycast(ray, out RaycastHit ground, 1000.0f, LayerMask.GetMask("BeforeSlot")))
            //{
            //    Debug.Log("BeforeSlot����");
            //}
            //else if ((Physics.Raycast(ray, out RaycastHit beforeSlot, 1000.0f, LayerMask.GetMask("Ground"))))
            //{
            //    Debug.Log("Ground");
            //}
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 distance_Between_Mouse_Inven = screenPos - (Vector2)inventoryRectTransform.position;//inventoryRectTransform.position�� �Ǻ��� �������� �������Ÿ� ���
            Vector2 distance_Between_Mouse_BeforeSlot = screenPos - (Vector2)beforeSlotRectTransform.position;
            Vector2 distance_Between_Mouse_enhancerUI = screenPos - (Vector2)enhancerUIRectTransform.position;
            Vector2 distance_Between_Mouse_Left_Slot = screenPos - (Vector2)mixer_Left_slot_Transform.position;
            Vector2 distance_Between_Mouse_Middle_Slot = screenPos - (Vector2)mixer_Middle_Slot_Transform.position;
            Vector2 distance_Between_Mouse_MixerUI = screenPos - (Vector2)mixerUI_Transform.position;
            Vector2 distance_Between_Mouse_QuickSlot_Box = screenPos - (Vector2)GameManager.QuickSlot_Box.QuickSlotBox_RectTransform.position;

            if (beforeSlotRectTransform.rect.Contains(distance_Between_Mouse_BeforeSlot) && GameManager.Enhancer.EnhancerUI.IsOpen)//��ȭ ������ ��ġ�̸鼭 ��ȭ�� ������ ������ �� ��
            {
                ItemData_Enhancable enhancable = TempSlot.ItemData as ItemData_Enhancable;
                if (enhancable != null)
                {
                    GameManager.Enhancer.ItemData = enhancable;
                   // IndexForEnhancer =  ���ʿ� ������� Slot �� TempSlot���� �Űܰ��� �ε��� ������ �ʿ���
                }
            }
            else if (GameManager.QuickSlot_Box.QuickSlotBox_RectTransform.rect.Contains(distance_Between_Mouse_QuickSlot_Box))
            {
                //��ų�̳� ���Ǹ� ���
                ItemData_Potion potion = TempSlot.ItemData as ItemData_Potion;
                if (potion != null)
                {
                    onDetectQuickSlot?.Invoke(potion, TempSlot.ItemCount);
                }
            }
            else if (mixer_Left_slot_Transform.rect.Contains(distance_Between_Mouse_Left_Slot) && mixer_UI.IsOpen)//����â�� ���ʽ���
            {
                GameManager.Mixer.LeftSlotData = TempSlot.ItemData;
                RemoveItem(TempSlot.ItemData, Index_JustChange_Slot);
            }
            else if (mixer_Middle_Slot_Transform.rect.Contains(distance_Between_Mouse_Middle_Slot) && mixer_UI.IsOpen)
            {
                GameManager.Mixer.MiddleSlotData = TempSlot.ItemData;
                RemoveItem(TempSlot.ItemData, Index_JustChange_Slot);
            }
            else if (mixerUI_Transform.rect.Contains(distance_Between_Mouse_MixerUI) && mixer_UI.IsOpen)
            {
                return;
            }
            else if (!inventoryRectTransform.rect.Contains(distance_Between_Mouse_Inven))// �Ÿ��� ũ�Ⱑ rect �� ũ�⺸�� ������ �κ��丮 ����
            {
                if (enhancerUIRectTransform.rect.Contains(distance_Between_Mouse_enhancerUI) && GameManager.Enhancer.EnhancerState == EnhancerState.Open)//inhancerUI���������� return
                {
                    return;
                }
                // �κ��丮 ���� ���̸�
                TempSlot.OnDrop(screenPos);
            }
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

    void Add_Reward_Item(ItemData item)//���ռ����� ������ �߰�
    {
        AddItem(item.code);
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
    public bool AddItem(ItemData_Enhancable data)
    {
        bool result = false;
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
        return result;
    }
    public void RemoveItem(ItemData data, uint slotIndex, uint decreaseCount = 1)
    {
        List<Slot> slots = GetItemTab(data);
        if (IsValidIndex(slotIndex, data))
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
        if (IsValidIndex(slotIndex, data))
        {
            Slot invenSlot = slots[(int)slotIndex];
            invenSlot.ClearSlotItem();
        }
        else
        {
            //Debug.Log($"������ ���� ���� : {slotIndex}�� ���� �ε����Դϴ�.");
        }
    }
    public void ClearInventory()
    {
        List<Slot> slots = GetItemTab();
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    public void MoveItem(ItemData data, uint from, uint to)
    {
        if (data == null)
        {
            data = TempSlot.ItemData;
        }
        List<Slot> slots = GetItemTab(data);
        // from������ to������ �ٸ��� from�� to�� ��� valid�ؾ� �Ѵ�.
        if ((from != to) && IsValidIndex(from, data) && IsValidIndex(to, data))
        {
            Slot fromSlot = (from == tempSlot.Index) ? tempSlot : slots[(int)from];  // �ӽ� ������ �����ؼ� ���׿����ڷ� ó��
            if (!fromSlot.IsEmpty)
            {
                Slot toSlot = (to == tempSlot.Index) ? TempSlot : slots[(int)to];
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
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);// �̶� ������ �����Ͱ� null�� �ȴ�.
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
    public void SplitItem(uint slotIndex, uint count) // ���ø��Ҷ��� ���� 
    {
        if (IsValidIndex(slotIndex))
        {
            List<Slot> slots = GetItemTab();
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
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        List<Slot> slots = GetItemTab();
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

        // ������ ������ ������ ���� �����ϱ�
        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(slots.Count);
        foreach (var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        // ���Կ� ������ ������ ������ ������� �Ҵ��ϱ�
        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);
            index++;
        }//���� ����κ�

        // ���� �Ϸ�� ���� �ٽ� �迭�� �����
       // slots = beforeSlots;
        RefreshInventory();
    }

    /// <summary>
    /// ��� ������ ����Ǿ����� �˸��� �Լ�
    /// </summary>
    void RefreshInventory()
    {
        List<Slot> slots = GetItemTab();
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
            if (slot.ItemData != null)
            {
                if (slot.ItemData.code == data.code && slot.ItemCount < slot.ItemData.maxStackCount)  // itemData�� ���� ���� ������ ������ �� ������ �����Ѵ�
                {
                    findSlot = slot;
                    break;
                }
            }
            else
            {
                continue;
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
    bool IsValidIndex(uint index, ItemData data = null)
    {
        List<Slot> slots = GetItemTab(data);
        if (index < slots.Count || index == tempSlot.Index)
        {
            return true;
        }
        return false;
    } 

    private List<Slot> GetItemTab(ItemData item = null)
    {
        List<Slot> slotList;
        if (item != null) // �� �Լ��� ȣ���� �� itemdata�� null �̸�  �κ��丮�� ���� ���õ� ���� ����Ʈ�� �����Ѵ�.
        {
            switch (item.ItemType) // null �� �ƴϸ� Inventory Ŭ�������� ���� � ���� ���õǾ��� ������� item�� itemType�� ���� ����Ʈ�� ���� �Ѵ�.
            {
                case ItemType.Equip:
                    return slotList = slots[Current_Inventory_State.Equip];
                case ItemType.Consume:
                    return slotList = slots[Current_Inventory_State.Consume];
                case ItemType.Etc:
                    return slotList = slots[Current_Inventory_State.Etc];
                case ItemType.Craft:
                    return slotList = slots[Current_Inventory_State.Craft];
                default:
                    break;
            }
        }
        else
        {      
            switch (GameManager.Inventory.State)
            {
                case Current_Inventory_State.Equip:
                    return slotList = slots[Current_Inventory_State.Equip];
                case Current_Inventory_State.Consume:
                    return slotList = slots[Current_Inventory_State.Consume];
                case Current_Inventory_State.Etc:
                    return slotList = slots[Current_Inventory_State.Etc];
                case Current_Inventory_State.Craft:
                    return slotList = slots[Current_Inventory_State.Craft];
                default:
                    break;
            }
        }
        return null;
    }
}
