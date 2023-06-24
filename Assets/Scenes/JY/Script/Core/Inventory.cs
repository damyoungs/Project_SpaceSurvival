using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Current_Inventory_State
{
    Equip,
    Consume,
    Etc,
    Craft
}
public class Inventory : MonoBehaviour
{
    public GameObject Equip_Inven;
    public GameObject Consume_Inven;
    public GameObject Etc_Inven;
    public GameObject Craft_Inven;

    bool is_Inventory_Open = false;

    public static Current_Inventory_State state;

    private void Start()
    {
        GameManager.UI_Spawner.InitializeSlot();
    }

    public void SwitchTab_To_Equip()
    {
        state = Current_Inventory_State.Equip;
        Equip_Inven.SetActive(true);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(false);
        Craft_Inven.SetActive(false);
    }
    public void SwitchTab_To_Consume()
    {
        state = Current_Inventory_State.Consume;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(true);
        Etc_Inven.SetActive(false);
        Craft_Inven.SetActive(false);
    }
    public void SwitchTab_To_Etc()
    {
        state = Current_Inventory_State.Etc;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(true);
        Craft_Inven.SetActive(false);
    }
    public void SwitchTab_To_Craft()
    {
        state = Current_Inventory_State.Craft;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(false);
        Craft_Inven.SetActive(true);
    }
    void SetInvenState(Current_Inventory_State state)
    {
        switch (state)
        {
            case Current_Inventory_State.Equip:
                Equip_Inven.SetActive(true);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Consume:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(true);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Etc:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(true);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Craft:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void Open_Inventory()
    {
        if (!is_Inventory_Open)
        {
            this.gameObject.SetActive(true);
            is_Inventory_Open = true;
        }
        else
        {
            this.gameObject.SetActive(false);
            is_Inventory_Open = false;
        }
    
    }

}
