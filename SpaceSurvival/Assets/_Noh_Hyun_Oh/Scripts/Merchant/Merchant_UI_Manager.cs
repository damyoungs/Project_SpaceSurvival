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
