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
    public EquipBox_Slot this[EquipSlot_Type type] => equipBox_Slots[(int) type];

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        equipBox_Slots = new EquipBox_Slot[4];
        for (int i = 1; i < transform.childCount; i++)
        {
            equipBox_Slots[i - 1] = transform.GetChild(i).GetComponent<EquipBox_Slot>();
          //  equipBox_Slots[i - 1].slotType = 
        }
    }

    void Start()
    {
       // Close();
    }
    public void Set_ItemData(ItemData itemData)
    {
        //itemdata 가 hat, Weapon, Suit, Jewel 인지 확인하고 슬롯의 타입과 맞지 않으면 리턴시키기
        ItemData_Armor armor = itemData as ItemData_Armor;
        ItemData_Hat hat = itemData as ItemData_Hat;
        ItemData_Craft jewel = itemData as ItemData_Craft;
        if (armor != null)
        {

        }
        else if (hat != null)
        {

        }
        else if (jewel != null)
        {

        }
        else if (itemData.ItemType == ItemType.Equip)
        {

        }
        EquipBox_Slot slot = FindSlot();



    }
    EquipBox_Slot FindSlot()
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
