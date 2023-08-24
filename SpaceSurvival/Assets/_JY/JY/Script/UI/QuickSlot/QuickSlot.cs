using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public Action<QuickSlot> disconnect_To_Binding_Slot;
    public Action<ItemData_Potion, QuickSlot> connect_To_Binding_Slot;


    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                Refresh_Count(itemCount);
            }
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
                if (itemData == null)
                {
                    disconnect_To_Binding_Slot?.Invoke(this);
                }
                else
                {
                    connect_To_Binding_Slot?.Invoke(itemData, this); // 아직 바인딩이 안된 상태에서 ItemData가 셋팅됐을 때 
                }
            }
        }
    }
    public bool IsEmpty => itemData == null;

    ISkill skill = null;

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
        onBeginDrag?.Invoke(itemData, itemCount);
        Clear();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
    }
}
