using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//UI �� �и��Ǿ��־��ٸ� delegate�� ��ȣ�� ������������  ������ �׳� Set�Լ����� �ٷ� �Լ��� ȣ���ϵ����Ѵ�
public class Slot : SlotUI_Base, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    RectTransform itemDescriptionTransform;
    TextMeshProUGUI itemDescription_Text;
    Animator anim;
    int popUpHash = Animator.StringToHash("PopUp");
    public Action onItemChange;
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager��  GetItem �Լ��� ����ɶ� Item�� ������ �޾ƿ������� ������Ƽ
    {
        get => itemData;
        private set
        {
            if (itemData != value)
            {
                itemData = value;
            }
        }
    }
    public bool IsEmpty => itemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ
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
    uint itemCount;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                UpdateAmountText(itemCount);
            }
        }
    }
    private void Start()
    {
        GameManager.SlotManager.isMovingChange += IsMovingChange;
        anim = GameManager.Inventory.GetComponent<Animator>();
        itemDescription_Text = GameManager.Inventory.transform.GetChild(9).GetComponentInChildren<TextMeshProUGUI>();
        itemDescriptionTransform = GameManager.Inventory.transform.GetChild(9).GetComponent<RectTransform>();
    }
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
    private void IsMovingChange() { IsMoving = !IsMoving; }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty && !IsMoving)
        {
            SetDiscription(ItemData);
            anim.SetBool(popUpHash, true);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        anim.SetBool(popUpHash, false);
        GameManager.SlotManager.OnSlotClicked(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool(popUpHash, false);
    }
    void SetDiscription(ItemData item)
    {
        itemDescription_Text.text = item.itemDescription;
    }
    void UpdateAmountText(uint amount)
    {
   
    }

 
}
