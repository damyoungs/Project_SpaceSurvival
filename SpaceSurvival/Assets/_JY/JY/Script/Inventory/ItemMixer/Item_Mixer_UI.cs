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
    Item_Mixer mixer;

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

    public Action<ItemData> onDarkForceValueChange;
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
            amountText.text = darkForceCount.ToString();    // 인풋 필드에 적용
            amountSlider.value = darkForceCount;
            onDarkForceValueChange?.Invoke(Result_Slot.ItemData);
        }
    }
    public Mixer_Slot_Left Left_Slot => left_Slot;
    public Mixer_Slot_Middle Middle_Slot => middle_Slot;
    public Mixer_Slot_Result Result_Slot => result_Slot;
    Item_Mixing_Table Mixing_Table;

    private void Awake()
    {
        mixer = GetComponent<Item_Mixer>();
        canvasGroup = GetComponent<CanvasGroup>();

        left_Slot = GetComponentInChildren<Mixer_Slot_Left>();
        middle_Slot = GetComponentInChildren<Mixer_Slot_Middle>();
        result_Slot = GetComponentInChildren<Mixer_Slot_Result>();

        closeButton.onClick.AddListener(() => mixer.MixerState = ItemMixerState.Close);// 수정필요
        cancelButton.onClick.AddListener(() => mixer.MixerState = ItemMixerState.Close);

        confirmButton.onClick.AddListener(() => mixer.MixerState = ItemMixerState.Confirm);
     
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

        mixer.onOpen += Open;
        mixer.onClose += Close;
        //mixer.onSetItem += RefreshEnhancerUI; //tempSlot의 아이템을 드롭했을 때
        mixer.onClearItem += ClearEnhancerUI; // beforeSlot을 클릭했을 때
        mixer.onConfirmButtonClick += BlockInteractable;// enhancerUI창의 체크버튼 클맀했을 때
        onDarkForceValueChange += UpdateSuccessRate; // InputField의 Darkforce의 값이 바뀔때
        //mixer.onWaitforResult += WaitForResult; // warningBox의 체크버튼을 누를 때 
       // itemEnhancer.onWaitforResult += BlockInteractable;
        mixer.onSuccess += () => StartCoroutine(PopUp_ProceedBox(true)); //WaitForResult에서 호출
        mixer.onFail += () => StartCoroutine(PopUp_ProceedBox(false));

        mixer.onSetItem += RefreshMixerUI;
        Mixing_Table = GameManager.Mixing_Table;



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
      //  mixer.ItemData = null;
        mixer.LeftSlotData= null;
        mixer.MiddleSlotData = null;
        result_Slot.ItemData = null;
        successRateText.text = "0";
    }
    void RefreshMixerUI(ItemData resultData)
    {
        if (resultData != null)
        {
            amountSlider.maxValue = GameManager.playerDummy.DarkForce;
            UpdateSuccessRate(resultData);
        }
    }

    private void ClearEnhancerUI()
    {
        //이미지, 성공률 text
        amountSlider.maxValue = 0.0f;
        amountSlider.value = 0;
        amountText.text = "0";
        successRateText.text = $"{0}";
        left_Slot.ItemData = null;
        middle_Slot.ItemData = null;
        result_Slot.ItemData = null;
       // mixer.ItemData = null;
    }
    void BlockInteractable()//Enhancer에서 신호 받음 
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
    void OpenInteractable()//warningbox에서 신호받음
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
    void UpdateSuccessRate(ItemData item)//확률 계산은 IEnhancable 에서 직접하는게 좋을것 같다. 필요한 데이터가 모두 거기있기때문이다.
    {
        if (item == null)
        {
            return;
        }
        successRateText.text = Mixing_Table.CalculateSuccessRate(result_Slot.ItemData, darkForceCount).ToString();
    }
    //void WaitForResult()
    //{
    //    if (mixer.ItemData.LevelUp(DarkForceCount))//
    //    {
    //        itemEnhancer.EnhancerState = EnhancerState.Success;
    //    }
    //    else
    //    {
    //        itemEnhancer.EnhancerState = EnhancerState.Fail;
    //    }
    //}
    IEnumerator PopUp_ProceedBox(bool levelUp)
    {
        if (levelUp)
        {
            proceedAnim.SetTrigger("Success");
            yield return new WaitForSeconds(3.0f);// Success clip의 재생시간을 고려한 딜레이
            onTriggerLevelUp?.Invoke();
            mixer.MixerState = ItemMixerState.ClearItem;
            Debug.Log("State 변경 ");
        }
        else
        {
            proceedAnim.SetTrigger("Fail");
            yield return new WaitForSeconds(3.0f);//대기시간이 없으면 버튼 활성화가 너무 빨리된다.
        }
        OpenInteractable();
    }

}
