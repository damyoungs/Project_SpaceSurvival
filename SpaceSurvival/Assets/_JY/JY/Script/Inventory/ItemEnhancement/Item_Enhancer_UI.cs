using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Item_Enhancer_UI : PopupWindowBase, IPopupSortWindow, IPointerDownHandler
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
    public Animator proceedAnim;

    public TMP_InputField amountText;
    public UnityEngine.UI.Slider amountSlider;
    
    Enhancer_Slot_Before beforeSlot;
    Enhancer_Slot_After afterSlot;

    public Action<ItemData_Enhancable> onDarkForceValueChange;
    public Action onTriggerLevelUp;

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

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

    protected override void Awake()
    {
        base.Awake();
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
        itemEnhancer.onSetItem += RefreshEnhancerUI; //tempSlot�� �������� ������� ��
        itemEnhancer.onClearItem += ClearEnhancerUI; // beforeSlot�� Ŭ������ ��
        itemEnhancer.onConfirmButtonClick += BlockInteractable;// enhancerUIâ�� üũ��ư Ŭ������ ��
        onDarkForceValueChange += UpdateSuccessRate; // InputField�� Darkforce�� ���� �ٲ�
        itemEnhancer.onWaitforResult += WaitForResult; // warningBox�� üũ��ư�� ���� �� 
       // itemEnhancer.onWaitforResult += BlockInteractable;
        itemEnhancer.onSuccess += () => StartCoroutine(PopUp_ProceedBox(true)); //WaitForResult���� ȣ��
        itemEnhancer.onFail += () => StartCoroutine(PopUp_ProceedBox(false));



        Close();

        WarningBox warningBox = FindObjectOfType<WarningBox>();
        warningBox.onWarningBoxClose += OpenInteractable;
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
    void BlockInteractable()//Enhancer���� ��ȣ ���� 
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
    void OpenInteractable()//warningbox���� ��ȣ����
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
        if (itemEnhancer.ItemData.LevelUp(DarkForceCount))//
        {
            itemEnhancer.EnhancerState = EnhancerState.Success;
        }
        else
        {
            itemEnhancer.EnhancerState = EnhancerState.Fail;
        }
    }
    IEnumerator PopUp_ProceedBox(bool levelUp)
    {
        if (levelUp)
        {
            proceedAnim.SetTrigger("Success");
            yield return new WaitForSeconds(3.0f);// Success clip�� ����ð��� ����� ������
            onTriggerLevelUp?.Invoke();
            itemEnhancer.EnhancerState = EnhancerState.ClearItem;
            Debug.Log("State ���� ");
        }
        else
        {
            proceedAnim.SetTrigger("Fail");
            yield return new WaitForSeconds(3.0f);//���ð��� ������ ��ư Ȱ��ȭ�� �ʹ� �����ȴ�.
        }
        OpenInteractable();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PopupSorting(this);
    }

    public void OpenWindow()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
    }

    public void CloseWindow()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.0f;
    }
}
