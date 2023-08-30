using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBox_Slot : MonoBehaviour
{
    Image itemIcon;
    ItemData itemData;
    public EquipSlot_Type slotType;


    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                Refresh();
            }
        }
    }
    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Refresh()
    {
        if (itemData == null)
        {
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
        }
        else
        {
            itemIcon.sprite = itemData.itemIcon;
            itemIcon.color = Color.white;
        }
    }
}
