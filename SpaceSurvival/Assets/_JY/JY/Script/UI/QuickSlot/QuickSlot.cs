using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickSlot_Type
{
    Shift,
    _8,
    _9,
    _0,
    Ctrl,
    Alt,
    Space,
    Insert
}

public class QuickSlot : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IBeginDragHandler,IEndDragHandler, IDragHandler
{
    Image itemIcon;
    TextMeshProUGUI quickSlotText;
    ItemData_Potion itemData = null;

    //---------------------------------------------- description 팝업 관련
    public Action<ItemData> onPointerEnter;
    public Action onPointerExit;
    public Action<Vector2> onPointerMove;
    //----------------------------------------------
    public Action<ItemData_Potion, uint> onBeginDrag;
    public Action onEndDrag;

    public Action<ItemData_Potion, QuickSlot> onSetData;
    public Action<QuickSlot> on_Clear_Quickslot_Data;

    public QuickSlot_Type type;
    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            Refresh_Count(itemCount);
        }
    }
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                Refresh_Icon(itemData);
                if (itemData != null)
                {
                    onSetData?.Invoke(itemData, this);//SlotManager, QuickSlotManager에서 받음
                }
                else
                {
                    on_Clear_Quickslot_Data?.Invoke(this);
                }
            }
        }
    }
    public bool IsEmpty => itemData == null;


    public string QuickSlot_Key_Value { get; set; }
    int index;
    public int Index
    {
        get => index;
        set
        {
            if (index == 99999)
            {
                index = value;
            }
        }
    }
    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        quickSlotText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        index = 99999;
    }
    void Refresh_Icon(ItemData_Potion itemData)
    {
        if (IsEmpty)
        {
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
        }
        else
        {
            itemIcon.sprite = itemData.itemIcon;
            itemIcon.color = Color.white;
        }
    }
    void Refresh_Count(uint count)
    {
        if (itemData != null)
        {
            quickSlotText.text = Mathf.Max(count, 0).ToString();
        }
        else
        {
            quickSlotText.text = QuickSlot_Key_Value;
        }
    }
    void Clear()
    {
        ItemData = null;
        ItemCount = 0; //ItemData가 null이면 CountText는 자동으로 Default값으로 세팅
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(this.itemData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      // onPointerClick?.Invoke(itemData);
      // Clear();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemData != null)
        {
            onPointerMove?.Invoke(eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke(itemData, itemCount);//tempSlot으로 아이템 이전
        Clear();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
    }
}
