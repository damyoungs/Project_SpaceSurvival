using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum QuickSlotList
{
    Shift = 0,
    _8,
    _9,
    _0,
    Ctrl,
    Alt,
    Space,
    Insert
}
public enum QuickSlot_State
{
    None,
    Set,
    Moving
}
public class QuickSlot_Manager : MonoBehaviour, IPopupSortWindow
{
    Player_ player;
    SlotManager slotManager;

    Button popupButton;
    TextMeshProUGUI buttonText;
    //InputKeyMouse inputAction;
    string open = "▲";
    string close = "▼";


    RectTransform rectTransform;
    public RectTransform QuickSlotBox_RectTransform => rectTransform;
    QuickSlot[] quickSlots;
    public QuickSlot[] QuickSlots => quickSlots;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 7.0f;
    bool isOpen = false;

    ItemData_Potion shiftSlot_Data;
    ItemData_Potion _8Slot_Data;
    ItemData_Potion _9Slot_Data;
    ItemData_Potion _0Slot_Data;
    ItemData_Potion ctrl_Slot_Data;
    ItemData_Potion alt_Slot_Data;
    ItemData_Potion space_Slot_Data;
    ItemData_Potion insert_Slot_Data;

    ItemData_Potion itemData;
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            itemData = value;
        }
    }
    uint itemCount;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    public QuickSlot this[QuickSlotList number] => quickSlots[(int)number];
    private void Awake()
    {
        //inputAction = new InputKeyMouse();
        hidePos = new Vector2(0, -280.0f);
        rectTransform = GetComponent<RectTransform>();

        popupButton = transform.GetChild(8).GetComponent<Button>();
        buttonText = transform.GetChild(8).GetComponentInChildren<TextMeshProUGUI>();

        popupButton.onClick.AddListener(PopUp);


        quickSlots = new QuickSlot[8];
        int i = 0;
        while (i < 8)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].onSetData += (_, quickSlot) => Connect_Delegate(quickSlot);
            quickSlots[i].on_Clear_Quickslot_Data += DisConnect_Delegate;
            i++;
        }

    }
    //private void OnEnable()
    //{
        //inputAction.QuickSlot.Enable();
        //inputAction.QuickSlot.PopUp.performed += QuickSlot_PopUp;
        //inputAction.QuickSlot.Shift.performed += Shift_performed;
        //inputAction.QuickSlot.Eight.performed += Eight_performed;
        //inputAction.QuickSlot.Nine.performed += Nine_performed;
        //inputAction.QuickSlot.Zero.performed += Zero_performed;
        //inputAction.QuickSlot.Ctrl.performed += Ctrl_performed;
        //inputAction.QuickSlot.Alt.performed += Alt_performed;
        //inputAction.QuickSlot.Space.performed += Space_performed;
        //inputAction.QuickSlot.Insert.performed += Insert_performed;
    //}
    private void Start()
    {
        InputSystemController.Instance.OnQuickSlot_Popup += QuickSlot_PopUp;
        Init();
        buttonText.text = open;
        // GameManager.SlotManager.onDetectQuickSlot += Set_ItemDataTo_QuickSlot;
        StartCoroutine(Get_References());
    }
    IEnumerator Get_References()
    {
        yield return null;
        player = GameManager.Player__;
        slotManager = GameManager.SlotManager;
    }

    public void Set_ItemDataTo_QuickSlot(ItemData_Potion data)
    {
        Find_Slot_By_Position(out QuickSlot targetSlot);
        if (targetSlot != null)
        {
            targetSlot.ItemData = data;
        }
    }
    void Connect_Delegate(QuickSlot slot)
    {
        switch (slot.type)
        {
            case QuickSlot_Type.Shift:
                //inputAction.QuickSlot.Shift.performed += Shift_performed;
                InputSystemController.Instance.OnQuickSlot_Shift += Shift_performed;
                shiftSlot_Data = slot.ItemData;
                break;
            case QuickSlot_Type._8:
                //inputAction.QuickSlot.Eight.performed += Eight_performed;
                InputSystemController.Instance.OnQuickSlot_Eight += Eight_performed;
                _8Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type._9:
                //inputAction.QuickSlot.Nine.performed += Nine_performed;
                InputSystemController.Instance.OnQuickSlot_Nine += Nine_performed;
                _9Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type._0:
                //inputAction.QuickSlot.Zero.performed += Zero_performed;
                InputSystemController.Instance.OnQuickSlot_Zero += Zero_performed;
                _0Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Ctrl:
                //inputAction.QuickSlot.Ctrl.performed += Ctrl_performed;
                InputSystemController.Instance.OnQuickSlot_Ctrl += Ctrl_performed;
                ctrl_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Alt:
                //inputAction.QuickSlot.Alt.performed += Alt_performed;
                InputSystemController.Instance.OnQuickSlot_Alt += Alt_performed;
                alt_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Space:
                //inputAction.QuickSlot.Space.performed += Space_performed;
                InputSystemController.Instance.OnQuickSlot_Space += Space_performed;
                space_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Insert:
                //inputAction.QuickSlot.Insert.performed += Insert_performed;
                InputSystemController.Instance.OnQuickSlot_Insert += Insert_performed;
                insert_Slot_Data = slot.ItemData;
                break;
            default:
                break;
        }
    }
    void DisConnect_Delegate(QuickSlot slot)
    {
        switch (slot.type)
        {
            case QuickSlot_Type.Shift:
                //inputAction.QuickSlot.Shift.performed -= Shift_performed;
                InputSystemController.Instance.OnQuickSlot_Shift -= Shift_performed;
                break;
            case QuickSlot_Type._8:
                //inputAction.QuickSlot.Eight.performed -= Eight_performed;
                InputSystemController.Instance.OnQuickSlot_Eight -= Eight_performed;
                break;
            case QuickSlot_Type._9:
                //inputAction.QuickSlot.Nine.performed -= Nine_performed;
                InputSystemController.Instance.OnQuickSlot_Nine -= Nine_performed;
                break;
            case QuickSlot_Type._0:
                //inputAction.QuickSlot.Zero.performed -= Zero_performed;
                InputSystemController.Instance.OnQuickSlot_Zero -= Zero_performed;
                break;
            case QuickSlot_Type.Ctrl:
                //inputAction.QuickSlot.Ctrl.performed -= Ctrl_performed;
                InputSystemController.Instance.OnQuickSlot_Ctrl -= Ctrl_performed;
                break;
            case QuickSlot_Type.Alt:
                //inputAction.QuickSlot.Alt.performed -= Alt_performed;
                InputSystemController.Instance.OnQuickSlot_Alt -= Alt_performed;
                break;
            case QuickSlot_Type.Space:
                //inputAction.QuickSlot.Space.performed -= Space_performed;
                InputSystemController.Instance.OnQuickSlot_Space -= Space_performed;
                break;
            case QuickSlot_Type.Insert:
                //inputAction.QuickSlot.Insert.performed -= Insert_performed;
                InputSystemController.Instance.OnQuickSlot_Insert -= Insert_performed;
                break;
            default:
                break;
        }
    }
    public bool Find_Slot_By_Position(out QuickSlot findSlot)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        findSlot = null;
        bool result = false;
        foreach (var targetSlot in quickSlots)
        {
            RectTransform rectTransform = targetSlot.GetComponent<RectTransform>();
            Vector2 distance = mousePos - (Vector2)rectTransform.position;
            if (rectTransform.rect.Contains(distance))
            {
                findSlot = targetSlot;
                result = true;
                break;
            }
        }
        return result;
    }

    IEnumerator PopUpCoroutine()
    {
        if (isOpen)
        {
            while (transform.position.y > hidePos.y)
            {
                transform.position += popUpSpeed * -Vector3.up;
                yield return null;
            }
            isOpen = false;
            buttonText.text = open;
        }
        else
        {
            while (transform.position.y < 0.0f)
            {
                transform.position += popUpSpeed * Vector3.up;
                yield return null;
            }
            isOpen = true;
            buttonText.text = close;
        }

    }
    void PopUp()
    {
        StartCoroutine(PopUpCoroutine());
    }
    //private void QuickSlot_PopUp(InputAction.CallbackContext _)
    private void QuickSlot_PopUp()
    {
        StartCoroutine(PopUpCoroutine());
    }
    //private void Space_performed(InputAction.CallbackContext context)
    private void Space_performed()
    {
        if (this[QuickSlotList.Space].ItemCount > 0)
        {
            space_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(space_Slot_Data);
        }
    }

    //private void Alt_performed(InputAction.CallbackContext context)
    private void Alt_performed()
    {
        if (this[QuickSlotList.Alt].ItemCount > 0)
        {
            alt_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(alt_Slot_Data);
        }
    }

    //private void Ctrl_performed(InputAction.CallbackContext context)
    private void Ctrl_performed()
    {
        if (this[QuickSlotList.Ctrl].ItemCount > 0)
        {
            ctrl_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(ctrl_Slot_Data);
        }
    }

    //private void Zero_performed(InputAction.CallbackContext context)
    private void Zero_performed()
    {
        if (this[QuickSlotList._0].ItemCount > 0)
        {
            _0Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_0Slot_Data);
        }
    }

    //private void Nine_performed(InputAction.CallbackContext context)
    private void Nine_performed()
    {
        if (this[QuickSlotList._9].ItemCount > 0)
        {
            _9Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_9Slot_Data);
        }
    }

    //private void Eight_performed(InputAction.CallbackContext context)
    private void Eight_performed()
    {
        if (this[QuickSlotList._8].ItemCount > 0)
        {
            _8Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_8Slot_Data);
        }
    }

    //private void Insert_performed(InputAction.CallbackContext context)
    private void Insert_performed()
    {
        if (this[QuickSlotList.Shift].ItemCount > 0)
        {
            shiftSlot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(shiftSlot_Data);
        }
    }
    private void Insert_performed(InputAction.CallbackContext context)
    {
        if (this[QuickSlotList.Insert].ItemCount > 0)
        {
            insert_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(insert_Slot_Data);
        }
    }

    //private void Shift_performed(InputAction.CallbackContext context)
    private void Shift_performed()
    {
    }


    void Init()
    {
        ItemDescription itemDescription = GameManager.SlotManager.ItemDescription;
        TempSlot_For_QuickSlot_Base tempSlot_Base = FindObjectOfType<TempSlot_For_QuickSlot_Base>();

        // quickSlots = new QuickSlot[8];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].Index = i;
            quickSlots[i].QuickSlot_Key_Value = Enum.GetName(typeof(QuickSlotList), i);//i번째 인덱스를 문자열로 바꿔서 변수에 할당

            quickSlots[i].onPointerEnter += itemDescription.Open;
            quickSlots[i].onPointerMove += itemDescription.MovePosition;
            quickSlots[i].onPointerExit += itemDescription.Close;

            quickSlots[i].onBeginDrag += (_, _) => itemDescription.Toggle_IsPause();
            quickSlots[i].onEndDrag += () => itemDescription.Toggle_IsPause();
            quickSlots[i].onBeginDrag += tempSlot_Base.StartDrag;
            quickSlots[i].onEndDrag += tempSlot_Base.EndDrag;
            tempSlot_Base.onEndDrag += (itemData, count) =>
            {
                Find_Slot_By_Position(out QuickSlot slot);//현재 마우스위치의 퀵슬롯 가져오기
                if (slot != null)
                {
                    slot.ItemData = itemData;
                    slot.ItemCount = count;
                }
            };


        }
    }

    public void OpenWindow()
    {
        if (gameObject.activeSelf)
        {
            QuickSlot_PopUp();
            PopupSorting(this);
        }
    }

    public void CloseWindow()
    {
        if (gameObject.activeSelf)
        {
            QuickSlot_PopUp();
        }
    }
}
