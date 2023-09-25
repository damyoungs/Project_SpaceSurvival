using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Merchant_UI_Manager : PopupWindowBase ,IPopupSortWindow
{

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
    /// ��ɺи��� ������ ����ִ� �Ŵ���
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
