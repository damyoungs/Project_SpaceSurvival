using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Current_Inventory_State
{
    Equip,
    Consume,
    Etc,
    Etc2
}
public class Inventory : Singleton<Inventory>
{
    public GameObject Equip_Inven;
    public GameObject Consume_Inven;
    public GameObject Etc_Inven;
    public GameObject Etc2_Inven;

    Current_Inventory_State state;

    public Current_Inventory_State State
    {
        get => state;
        set
        {
            state = value;
            SetInvenState(state);
        }

    }
  
    public void SwitchTab_To_Equip()
    {
        State = Current_Inventory_State.Equip;
    }
    public void SwitchTab_To_Consume()
    {
        State = Current_Inventory_State.Consume;
    }
    public void SwitchTab_To_Etc()
    {
        State = Current_Inventory_State.Etc;
    }
    public void SwitchTab_To_Etc2()
    {
        State = Current_Inventory_State.Etc2;
    }
    void SetInvenState(Current_Inventory_State state)
    {
        switch (state)
        {
            case Current_Inventory_State.Equip:
                Equip_Inven.SetActive(true);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Etc2_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Consume:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(true);
                Etc_Inven.SetActive(false);
                Etc2_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Etc:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(true);
                Etc2_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Etc2:
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Etc2_Inven.SetActive(true);
                break;
            default:
                break;
        }
    }

}
