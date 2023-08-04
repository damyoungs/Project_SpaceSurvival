using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Item_Enhancer_UI : MonoBehaviour
{ 
    CanvasGroup canvasGroup;
    Item_Enhancer itemEnhancer;

    public UnityEngine.UI.Button closeButton;
    public UnityEngine.UI.Button cancelButton;
    public UnityEngine.UI.Button plusButton;
    public UnityEngine.UI.Button minusButton;
    public UnityEngine.UI.Button confirmButton;
    public TextMeshProUGUI successRateText;
    public TextMeshProUGUI beforelevelText;
    public TextMeshProUGUI afterlevelText;


    public TMP_InputField amountText;
    public UnityEngine.UI.Slider amountSlider;

    public GameObject warningBox;
    
    Enhancer_Slot_Before beforeSlot;
    Enhancer_Slot_After afterSlot;

    public Action<ItemData_Enhancable> onDarkForceValurChange;

    const uint MinDarkForceCount = 0;
    uint darkForceCount = MinDarkForceCount;
    byte itemLevel = 0;
    public uint DarkForceCount
    {
        get => darkForceCount;
        set
        {
            darkForceCount = Math.Clamp(value, MinDarkForceCount, (uint)GameManager.playerDummy.DarkForce);
            amountText.text = darkForceCount.ToString();    // 인풋 필드에 적용
            amountSlider.value = darkForceCount;
            onDarkForceValurChange?.Invoke(itemEnhancer.ItemData);
        }
    }
    public Enhancer_Slot_Before BeforeSlot => beforeSlot;
    public Enhancer_Slot_After AfterSlot => afterSlot;

    private void Awake()
    {
        itemEnhancer = GetComponent<Item_Enhancer>();
        canvasGroup = GetComponent<CanvasGroup>();
        beforeSlot = GetComponentInChildren<Enhancer_Slot_Before>();
        afterSlot = GetComponentInChildren<Enhancer_Slot_After>();

        closeButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Close);// 수정필요
        cancelButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Close);

        confirmButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Confirm);
     
        plusButton.onClick.AddListener(() => DarkForceCount++);
        minusButton.onClick.AddListener(() => DarkForceCount--);


        amountText.onValueChanged.AddListener((text) =>
        {
            if (uint.TryParse(text, out uint result))
            {
                DarkForceCount = result;
            }
            else
            {
                DarkForceCount = MinDarkForceCount;
            }
        });
        amountSlider.onValueChanged.AddListener((ratio) =>
        {
            DarkForceCount = (uint)ratio;
        });
    }
    private void Start()
    {

        itemEnhancer.onOpen += Open;
        itemEnhancer.onClose += Close;
        itemEnhancer.onSetItem += RefreshEnhancerUI;
        itemEnhancer.onClearItem += ClearEnhancerUI;
        itemEnhancer.onConfirmButtonClick += WarningBoxOn;
        onDarkForceValurChange += UpdateSuccessRate;
        itemEnhancer.onWaitforResult += WaitForResult;
        Close();

        WarningBox warningBox = FindObjectOfType<WarningBox>();
        warningBox.onWarningBoxClose += WarningBoxOff;
    }
    public void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ClearEnhancerUI();
    }
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        DarkForceCount = 0;
        itemEnhancer.ItemData = null;
        BeforeSlot.ItemData = null;
        AfterSlot.ItemData = null;
        successRateText.text = "0";
    }
    void RefreshEnhancerUI(ItemData_Enhancable itemData)
    {
        if (itemData != null)
        {
            itemLevel = itemEnhancer.ItemData.Itemlevel;
            beforelevelText.text = $"{itemLevel}";
            afterlevelText.text = $"{itemLevel + 1}";
            amountSlider.maxValue = GameManager.playerDummy.DarkForce;
            UpdateSuccessRate(itemData);
        }
    }

    private void ClearEnhancerUI()
    {
        //이미지, 성공률 text
        beforelevelText.text = string.Empty;
        afterlevelText.text = string.Empty;
        amountSlider.maxValue = 0.0f;
        amountSlider.value = 0;
        amountText.text = "0";
        successRateText.text = $"{0}";
        beforeSlot.ItemData = null;
        afterSlot.ItemData = null;
        itemEnhancer.ItemData = null;
    }
    void WarningBoxOn()//Enhancer에서 신호 받음 
    {
        beforeSlot.imageComp.raycastTarget = false;
        beforeSlot.itemIcon.raycastTarget = false;
        confirmButton.interactable = false;
        cancelButton.interactable = false;
        closeButton.interactable = false;
        amountSlider.interactable = false;
        amountText.interactable= false;
        plusButton.interactable = false;
        minusButton.interactable = false;
    }
    void WarningBoxOff()//warningbox에서 신호받음
    {
        beforeSlot.imageComp.raycastTarget = true;
        beforeSlot.itemIcon.raycastTarget = true;
        confirmButton.interactable = true;
        cancelButton.interactable = true;
        closeButton.interactable = true;
        amountSlider.interactable = true;
        amountText.interactable = true;
        plusButton.interactable = true;
        minusButton.interactable = true;
    }
    void UpdateSuccessRate(ItemData item)//확률 계산은 IEnhancable 에서 직접하는게 좋을것 같다. 필요한 데이터가 모두 거기있기때문이다.
    {
        if (item == null)
        {
            return;
        }
          
        ItemData_Enhancable enhancable = item as ItemData_Enhancable;
        if (enhancable != null)
        {
            successRateText.text = enhancable.CalculateSuccessRate(DarkForceCount).ToString("f1");
        }
    }
    void WaitForResult()
    {
        ItemData_Enhancable tempData = itemEnhancer.ItemData;// 프로퍼티는 함수라서 바로 ref로 넘겨줄 수 없다
        if (itemEnhancer.ItemData.LevelUp(DarkForceCount))//
        {
            StartCoroutine(PopUpSuccessEffect());
        }
        else
        {
            StartCoroutine(PopUpFailEffect());
        }
       
    }
    IEnumerator PopUpSuccessEffect()
    {
        //이펙트 팝업
        Debug.Log("강화 성공 대기 시작");
        yield return new WaitForSeconds(3.0f);
        itemEnhancer.EnhancerState = EnhancerState.Success;
        Debug.Log("강화 성공 대기시간 종료");
    }
    IEnumerator PopUpFailEffect()
    {
        //이펙트 팝업
        Debug.Log("강화 실패 대기 시작");
        yield return new WaitForSeconds(3.0f);
        itemEnhancer.EnhancerState = EnhancerState.Fail;
        Debug.Log("강화 실패 대기시간 종료");
    }
}
