using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public enum EquipSlot_Type
{
    Hat,
    Weapon,
    Body,
    Jewel
}
public class EquipBox : MonoBehaviour
{
    EquipBox_Slot[] equipBox_Slots;
    EquipBox_Description description;
    public EquipBox_Description Description => description;
    public EquipBox_Slot this[EquipSlot_Type type] => equipBox_Slots[(int) type];

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;
    private void Awake()
    {
        description = GetComponentInChildren<EquipBox_Description>();
        canvasGroup = GetComponent<CanvasGroup>();
        equipBox_Slots = new EquipBox_Slot[4];
        for (int i = 1; i < transform.childCount - 1; i++)
        {
            equipBox_Slots[i - 1] = transform.GetChild(i).GetComponent<EquipBox_Slot>();
            equipBox_Slots[i - 1].onPointerEnter += description.Open;
            equipBox_Slots[i - 1].onPointerMove += description.MovePosition;
            equipBox_Slots[i - 1].onPointerExit += description.Close;
        }
    }

    void Start()
    {
        GameManager.playerDummy.onEquipItem += Set_ItemData_For_DoubleClick;
        GameManager.SlotManager.on_UnEquip_Item += UnEquip_Item;
    }
    public void Set_ItemData_For_Drag(ItemData itemData)
    {
        //itemdata 가 hat, Weapon, Suit, Jewel 인지 확인하고 슬롯의 타입과 맞지 않으면 리턴시키기
        ItemData_Armor armor = itemData as ItemData_Armor;
        ItemData_Hat hat = itemData as ItemData_Hat;
        ItemData_Craft jewel = itemData as ItemData_Craft;
        EquipBox_Slot slot = FindSlot_By_Position();
        if (slot != null)
        {
            if (armor != null)
            {
                if (slot.slotType == EquipSlot_Type.Body)
                {
                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;// 장착에 성공할 것이므로 인벤토리의 슬롯 비우기
                    slot.SetItemData(armor);
                }
            }
            else if (hat != null)
            {
                if (slot.slotType == EquipSlot_Type.Hat)
                {
                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(hat);
                }
            }
            else if (jewel != null)
            {
                if (slot.slotType == EquipSlot_Type.Jewel)
                {
                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(jewel);
                }
            }
            else if (itemData.ItemType == ItemType.Equip)
            {
                if (slot.slotType == EquipSlot_Type.Weapon)
                {
                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(itemData);
                }
            }
        }
    }
    //equipSlot Clear하는 델리게이트 연결할 차례 
    void UnEquip_Item(ItemData itemData)
    {
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        slot.ItemData = null;
    }
    public void Set_ItemData_For_DoubleClick(ItemData itemData)
    {
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        if (slot != null)
        {
            GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
            slot.SetItemData(itemData);
        }

    }
    EquipBox_Slot Find_Slot_By_Type(ItemData itemData)
    {
        EquipBox_Slot equipSlot = null;
        ItemData_Armor armor = itemData as ItemData_Armor;
        ItemData_Hat hat = itemData as ItemData_Hat;
        ItemData_Craft jewel = itemData as ItemData_Craft;
        if (itemData.ItemType == ItemType.Equip)
        {
            equipSlot = this[EquipSlot_Type.Weapon];
        }
        else if (armor != null)
        {
            equipSlot = this[EquipSlot_Type.Body];
        }
        else if (hat != null)
        {
            equipSlot = this[EquipSlot_Type.Hat];
        }
        else if (jewel != null)
        {
            equipSlot = this[EquipSlot_Type.Jewel];
        }
        return equipSlot;
    }
    EquipBox_Slot FindSlot_By_Position()
    {
        EquipBox_Slot equipSlot = null;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 distance;
        RectTransform slotRect;
        foreach(var slot in equipBox_Slots)
        {
            slotRect = slot.GetComponent<RectTransform>();
            distance = mousePos - (Vector2)slotRect.position;
            if (slotRect.rect.Contains(distance))
            {
                equipSlot = slot;
                break;
            }
        }
        Debug.Log(equipSlot);
        return equipSlot;
    }
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
