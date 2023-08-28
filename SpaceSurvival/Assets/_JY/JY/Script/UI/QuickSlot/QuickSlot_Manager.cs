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
public class QuickSlot_Manager : MonoBehaviour
{
    Button popupButton;
    TextMeshProUGUI buttonText;
    InputKeyMouse inputAction;
    string open = "��";
    string close = "��";


    RectTransform rectTransform;
    public RectTransform QuickSlotBox_RectTransform => rectTransform;
    public QuickSlot[] quickSlots;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 7.0f;
    bool isOpen = false;

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
       // GameManager.SlotManager.onDetectQuickSlot += Set_ItemDataTo_QuickSlot;
    }
    private void Insert_performed(InputAction.CallbackContext context)
    {
    }
    public void Set_ItemDataTo_QuickSlot(ItemData_Potion data)
    {
        Find_Slot(out QuickSlot targetSlot);
        if (targetSlot != null)
        {
            targetSlot.ItemData = data;
        }
    }
    public bool Find_Slot(out QuickSlot findSlot)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        findSlot = null;
        bool result = false;
        foreach(var targetSlot in quickSlots)
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
        ItemDescription itemDescription = GameManager.SlotManager.ItemDescription;
        TempSlot_For_QuickSlot_Base tempSlot_Base = FindObjectOfType<TempSlot_For_QuickSlot_Base>();

       // quickSlots = new QuickSlot[8];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].Index = i;
            quickSlots[i].QuickSlot_Key_Value = Enum.GetName(typeof(QuickSlotList), i);//i��° �ε����� ���ڿ��� �ٲ㼭 ������ �Ҵ�

            quickSlots[i].onPointerEnter += itemDescription.Open;
            quickSlots[i].onPointerMove += itemDescription.MovePosition;
            quickSlots[i].onPointerExit += itemDescription.Close;

            quickSlots[i].onBeginDrag += (_,_) => itemDescription.Toggle_IsPause();
            quickSlots[i].onEndDrag += () => itemDescription.Toggle_IsPause();
            quickSlots[i].onBeginDrag += tempSlot_Base.StartDrag;
            quickSlots[i].onEndDrag += tempSlot_Base.EndDrag;
            tempSlot_Base.onEndDrag += (itemData, count) =>
            {
                Find_Slot(out QuickSlot slot);//���� ���콺��ġ�� ������ ��������
                if (slot != null)
                {
                    slot.ItemData = itemData;
                    slot.ItemCount = count;
                }
            };


        }
    }
}