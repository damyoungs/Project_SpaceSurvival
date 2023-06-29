using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Current_Inventory_State
{
    Equip,
    Consume,
    Etc,
    Craft
}
public enum ItemName 
{
    hpPotion,
    mpPotion,
    sword,
    hammer
}
public class Inventory : MonoBehaviour
{
    public static ItemName ItemName;

    public GameObject Equip_Inven;
    public GameObject Consume_Inven;
    public GameObject Etc_Inven;
    public GameObject Craft_Inven;

    Transform toolBar;
    Button close_Button;
    Button add_Button;
    Button equip_Button;
    Button consume_Button;
    Button etc_Button;
    Button craft_Button;

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
    private void Awake()
    {
        toolBar = transform.GetChild(0);
        close_Button = toolBar.GetChild(0).GetComponent<Button>();
        add_Button = toolBar.GetChild(1).GetComponent<Button>();
        equip_Button = transform.GetChild(1).GetComponent<Button>();
        consume_Button = transform.GetChild(2).GetComponent<Button>();
        etc_Button = transform.GetChild(3).GetComponent<Button>();
        craft_Button = transform.GetChild(4).GetComponent<Button>();

        close_Button.onClick.AddListener(Open_Inventory);
        add_Button.onClick.AddListener(GameManager.SlotManager.Add_Slot);
        equip_Button.onClick.AddListener(SwitchTab_To_Equip);
        consume_Button.onClick.AddListener(SwitchTab_To_Consume);
        etc_Button.onClick.AddListener(SwitchTab_To_Etc);
        craft_Button.onClick.AddListener(SwitchTab_To_Craft);
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
        GameManager.SlotManager.Initialize();


    }
    void Update_State(Current_Inventory_State state)
    {
        ColorBlock equipColors = equip_Button.colors;
        ColorBlock consumeColors = consume_Button.colors;
        ColorBlock etcColors = etc_Button.colors;
        ColorBlock craftColors = craft_Button.colors;

        equipColors.normalColor = Color.white;
        consumeColors.normalColor = Color.white;
        etcColors.normalColor = Color.white;
        craftColors.normalColor = Color.white;

        switch (state)
        {
            case Current_Inventory_State.Equip:
                Equip_Inven.SetActive(true);
                equipColors.normalColor = Color.grey;
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Consume:
                Consume_Inven.SetActive(true);
                consumeColors.normalColor = Color.grey;
                Equip_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Etc:
                Etc_Inven.SetActive(true);
                etcColors.normalColor = Color.grey;
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Craft:
                Craft_Inven.SetActive(true);
                craftColors.normalColor = Color.grey;
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void SwitchTab_To_Equip() { State = Current_Inventory_State.Equip; } //버튼 누르면 호출
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
    void GetItem(ItemName name)
    {
        //GameManager.SlotManager.SetItemData
        
    }
    
}
