using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Merchant_UI_Manager : PopupWindowBase , IPopupSortWindow
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
    /// 기능분리용 데이터 담겨있는 매니저
    /// </summary>
    Merchant_Manager merchant_Manager;



    public Action<IPopupSortWindow> PopupSorting { get; set; }

    protected override void Awake()
    {
        base.Awake();
        
        merchant_Manager = GetComponent<Merchant_Manager>();
        
        buyButton = topPanel.GetChild(1).GetChild(0).GetComponent<Button>(); 
        buyButton.onClick.AddListener(() => {
            
        });
        
        sellButton = topPanel.GetChild(1).GetChild(1).GetComponent<Button>(); 
        sellButton.onClick.AddListener(() => { 
            
        });

        coinText = topPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
      
        darkForceText = topPanel.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();



        equipButton = contentPanel.GetChild(0).GetChild(0).GetComponent<Button>();
        equipButton.onClick.AddListener(() => { 
        
        });

        consumeButton = contentPanel.GetChild(0).GetChild(1).GetComponent<Button>();
        consumeButton.onClick.AddListener(() => { 
        
        });

        etcButton = contentPanel.GetChild(0).GetChild(2).GetComponent<Button>();
        etcButton.onClick.AddListener(() => { 
        
        });

        craftButton = contentPanel.GetChild(0).GetChild(3).GetComponent<Button>();
        craftButton.onClick.AddListener(() => { 
        
        });

        content = contentPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;

        rawImage = contentPanel.GetChild(2).GetComponentInChildren<RawImage>();
        npcName = contentPanel.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = contentPanel.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        talkNextButton = contentPanel.GetChild(2).GetChild(3).GetComponent<Button>();
        talkNextButton.onClick.AddListener(() => { 
        
        });
        talkLogButton = contentPanel.GetChild(2).GetChild(3).GetComponent<Button>();
        talkLogButton.onClick.AddListener(() => { 
        
        });
    }










    private void ResetData() 
    {

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
        this.gameObject.SetActive(false);
    }


}
