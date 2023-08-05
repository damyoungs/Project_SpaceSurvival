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

    public Action<ItemData_Enhancable> onDarkForceValueChange;
    public Action onEffectEnd;

    const uint MinDarkForceCount = 0;
    uint darkForceCount = MinDarkForceCount;
    byte itemLevel = 0;
    public uint DarkForceCount
    {
        get => darkForceCount;
        set
        {
            darkForceCount = Math.Clamp(value, MinDarkForceCount, (uint)GameManager.playerDummy.DarkForce);
            amountText.text = darkForceCount.ToString();    // ��ǲ �ʵ忡 ����
            amountSlider.value = darkForceCount;
            onDarkForceValueChange?.Invoke(itemEnhancer.ItemData);
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

        closeButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Close);// �����ʿ�
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
        onDarkForceValueChange += UpdateSuccessRate;
        itemEnhancer.onWaitforResult += WaitForResult;
        itemEnhancer.onSuccess += () => StartCoroutine(PopUpSuccessEffect());
        itemEnhancer.onFail += () => StartCoroutine(PopUpFailEffect());

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
        //�̹���, ������ text
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
    void WarningBoxOn()//Enhancer���� ��ȣ ���� 
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
    void WarningBoxOff()//warningbox���� ��ȣ����
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
    void UpdateSuccessRate(ItemData item)//Ȯ�� ����� IEnhancable ���� �����ϴ°� ������ ����. �ʿ��� �����Ͱ� ��� �ű��ֱ⶧���̴�.
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
        ItemData_Enhancable tempData = itemEnhancer.ItemData;// ������Ƽ�� �Լ��� �ٷ� ref�� �Ѱ��� �� ����
        if (itemEnhancer.ItemData.LevelUp(DarkForceCount))//
        {
            itemEnhancer.EnhancerState = EnhancerState.Success;
        }
        else
        {
            itemEnhancer.EnhancerState = EnhancerState.Fail;
        }
    }
    IEnumerator PopUpSuccessEffect()
    {
        //����Ʈ �˾�
        yield return new WaitForSeconds(3.0f);
        Debug.Log("����Ʈ ����");
        onEffectEnd?.Invoke();
        //Enhancable�� ����Ʈ �����ٰ� ��ȣ ������
    }
    IEnumerator PopUpFailEffect()
    {
        //����Ʈ �˾�
        yield return new WaitForSeconds(3.0f);
    }
}
