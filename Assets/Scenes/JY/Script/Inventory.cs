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
public enum ItemName
{
    Capsule,
    Cube,
    Plane
}
public class Inventory : Singleton<Inventory>
{
    public GameObject Equip_Inven;
    public GameObject Consume_Inven;
    public GameObject Etc_Inven;
    public GameObject Etc2_Inven;

    public static Current_Inventory_State state;



    public void SwitchTab_To_Equip()
    {
        state = Current_Inventory_State.Equip;
        Equip_Inven.SetActive(true);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(false);
        Etc2_Inven.SetActive(false);
    }
    public void SwitchTab_To_Consume()
    {
        state = Current_Inventory_State.Consume;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(true);
        Etc_Inven.SetActive(false);
        Etc2_Inven.SetActive(false);
    }
    public void SwitchTab_To_Etc()
    {
        state = Current_Inventory_State.Etc;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(true);
        Etc2_Inven.SetActive(false);
    }
    public void SwitchTab_To_Etc2()
    {
        state = Current_Inventory_State.Etc2;
        Equip_Inven.SetActive(false);
        Consume_Inven.SetActive(false);
        Etc_Inven.SetActive(false);
        Etc2_Inven.SetActive(true);
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

    public void GetItem(ItemName name)
    {

    }
}
