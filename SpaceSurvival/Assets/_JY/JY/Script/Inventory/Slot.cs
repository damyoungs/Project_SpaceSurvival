using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//UI 와 분리되어있었다면 delegate로 신호를 보내야하지만  지금은 그냥 Set함수에서 바로 함수를 호출하도록한다
public class Slot : SlotUI_Base, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    RectTransform itemDescriptionTransform;
    TextMeshProUGUI itemDescription_Text;
    Animator anim;
    int popUpHash = Animator.StringToHash("PopUp");
    public Action onItemChange;
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
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
    public bool IsEmpty => itemData == null;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티
    public bool IsMoving { get; set; } = false; //이동중 description 팝업을 방지하기 위한 변수 
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
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {ItemCount}개 설정");
        }
        else
        {
            ClearSlotItem();    // data가 null이면 해당 슬롯은 초기화
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
            // 넘쳤다.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 최대치까지 증가. 현재 {ItemCount}개. {over}개 넘침");
        }
        else
        {
            // 안넘쳤다.
            ItemCount = newCount;
            overCount = 0;
            result = true;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {increaseCount}개 증가. 현재 {ItemCount}개.");
        }

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount < 1)
        {
            // 슬롯이 완전히 비는 경우
            ClearSlotItem();
        }
        else
        {
            // 슬롯에 아이템이 남아있는 경우
            ItemCount = (uint)newCount;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {decreaseCount}개 감소. 현재 {ItemCount}개.");
        }
    }
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        //Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
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
