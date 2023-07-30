using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Current_Inventory_State
{
    None,
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
    public GameObject ItemEnhancer;

    GameObject ItemDescription;

    Transform toolBar;
    Button close_Button;
    Button add_Button;
    Button equip_Button;
    Button consume_Button;
    Button etc_Button;
    Button craft_Button;
    Button sort_Button;
    Button enhance_Button;

    Image equipButtonColor;
    Image consumeButtonColor;
    Image etcButtonColor;
    Image craftButtonColor;

    bool is_Inventory_Open = false;
    public bool is_Initialized;

    public delegate void Inventory_State_Changed(Current_Inventory_State state); //state가 바뀌면 setter가 호출할 delegate
    Inventory_State_Changed inventory_changed;

    private  Current_Inventory_State state;
    public  Current_Inventory_State State
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
    bool enhancerOpen = false;
    public bool EnhancerOpen
    {
        get => enhancerOpen;
        set
        {
            enhancerOpen = value;
            if (enhancerOpen)
            {
                ItemEnhancer.SetActive(true);
            }
            else
            {
                ItemEnhancer.SetActive(false);
            }
        }
    }
    private void Awake()
    {
        toolBar = transform.GetChild(0);
        close_Button = toolBar.GetChild(0).GetComponent<Button>();
        add_Button = toolBar.GetChild(1).GetComponent<Button>();
        sort_Button = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        enhance_Button = toolBar.GetChild(3).GetComponent<Button>();
        equip_Button = transform.GetChild(1).GetComponent<Button>();
        consume_Button = transform.GetChild(2).GetComponent<Button>();
        etc_Button = transform.GetChild(3).GetComponent<Button>();
        craft_Button = transform.GetChild(4).GetComponent<Button>();


        equipButtonColor = equip_Button.GetComponent<Image>();
        consumeButtonColor = consume_Button.GetComponent<Image>();
        etcButtonColor = etc_Button.GetComponent<Image>();
        craftButtonColor = craft_Button.GetComponent<Image>();

        sort_Button.onClick.AddListener(SlotSorting);
        close_Button.onClick.AddListener(Open_Inventory);
        add_Button.onClick.AddListener(GameManager.SlotManager.Make_Slot);
        enhance_Button.onClick.AddListener(Change_EnhancerOpen_Property);
        equip_Button.onClick.AddListener(SwitchTab_To_Equip);
        consume_Button.onClick.AddListener(SwitchTab_To_Consume);
        etc_Button.onClick.AddListener(SwitchTab_To_Etc);
        craft_Button.onClick.AddListener(SwitchTab_To_Craft);

        ItemDescription = transform.GetChild(9).gameObject;
        ItemDescription.SetActive(true);
    }
    void SlotSorting() //addListener 로 매개변수필요한 함수 바로 등록이 안되서 우회접근
    {
        GameManager.SlotManager.SlotSorting(ItemSortBy.Price, false);
    }
    void Change_EnhancerOpen_Property()
    {
        EnhancerOpen = !EnhancerOpen;
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
        equipButtonColor.color = Color.white;
        consumeButtonColor.color = Color.white;
        etcButtonColor.color = Color.white;
        craftButtonColor.color = Color.white;


        switch (state)
        {
            case Current_Inventory_State.Equip:
                Equip_Inven.SetActive(true);
                equipButtonColor.color = Color.grey;
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Consume:
                Consume_Inven.SetActive(true);
                consumeButtonColor.color = Color.grey;
                Equip_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Etc:
                Etc_Inven.SetActive(true);
                etcButtonColor.color = Color.gray;
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Current_Inventory_State.Craft:
                Craft_Inven.SetActive(true);
                craftButtonColor.color = Color.grey;
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
    public void RefreshOrder()
    {
        this.transform.SetAsFirstSibling();
    }
}
