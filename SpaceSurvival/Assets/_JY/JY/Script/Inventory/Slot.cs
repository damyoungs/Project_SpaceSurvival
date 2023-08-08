using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//UI �� �и��Ǿ��־��ٸ� delegate�� ��ȣ�� ������������  ������ �׳� Set�Լ����� �ٷ� �Լ��� ȣ���ϵ����Ѵ�
public class Slot : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    RectTransform itemDescriptionTransform;
    TextMeshProUGUI itemDescription_Text;
    Animator anim;
    int popUpHash = Animator.StringToHash("PopUp");

    public Action<ItemData, uint> onDragBegin;
    public Action<ItemData, uint, bool> onDragEnd;
    public Action<ItemData, uint> onClick;
    public Action<ItemData, uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;
    public bool IsMoving { get; set; } = false; //�̵��� description �˾��� �����ϱ� ���� ���� 
    bool isEquipped = false;
    public bool IsEquipped
    {
        get => isEquipped;
        private set
        {
            if (isEquipped != value)
            {
                isEquipped = value;
            }
        }
    }


    public uint Index { get; set; }
 
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        if (data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = false;
            //Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" �������� {ItemCount}�� ����");
        }
        else
        {
            ClearSlotItem();    // data�� null�̸� �ش� ������ �ʱ�ȭ
        }
    }
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        int over;

        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)
        {
            // ���ƴ�.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            //Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" �������� �ִ�ġ���� ����. ���� {ItemCount}��. {over}�� ��ħ");
        }
        else
        {
            // �ȳ��ƴ�.
            ItemCount = newCount;
            overCount = 0;
            result = true;
            //Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" �������� {increaseCount}�� ����. ���� {ItemCount}��.");
        }

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount < 1)
        {
            // ������ ������ ��� ���
            ClearSlotItem();
        }
        else
        {
            // ���Կ� �������� �����ִ� ���
            ItemCount = (uint)newCount;
            //Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" �������� {decreaseCount}�� ����. ���� {ItemCount}��.");
        }
    }
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        //Debug.Log($"�κ��丮 {slotIndex}�� ������ ���ϴ�.");
    }
    public void UseItem(GameObject target)
    {
    }

    public void EquipItem(GameObject target)
    {
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(ItemData, Index);
    }
    public void OnPointerClick(PointerEventData eventData)// itemdata�� null �� �Ǵ� ����
    {
        onClick?.Invoke(ItemData, Index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(Index);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragBegin?.Invoke(ItemData, Index);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�װ� ���������� Ȯ��
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // ���콺 �ִ� ��ġ�� ���� ������Ʈ�� �ִ��� Ȯ��
        if (obj != null)
        {
            // ���콺 ��ġ�� � ������Ʈ�� �ִ�.
            Slot endSlot = obj.GetComponent<Slot>();  // ���콺 ��ġ�� �ִ� ������Ʈ�� ����UI���� Ȯ��
            if (endSlot != null)
            {
                // ����UI��.
                Debug.Log($"�巡�� ���� : {endSlot.Index}�� ����");
                onDragEnd?.Invoke(GameManager.SlotManager.TempSlot.ItemData, endSlot.Index, true); // ���������� �ִ� ������ �ε����� ���������� �����ٰ� �˶� ������
            }
            else
            {
                // ����UI�� �ƴϴ�.
                Debug.Log($"������ �ƴմϴ�.");           // (�巡�� �������� ���� ���� ��ġ�� �ǵ����� ���� ����)
                onDragEnd?.Invoke(GameManager.SlotManager.TempSlot.ItemData, Index, false);        // ���� �巡�װ� ������ �ε����� ������������ �����ٰ� �˶� ������
            }
        }
        else
        {
            // ���콺 ��ġ�� �ƹ��� ������Ʈ�� ����.
            Debug.Log("�ƹ��� ������Ʈ�� �����ϴ�.");
        }
    }



    public void OnDrag(PointerEventData eventData)
    {
       
    }
}