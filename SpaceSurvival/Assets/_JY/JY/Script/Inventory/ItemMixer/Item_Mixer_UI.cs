using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Item_Mixer_UI : MonoBehaviour
{ 
    CanvasGroup canvasGroup;
    Item_Enhancer itemEnhancer;

    public UnityEngine.UI.Button closeButton;
    public UnityEngine.UI.Button cancelButton;
    public UnityEngine.UI.Button plusButton;
    public UnityEngine.UI.Button minusButton;
    public UnityEngine.UI.Button confirmButton;
    public TextMeshProUGUI successRateText;
    public Animator proceedAnim;

    public TMP_InputField amountText;
    public UnityEngine.UI.Slider amountSlider;
    
    Mixer_Slot_Left left_Slot;
    Mixer_Slot_Middle middle_Slot;
    Mixer_Slot_Result result_Slot;

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
    public Mixer_Slot_Left Left_Slot => left_Slot;
    public Mixer_Slot_Middle Middle_Slot => middle_Slot;
    public Mixer_Slot_Result Result_Slot => result_Slot;

    private void Awake()
    {
        itemEnhancer = GetComponent<Item_Enhancer>();
        canvasGroup = GetComponent<CanvasGroup>();

        left_Slot = GetComponentInChildren<Mixer_Slot_Left>();
        middle_Slot = GetComponentInChildren<Mixer_Slot_Middle>();
        result_Slot = GetComponentInChildren<Mixer_Slot_Result>();

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
        left_Slot.ItemData = null;
        middle_Slot.ItemData = null;
        result_Slot.ItemData = null;
        successRateText.text = "0";
    }
    void RefreshEnhancerUI(ItemData_Enhancable itemData)
    {
        if (itemData != null)
        {
            itemLevel = itemEnhancer.ItemData.Itemlevel;
            amountSlider.maxValue = GameManager.playerDummy.DarkForce;
            UpdateSuccessRate(itemData);
        }
    }

    private void ClearEnhancerUI()
    {
        //�̹���, ������ text
        amountSlider.maxValue = 0.0f;
        amountSlider.value = 0;
        amountText.text = "0";
        successRateText.text = $"{0}";
        left_Slot.ItemData = null;
        middle_Slot.ItemData = null;
        result_Slot.ItemData = null;
        itemEnhancer.ItemData = null;
    }
    void BlockInteractable()//Enhancer���� ��ȣ ���� 
    {
        left_Slot.imageComp.raycastTarget = false;
        middle_Slot.itemIcon.raycastTarget = false;
        result_Slot.itemIcon.raycastTarget = false;
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
        left_Slot.imageComp.raycastTarget = true;
        middle_Slot.itemIcon.raycastTarget = true;
        result_Slot.itemIcon.raycastTarget = true;
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

}
