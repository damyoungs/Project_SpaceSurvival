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
    TempSlot tempSlot;
    ItemDescription itemDescription;
    public ItemDescription ItemDescription => itemDescription;
    RectTransform inventoryRectTransform;

    public TempSlot TempSlot => tempSlot;

    Inventory inven; 
    Transform equip_Below;
    Transform consume_Below;
    Transform etc_Below;
    Transform craft_Below;

    QuickSlots quickSlotBox;

   
    RectTransform beforeSlotRectTransform;
    RectTransform enhancerUIRectTransform;

    RectTransform mixer_Left_slot_Transform;
    RectTransform mixer_Middle_Slot_Transform;
    RectTransform mixerUI_Transform;
    Item_Mixer_UI mixer_UI;

    ItemSplitter spliter;
    bool isShiftPress = false;

    public Action<ItemData_Potion, uint> onDetectQuickSlot;

    public Dictionary<Current_Inventory_State, List<Slot>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //슬롯 생성후 번호를 부여하기위한 Dic
    private Dictionary<QuickSlot, List<Slot>> linkedSlots;
 
    public byte Index_JustChange_Slot { get; set; }
  

    private void Awake()
    {
        input = new InputKeyMouse();
        tempSlot = FindObjectOfType<TempSlot>(true);
        itemDescription = FindObjectOfType<ItemDescription>();
    }
    private void Start()
    {
        spliter = FindObjectOfType<ItemSplitter>(true);
      //  Initialize();
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
    public void Initialize()//Inventory에서 Start타이밍에 호출
    {
        beforeSlotRectTransform = GameManager.Enhancer.EnhancerUI.BeforeSlot.GetComponent<RectTransform>();
        enhancerUIRectTransform = GameManager.Enhancer.EnhancerUI.AfterSlot.GetComponent<RectTransform>();
   
        mixer_Left_slot_Transform = GameManager.Mixer.MixerUI.Left_Slot.GetComponent<RectTransform>();
        mixer_Middle_Slot_Transform = GameManager.Mixer.MixerUI.Middle_Slot.GetComponent<RectTransform>();
        mixerUI_Transform = GameManager.Mixer.GetComponent<RectTransform>();
        mixer_UI = GameManager.Mixer.MixerUI;
        quickSlotBox = GameManager.QuickSlot_Box;

        foreach (QuickSlot quickSlot in quickSlotBox.quickSlots)
        {
            quickSlot.connect_To_Binding_Slot += Connect_To_Slot;
            quickSlot.disconnect_To_Binding_Slot += Disconnect_to_Slot;
        }

        inven = GameManager.Inventory;
        equip_Below = inven.transform.GetChild(0).GetChild(0).GetChild(0);
        consume_Below = inven.transform.GetChild(1).GetChild(0).GetChild(0);
        etc_Below = inven.transform.GetChild(2).GetChild(0).GetChild(0);
        craft_Below = inven.transform.GetChild(3).GetChild(0).GetChild(0);
        inventoryRectTransform = inven.GetComponent<RectTransform>();

        itemDescription.Close();
        TempSlot.InitializeSlot(TempSlot);
        TempSlot.onTempSlotOpenClose += OnDetailPause; // TempSlot이 Open할때 true로 호출하고 Close할때 false로 호출
        spliter.onCancel += () => itemDescription.IsPause = false;   // 캔슬버턴 누르면 상세정보창 일시정지 해제
        spliter.Close();
        spliter.onOkClick += OnSpliterOk;

        


        mixer_UI.onEndSession_Success += Add_Reward_Item;

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

        linkedSlots = new Dictionary<QuickSlot, List<Slot>>
        {
            {quickSlotBox[QuickSlotList.Shift], new List<Slot>() },
            {quickSlotBox[QuickSlotList._8], new List<Slot>() },
            {quickSlotBox[QuickSlotList._9], new List<Slot>() },
            {quickSlotBox[QuickSlotList._0], new List<Slot>() },
            {quickSlotBox[QuickSlotList.Ctrl], new List<Slot>() },
            {quickSlotBox[QuickSlotList.Alt], new List<Slot>() },
            {quickSlotBox[QuickSlotList.Space], new List<Slot>() },
            {quickSlotBox[QuickSlotList.Insert], new List<Slot>() },

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
        MoveItem(data ,index, tempSlot.Index);    // 시작 슬롯에서 임시 슬롯으로 아이템 옮기기
        TempSlot.Open();                          // 임시 슬롯 열기
    }
    private void OnItemMoveEnd(ItemData data, uint index, bool isSuccess)
    {
        MoveItem(data, tempSlot.Index, index);    // 임시 슬롯에서 도착 슬롯으로 아이템 옮기기
        if (tempSlot.IsEmpty)          // 비었다면(같은 종류의 아이템일 때 일부만 들어가는 경우가 있을 수 있으므로)
        {
            TempSlot.Close();                     // 임시 슬롯 닫기
        }

        if (isSuccess)
        {
            List<Slot> slots = GetItemTab();
            itemDescription.Open(slots[(int)index].ItemData);     // 드래그가 성공적으로 끝났으면 상세 정보창도 열기
        }
    }
    private void OnSlotClick(ItemData data, uint index)
    {
        ItemData_Potion potion = data as ItemData_Potion;
        if (isShiftPress)
        {
            if (tempSlot.IsEmpty)
            {
                OnSpliterOpen(index);
            }
        }
        else if (potion != null)
        {
            potion.Consume(GameManager.playerDummy);
            RemoveItem(data, index);
        }
        else
        {
            // 임시 슬롯에 아이템이 있을 때 클릭이 되었으면
            OnItemMoveEnd(data, index, true); // 클릭된 슬롯으로 아이템 이동
        }
    }
    private void OnItemDetailOn( ItemData data, uint index)
    {
        List<Slot> slots = GetItemTab(data); //빈슬롯 위에 Pointer Enter시 data가 null 이되서 리스트를 가져올때 터짐

        itemDescription.Open(slots[(int)index].ItemData); // 상세정보창 열기
    }
    private void OnItemDetailOff(uint index)
    {
        itemDescription.Close(); // 상세정보창 닫기
    }
    private void OnSlotPointerMove(Vector2 screenPos)//마우스 움직일때마다 호출
    {
        itemDescription.MovePosition(screenPos);
    }
    private void OnDetailPause(bool isPause)
    {
        itemDescription.IsPause = isPause;
    }
    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="index">아이템을 분리할 슬롯의 인덱스</param>
    private void OnSpliterOpen(uint index)
    {
        List<Slot> slots = GetItemTab();
        Slot target = slots[(int)index];
        spliter.transform.position = target.transform.position + Vector3.up * 100;
        spliter.Open(target);
        itemDescription.IsPause = true;
    }

    /// <summary>
    /// 아이템 분리창에서 OK 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    /// <param name="index">아이템이 분리될 슬롯</param>
    /// <param name="count">분리된 개수</param>
    private void OnSpliterOk(uint index, uint count)
    {
        SplitItem(index, count);
        TempSlot.Open();
    }

    /// <summary>
    /// 쉬프트키가 눌려지거나 때졌을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnShiftPress(InputAction.CallbackContext context)
    {
        isShiftPress = !context.canceled;   // 쉬프트키 상황 기록
    }


    /// <summary>
    /// 마우스 클릭이 떨어졌을 때 실행되는 함수(아이템 드랍용)
    /// </summary>
    private void OnItemDrop(InputAction.CallbackContext _)
    {
        if (tempSlot == null)
            return;
        if (!tempSlot.IsEmpty)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 distance_Between_Mouse_Inven = screenPos - (Vector2)inventoryRectTransform.position;//inventoryRectTransform.position의 피봇을 기준으로 떨어진거리 계산
            Vector2 distance_Between_Mouse_BeforeSlot = screenPos - (Vector2)beforeSlotRectTransform.position;
            Vector2 distance_Between_Mouse_enhancerUI = screenPos - (Vector2)enhancerUIRectTransform.position;
            Vector2 distance_Between_Mouse_Left_Slot = screenPos - (Vector2)mixer_Left_slot_Transform.position;
            Vector2 distance_Between_Mouse_Middle_Slot = screenPos - (Vector2)mixer_Middle_Slot_Transform.position;
            Vector2 distance_Between_Mouse_MixerUI = screenPos - (Vector2)mixerUI_Transform.position;
            Vector2 distance_Between_Mouse_QuickSlot_Box = screenPos - (Vector2)GameManager.QuickSlot_Box.QuickSlotBox_RectTransform.position;

            if (beforeSlotRectTransform.rect.Contains(distance_Between_Mouse_BeforeSlot) && GameManager.Enhancer.EnhancerUI.IsOpen)//강화 슬롯의 위치이면서 강화ㅑ 가능한 아이템 일 때
            {
                ItemData_Enhancable enhancable = TempSlot.ItemData as ItemData_Enhancable;
                if (enhancable != null)
                {
                    GameManager.Enhancer.ItemData = enhancable;
                }
            }
            else if (GameManager.QuickSlot_Box.QuickSlotBox_RectTransform.rect.Contains(distance_Between_Mouse_QuickSlot_Box))
            {
                //스킬이나 포션만 등록
                ItemData_Potion potion = TempSlot.ItemData as ItemData_Potion;
                if (potion != null)
                {
                    slots[Current_Inventory_State.Consume][Index_JustChange_Slot].ItemData = potion;//이것을 생략하면 드래그한 슬롯의 ItemData가 null 이 되서 리스트에 추가되지 않는 문제가 있다.
                    uint newCount = GetTotalAmount(tempSlot.ItemData, out List<Slot> sameItemSlots) + tempSlot.ItemCount;//out으로 같은 아이템을가진 슬롯의 List 받기

                    if (GameManager.QuickSlot_Box.Set_ItemDataTo_QuickSlot(potion, newCount, out QuickSlot targetSlot))
                    {
                        int i = 0;
                        while (i < sameItemSlots.Count)
                        {
                            linkedSlots[targetSlot].Add(sameItemSlots[i]);
                            i++;
                        }
                        foreach (Slot slot in sameItemSlots)
                        {
                            slot.onItemCountChange = null;
                            slot.onItemCountChange += Throw_NewCount_To_QuickSlot;
                            slot.BindingSlot = targetSlot;
                        }
                    }
                }
            }
            else if (mixer_Left_slot_Transform.rect.Contains(distance_Between_Mouse_Left_Slot) && mixer_UI.IsOpen)//조합창의 왼쪽슬롯
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
            else if (!inventoryRectTransform.rect.Contains(distance_Between_Mouse_Inven))// 거리의 크기가 rect 의 크기보다 작으면 인벤토리 안쪽
            {
                if (enhancerUIRectTransform.rect.Contains(distance_Between_Mouse_enhancerUI) && GameManager.Enhancer.EnhancerState == EnhancerState.Open)//inhancerUI열려있으면 return
                {
                    return;
                }
                // 인벤토리 영역 밖이면
                TempSlot.OnDrop(screenPos);
            }
        }
    }
    void Connect_To_Slot(ItemData_Potion itemData, QuickSlot quickSlot)
    {
        List<Slot> slotArr = Get_Binding_Slots(itemData);//연결할 때는 quickSlot이 아니라 ItemData로 찾아야함
        if (slotArr == null) return;

       
        foreach(Slot slot in slotArr)
        {
            slot.BindingSlot = quickSlot;// 타겟을 바꿔줘야 한다.
            slot.onItemCountChange = null;// 처음 연결할 때 이미 추가되어있는 상황에 대비해서 중복 추가 방지용
            slot.onItemCountChange += Throw_NewCount_To_QuickSlot;
            linkedSlots[quickSlot].Add(slot); // 딕셔너리 추가
        }
    }
    void Disconnect_to_Slot(QuickSlot quickSlot)
    {
        List<Slot> slotArr = Get_Binding_Slots(quickSlot);
        foreach(Slot slot in slotArr)
        {
            slot.onItemCountChange = null;
        }
        linkedSlots[quickSlot].Clear(); // 클리어 해주지 않으면 여전히 남아있음
    }
    List<Slot> Get_Binding_Slots(QuickSlot quickSlot)
    {
        List<Slot> slotArr = null;
        slotArr = linkedSlots[quickSlot];
        return slotArr;
    }
    List<Slot> Get_Binding_Slots(ItemData_Potion itemData)
    {
        List<Slot> slotArr = new List<Slot>();
        //이 시점에선 linkedSlots이 이미 Clear된 상태라서 Inventory의 아이템을 비교해서 찾아야한다.
        foreach (Slot slot in slots[Current_Inventory_State.Consume])
        {
            if (slot.ItemData == itemData)
            {
                slotArr.Add(slot);
            }
            
        }
        return slotArr;
    }
    QuickSlot Get_Binding_QuickSlot(Slot slot)
    {
        return null;
    }
    void Throw_NewCount_To_QuickSlot(QuickSlot targetSlot)
    {
        List<Slot> slots = linkedSlots[targetSlot];
        uint newCount = Get_Total_ItemCount(slots);
        targetSlot.ItemCount = newCount;//참조를 받아왔기 때문에 바로 수정 가능
    }
    uint Get_Total_ItemCount(List<Slot> slots)//같은 아이템 갯수의 합을 리턴하는 함수 퀵슬롯에 할당 후 델리게이트가 연결된 상태에서 수량이 변경될 때마다 실행
    {
        uint newCount = 0;
        foreach(Slot slot in slots)
        {
            newCount += slot.ItemCount;
        }
        return newCount;
    }
    private uint GetTotalAmount(ItemData itemData, out List<Slot> sameItemSlots)//같은 아이템을 가진 슬롯List와 Total카운트를 구해주는 함수//처음 퀵슬롯에 할당할때 호출
    {
        uint newCount = 0;
        List<Slot> consumeTab = slots[Current_Inventory_State.Consume];
        sameItemSlots = new List<Slot>();
        foreach (Slot slot in consumeTab)
        {
            if (slot.ItemData == itemData)
            {
                newCount += slot.ItemCount;
                sameItemSlots.Add(slot);
            }
        }
        return newCount;
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

    void Add_Reward_Item(ItemData item)//조합성공시 아이템 추가
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
    public bool AddItem(ItemData_Enhancable data)
    {
        bool result = false;
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
            //Debug.Log($"아이템 감소 실패 : {slotIndex}는 없는 인덱스입니다.");
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
            //Debug.Log($"아이템 삭제 실패 : {slotIndex}는 없는 인덱스입니다.");
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
        // from지점과 to지점이 다르고 from과 to가 모두 valid해야 한다.
        if ((from != to) && IsValidIndex(from, data) && IsValidIndex(to, data))
        {
            Slot fromSlot = (from == tempSlot.Index) ? tempSlot : slots[(int)from];  // 임시 슬롯을 감안해서 삼항연산자로 처리
            if (!fromSlot.IsEmpty)
            {
                Slot toSlot = (to == tempSlot.Index) ? TempSlot : slots[(int)to];
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
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);// 이때 슬롯의 데이터가 null이 된다.
                    toSlot.AssignSlotItem(tempData, tempCount);
                    //Debug.Log($"{from}번 슬롯과 {to}번 슬롯의 아이템 교체");
                }
            }
        }
    }


    public void SplitItem(uint slotIndex, uint count) // 스플릿할때는 굳이 
    {
        if (IsValidIndex(slotIndex))
        {
            List<Slot> slots = GetItemTab();
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
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        List<Slot> slots = GetItemTab();
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

        // 아이템 종류와 개수를 따로 저장하기
        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(slots.Count);
        foreach (var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        // 슬롯에 아이템 종류와 개수를 순서대로 할당하기
        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);
            index++;
        }//수동 복사부분

        // 정렬 완료된 것을 다시 배열로 만들기
       // slots = beforeSlots;
        RefreshInventory();
    }

    /// <summary>
    /// 모든 슬롯이 변경되었음을 알리는 함수
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
        foreach (var slot in slots)  // 모든 슬롯을 다 돌면서
        {
            if (slot.ItemData != null)
            {
                if (slot.ItemData.code == data.code && slot.ItemCount < slot.ItemData.maxStackCount)  // itemData가 같고 여유 공간이 있으면 그 슬롯을 리턴한다
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

    /// 적절한 인덱스인지 확인하는 함수
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
        if (item != null) // 이 함수를 호출할 때 itemdata가 null 이면  인벤토리에 현재 선택된 탭의 리스트를 리턴한다.
        {
            switch (item.ItemType) // null 이 아니면 Inventory 클래스에서 현재 어떤 탭이 선택되었든 관계없이 item의 itemType에 따라 리스트를 결정 한다.
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
