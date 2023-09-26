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
    /// 다크포스값 보여줄 텍스트
    /// </summary>
    TextMeshProUGUI darkForceText;

    /// <summary>
    /// 코인 값 보여줄 텍스트 
    /// </summary>
    TextMeshProUGUI coinText;

    /// <summary>
    /// 구입 목록 버튼 
    /// </summary>
    Button buyButton; 

    /// <summary>
    /// 판매 물품 버튼
    /// </summary>
    Button sellButton;



    /// <summary>
    /// 장비 버튼
    /// </summary>
    Button equipButton;

    /// <summary>
    /// 소비 버튼
    /// </summary>
    Button consumeButton;

    /// <summary>
    /// 기타 버튼 
    /// </summary>
    Button etcButton;

    /// <summary>
    /// 조합 버튼
    /// </summary>
    Button craftButton;


    /// <summary>
    /// NPC 모습 보여줄 로우 이미지
    /// </summary>
    RawImage rawImage;

    /// <summary>
    /// npc 이름이 들어갈 위치
    /// </summary>
    TextMeshProUGUI npcName;

    /// <summary>
    /// 대화내용이 저장될 위치
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// 다음 대화내용으로넘기는버튼
    /// </summary>
    Button talkNextButton;

    /// <summary>
    /// 로그 버튼 
    /// </summary>
    Button talkLogButton;


    /// <summary>
    /// 판매 목록 의 아이템이 들어갈 컨텐츠 위치 
    /// </summary>
    Transform content;

    /// <summary>
    /// 아이템 갯수늘어나면 부모컨텐츠 사이즈 늘리기위해 가져온다
    /// </summary>
    RectTransform contentRect;

    /// <summary>
    /// 아이템 컨텐츠 기본 높이 저장해두기 
    /// </summary>
    float defaultItemSizeY;


    /// <summary>
    /// 엔피씨 근처로갈때 보여줄 버튼
    /// </summary>
    Button NpcTalk;




    /// <summary>
    /// 기능분리용 데이터 담겨있는 매니저
    /// </summary>
    Merchant_Manager merchant_Manager;

    /// <summary>
    /// 확인 처리용 팝업창
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


        //최소 한개넣어두고 미리 사이즈구해둔다 
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
    /// 데이터가변해서 상점UI 전체를 리플래쉬해주는 함수
    /// </summary>
    public void ReFresh_Merchant_Item() 
    {
       
        Merchant_UI_Item[] merchant_UI_Items = null;
        if (merchant_Manager.Selected == Merchant_Selected.Sell)
        {
            List<Slot> invenSlots;
            InitUI();

            switch (merchant_Manager.Merchant_State) //현재 상태에 따른 데이터 가져오고 
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
                if (itemSlot.ItemData != null) //슬롯에 데이터가 들어있으면 가져온다 
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
            switch (merchant_Manager.Merchant_State) //현재 상태에 따른 데이터 가져오고 
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
        
        //부모 컨텐츠 사이즈 조절 
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, defaultItemSizeY* merchant_UI_Items.Length);
    }

    /// <summary>
    /// 필요한 갯수만큼 오브젝트 풀에서 가져오는 함수
    /// 리셋할때 풀로 전부 돌리고있어서 체크하는 부분은 필요가없을거같긴하다..
    /// </summary>
    /// <param name="checkCount">보여줄 갯수</param>
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
    /// 기존 UI 데이터 리셋시키기
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
