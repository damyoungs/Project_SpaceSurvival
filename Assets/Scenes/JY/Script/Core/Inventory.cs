using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool is_Initialized;

    public delegate void Inventory_State_Changed(Current_Inventory_State state); //state가 바뀌면 setter가 호출할 delegate
    Inventory_State_Changed inventory_changed;
    private Current_Inventory_State state;
    public Current_Inventory_State State
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                state = value;
                inventory_changed?.Invoke(state);
            }
        }
    }
    private void OnEnable()
    {
        inventory_changed += Update_State;
    }
    private void OnDisable()
    {
        inventory_changed -= Update_State;
    }
    private void Start()
    {
        GameManager.UI_Spawner.InitializeSlot();
    }
    void Update_State(Current_Inventory_State state)
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
    public void SwitchTab_To_Equip() { State = Current_Inventory_State.Equip; }
    public void SwitchTab_To_Consume() { State = Current_Inventory_State.Consume;}
    public void SwitchTab_To_Etc() { State = Current_Inventory_State.Etc;}
    public void SwitchTab_To_Craft() { State = Current_Inventory_State.Craft;}
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
