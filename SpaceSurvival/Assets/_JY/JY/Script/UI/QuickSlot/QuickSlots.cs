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
public class QuickSlots : MonoBehaviour
{
    Button popupButton;
    TextMeshProUGUI buttonText;
    InputKeyMouse inputAction;
    string open = "▲▲";
    string close = "▼▼";


    RectTransform rectTransform;
    public RectTransform QuickSlotBox_RectTransform => rectTransform;
    QuickSlot[] quickSlots = null;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 7.0f;
    bool isOpen = false;

    public Action<ItemData_Potion, uint> onSetData;
    ItemData_Potion itemData;
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
            }
        }
    }

    //QuickSlot_State state = QuickSlot_State.None;
    //public QuickSlot_State State
    //{
    //    get => state;
    //    set
    //    {
    //        if (state != value)
    //        {
    //            state = value;
    //            switch (state)
    //            {
    //                case QuickSlot_State.Set:
    //                    break;
    //                case QuickSlot_State.Moving:
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //}
    public QuickSlot this[QuickSlotList number] => quickSlots[(int) number];
    private void Awake()
    {
        inputAction = new InputKeyMouse();
        hidePos = new Vector2(0, -280.0f);
        rectTransform = GetComponent<RectTransform>();

        popupButton = transform.GetChild(8).GetComponent<Button>();
        buttonText = transform.GetChild(8).GetComponentInChildren<TextMeshProUGUI>();

        popupButton.onClick.AddListener(PopUp);

    }
    private void OnEnable()
    {
        inputAction.QuickSlot.Enable();
        inputAction.QuickSlot.PopUp.performed += QuickSlot_PopUp;
        inputAction.QuickSlot.Shift.performed += Shift_performed;
        inputAction.QuickSlot.Eight.performed += Eight_performed;
        inputAction.QuickSlot.Nine.performed += Nine_performed;
        inputAction.QuickSlot.Zero.performed += Zero_performed;
        inputAction.QuickSlot.Ctrl.performed += Ctrl_performed;
        inputAction.QuickSlot.Alt.performed += Alt_performed;
        inputAction.QuickSlot.Space.performed += Space_performed;
        inputAction.QuickSlot.Insert.performed += Insert_performed;
    }
    private void Start()
    {
        Init();
        buttonText.text = open;
        GameManager.SlotManager.onDetectQuickSlot += Set_ItemDataTo_QuickSlot;
    }
    private void Insert_performed(InputAction.CallbackContext context)
    {
    }
    void Set_ItemDataTo_QuickSlot(ItemData_Potion data, uint itemcount)
    {
       // ItemData = data;
        //State = QuickSlot_State.Set;
        QuickSlot slot = Find_Clicked_Slot();
        if (slot != null)
        {
            slot.ItemData = data;
            slot.ItemCount = itemcount;
        }
    }
    QuickSlot Find_Clicked_Slot()
    {
        QuickSlot slot = null;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        foreach(var targetSlot in quickSlots)
        {
            RectTransform rectTransform = targetSlot.GetComponent<RectTransform>();
            Vector2 distance = mousePos - (Vector2)rectTransform.position;
            if (rectTransform.rect.Contains(distance))
            {
                slot = targetSlot;
                break;
            }
        }
        return slot;
    }
    IEnumerator PopUpCoroutine()
    {
        if (isOpen)
        {
            while (transform.position.y > hidePos.y)
            {
                transform.position +=  popUpSpeed * -Vector3.up;
                yield return null;
            }
            isOpen = false;
            buttonText.text = open;
        }
        else
        {
            while (transform.position.y < 0.0f)
            {
                transform.position +=  popUpSpeed * Vector3.up;
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
    private void QuickSlot_PopUp(InputAction.CallbackContext _)
    {
        StartCoroutine(PopUpCoroutine());
    }
    private void Space_performed(InputAction.CallbackContext context)
    {
    }

    private void Alt_performed(InputAction.CallbackContext context)
    {
    }

    private void Ctrl_performed(InputAction.CallbackContext context)
    {
    }

    private void Zero_performed(InputAction.CallbackContext context)
    {
    }

    private void Nine_performed(InputAction.CallbackContext context)
    {
    }

    private void Eight_performed(InputAction.CallbackContext context)
    {
    }

 

    private void Shift_performed(InputAction.CallbackContext context)
    {
    }

  
    void Init()
    {
        ItemDescription itemDescription = GameManager.SlotManager.itemDescription;
        TempSlot_For_QuickSlot_Base tempSlot_Base = FindObjectOfType<TempSlot_For_QuickSlot_Base>();

        quickSlots = new QuickSlot[8];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].Index = i;
            quickSlots[i].QuickSlot_Key_Value = Enum.GetName(typeof(QuickSlotList), i);//i번째 인덱스를 문자열로 바꿔서 변수에 할당

            quickSlots[i].onPointerEnter += itemDescription.Open;
            quickSlots[i].onPointerMove += itemDescription.MovePosition;
            quickSlots[i].onPointerExit += itemDescription.Close;

            quickSlots[i].onBeginDrag += (_,_) => itemDescription.Toggle_IsPause();
            quickSlots[i].onEndDrag += () => itemDescription.Toggle_IsPause();
            quickSlots[i].onBeginDrag += tempSlot_Base.StartDrag;
            quickSlots[i].onEndDrag += tempSlot_Base.EndDrag;
            tempSlot_Base.onEndDrag += Set_ItemDataTo_QuickSlot;


        }
    }
}
