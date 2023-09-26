using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Merchant_UI_Manager : PopupWindowBase, IPopupSortWindow
{

   
    


    /// <summary>
    /// ��ũ������ ������ �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI darkForceText;

    /// <summary>
    /// ���� �� ������ �ؽ�Ʈ 
    /// </summary>
    TextMeshProUGUI coinText;

    /// <summary>
    /// ���� ��� ��ư 
    /// </summary>
    Button buyButton; 

    /// <summary>
    /// �Ǹ� ��ǰ ��ư
    /// </summary>
    Button sellButton;



    /// <summary>
    /// ��� ��ư
    /// </summary>
    Button equipButton;

    /// <summary>
    /// �Һ� ��ư
    /// </summary>
    Button consumeButton;

    /// <summary>
    /// ��Ÿ ��ư 
    /// </summary>
    Button etcButton;

    /// <summary>
    /// ���� ��ư
    /// </summary>
    Button craftButton;


    /// <summary>
    /// NPC ��� ������ �ο� �̹���
    /// </summary>
    RawImage rawImage;

    /// <summary>
    /// npc �̸��� �� ��ġ
    /// </summary>
    TextMeshProUGUI npcName;

    /// <summary>
    /// ��ȭ������ ����� ��ġ
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// ���� ��ȭ�������γѱ�¹�ư
    /// </summary>
    Button talkNextButton;

    /// <summary>
    /// �α� ��ư 
    /// </summary>
    Button talkLogButton;


    /// <summary>
    /// �Ǹ� ��� �� �������� �� ������ ��ġ 
    /// </summary>
    Transform content;

    /// <summary>
    /// ������ �����þ�� �θ������� ������ �ø������� �����´�
    /// </summary>
    RectTransform contentRect;

    /// <summary>
    /// ������ ������ �⺻ ���� �����صα� 
    /// </summary>
    float defaultItemSizeY;


    /// <summary>
    /// ���Ǿ� ��ó�ΰ��� ������ ��ư
    /// </summary>
    Button NpcTalk;




    /// <summary>
    /// ��ɺи��� ������ ����ִ� �Ŵ���
    /// </summary>
    Merchant_Manager merchant_Manager;

    /// <summary>
    /// Ȯ�� ó���� �˾�â
    /// </summary>
    MerchantModalPopup popup;

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    NpcTalkController talkController;

    protected override void Awake()
    {
        base.Awake();
        
        merchant_Manager = GetComponent<Merchant_Manager>();

        talkController = FindObjectOfType<NpcTalkController>();
        talkController.openTalkWindow += () => {
            gameObject.SetActive(true);
        };
        talkController.closeTalkWindow += () => {
            ResetData();  
        };

        popup = transform.parent.GetComponentInChildren<MerchantModalPopup>(true);

        buyButton = topPanel.GetChild(1).GetChild(0).GetComponent<Button>(); 
        buyButton.onClick.AddListener(() => {
            Debug.Log(buyButton);
            merchant_Manager.Selected = Merchant_Selected.Buy;
            ReFresh_Merchant_Item();
        });
        
        sellButton = topPanel.GetChild(1).GetChild(1).GetComponent<Button>(); 
        sellButton.onClick.AddListener(() => { 
            Debug.Log(sellButton);
            merchant_Manager.Selected = Merchant_Selected.Sell;
            ReFresh_Merchant_Item();
        });

        coinText = topPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
      
        darkForceText = topPanel.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();



        equipButton = contentPanel.GetChild(0).GetChild(0).GetComponent<Button>();
        equipButton.onClick.AddListener(() => {
            merchant_Manager.Merchant_State = Current_Inventory_State.Equip;
            ReFresh_Merchant_Item();
        });

        consumeButton = contentPanel.GetChild(0).GetChild(1).GetComponent<Button>();
        consumeButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Current_Inventory_State.Consume;
            ReFresh_Merchant_Item();
        });

        etcButton = contentPanel.GetChild(0).GetChild(2).GetComponent<Button>();
        etcButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Current_Inventory_State.Etc;
            ReFresh_Merchant_Item();
        });

        craftButton = contentPanel.GetChild(0).GetChild(3).GetComponent<Button>();
        craftButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Current_Inventory_State.Craft;
            ReFresh_Merchant_Item();
        });

        content = contentPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
        contentRect = content.transform.GetComponent<RectTransform>();

        rawImage = contentPanel.GetChild(2).GetComponentInChildren<RawImage>();
        npcName = contentPanel.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = contentPanel.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        talkNextButton = contentPanel.GetChild(2).GetChild(3).GetComponent<Button>();
        talkNextButton.onClick.AddListener(() => { 
            Debug.Log(talkNextButton);
        
        });
        talkLogButton = contentPanel.GetChild(2).GetChild(4).GetComponent<Button>();
        talkLogButton.onClick.AddListener(() => { 
            Debug.Log(talkLogButton);
        
        });


        //�ּ� �Ѱ��־�ΰ� �̸� ������صд� 
        defaultItemSizeY = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;


    }
    private void Start()
    {
       ResetData();
    }



    public bool IsSelect() 
    {
        bool result = false;


        return result;
    }

    private void OnDisable()
    {
        popup.OnClose();
    }


    /// <summary>
    /// �����Ͱ����ؼ� ����UI ��ü�� ���÷������ִ� �Լ�
    /// </summary>
    public void ReFresh_Merchant_Item() 
    {
       
        Merchant_UI_Item[] merchant_UI_Items = null;
        if (merchant_Manager.Selected == Merchant_Selected.Sell)
        {
            List<Slot> invenSlots;
            InitUI();

            switch (merchant_Manager.Merchant_State) //���� ���¿� ���� ������ �������� 
            {
                case Current_Inventory_State.None:
                    return;
                case Current_Inventory_State.Equip:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Current_Inventory_State.Consume:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Current_Inventory_State.Etc:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Current_Inventory_State.Craft:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                default:
                    return;
            }

            List<Slot> merchantSettingData = new List<Slot>(invenSlots.Count);
            foreach (Slot itemSlot in invenSlots)
            {
                if (itemSlot.ItemData != null) //���Կ� �����Ͱ� ��������� �����´� 
                {
                    merchantSettingData.Add(itemSlot);
                }
            }

            UI_MerchantItemSetting(merchantSettingData.Count);

            merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();

            int itemIndex = 0;
            foreach (Slot itemSlot in merchantSettingData)
            {
                merchant_UI_Items[itemIndex].InitData(itemSlot.ItemData, itemSlot.ItemCount,itemSlot);
                merchant_UI_Items[itemIndex].onItemClick = popup.OnPopup;// merchant_Manager.ItemClick;
                merchant_UI_Items[itemIndex].gameObject.SetActive(true);
                itemIndex++;
            }
        }
        else if (merchant_Manager.Selected == Merchant_Selected.Buy)
        {
            InitUI();
            List<ItemData> merchantList;
            int[] merchantCountArray;
            switch (merchant_Manager.Merchant_State) //���� ���¿� ���� ������ �������� 
            {
                case Current_Inventory_State.None:
                    return;
                case Current_Inventory_State.Equip:
                    merchantList = merchant_Manager.EquipItems;
                    merchantCountArray = merchant_Manager.EquipItemsCount;
                    break;
                case Current_Inventory_State.Consume:
                    merchantList = merchant_Manager.ConsumeItems;
                    merchantCountArray = merchant_Manager.ConsumeItemsCount;
                    break;
                case Current_Inventory_State.Etc:
                    merchantList = merchant_Manager.EtcItems;
                    merchantCountArray = merchant_Manager.EtcItemsCount;
                    break;
                case Current_Inventory_State.Craft:
                    merchantList = merchant_Manager.CraftItems;
                    merchantCountArray = merchant_Manager.CraftItemsCount;
                    break;
                default:
                    return;
            }
            UI_MerchantItemSetting(merchantList.Count);

            merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();

            int itemIndex = 0;
            foreach (ItemData itemSlot in merchantList)
            {
                merchant_UI_Items[itemIndex].InitData(itemSlot,(uint)merchantCountArray[itemIndex]);
                merchant_UI_Items[itemIndex].onItemClick = popup.OnPopup;//merchant_Manager.ItemClick;
                merchant_UI_Items[itemIndex].gameObject.SetActive(true);
                itemIndex++;
            }
        }
        else 
        {
            return;
        }
        
        //�θ� ������ ������ ���� 
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, defaultItemSizeY* merchant_UI_Items.Length);
    }

    /// <summary>
    /// �ʿ��� ������ŭ ������Ʈ Ǯ���� �������� �Լ�
    /// �����Ҷ� Ǯ�� ���� �������־ üũ�ϴ� �κ��� �ʿ䰡�����Ű����ϴ�..
    /// </summary>
    /// <param name="checkCount">������ ����</param>
    private void UI_MerchantItemSetting(int checkCount) 
    {
        if (checkCount > content.childCount)
        {
            Merchant_UI_Item item; 
            int forSize = checkCount - content.childCount;
            for (int i = 0; i < forSize; i++)
            {
                item = (Merchant_UI_Item)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.MERCHANT_iTEM_POLL);
                item.transform.SetParent(content);
            }
        }
    }

    /// <summary>
    /// ���� UI ������ ���½�Ű��
    /// </summary>
    private void InitUI() 
    {
        Merchant_UI_Item[] merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>(); //

        foreach (Merchant_UI_Item slotItem in merchant_UI_Items)
        {
            slotItem.ResetData();
        }
    }

    private void ResetData() 
    {
        Merchant_UI_Item[] merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();
        foreach (Merchant_UI_Item item in merchant_UI_Items)
        {
            item.ResetData();
        }
        gameObject.SetActive(false);
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        PopupSorting(this);
    }

    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        ResetData();
    }


}
