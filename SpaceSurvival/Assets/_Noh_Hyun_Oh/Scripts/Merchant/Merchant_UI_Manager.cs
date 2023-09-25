using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Merchant_UI_Manager : PopupWindowBase ,IPopupSortWindow
{

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


        //talkNextButton = contentPanel.GetChild(2).GetChild();
        //talkLogButton

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
