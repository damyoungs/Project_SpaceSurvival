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
    Mixer_Anim mixer_Anim;

    public UnityEngine.UI.Button closeButton;
    public UnityEngine.UI.Button cancelButton;
    public UnityEngine.UI.Button plusButton;
    public UnityEngine.UI.Button minusButton;
    public UnityEngine.UI.Button confirmButton;
    public TextMeshProUGUI successRateText;

    public TMP_InputField amountText;
    public UnityEngine.UI.Slider amountSlider;
    
    Mixer_Slot_Left left_Slot;
    Mixer_Slot_Middle middle_Slot;
    Mixer_Slot_Result result_Slot;

    public Action<ItemData> onDarkForceValueChange;
    public Action<ItemData> onEndSession_Success;
    public Action onTriggerLevelUp;

    const uint MinDarkForceCount = 0;
    uint darkForceCount = MinDarkForceCount;

    bool isCritical = false; // 조합 크리티컬이 터졌는지 확인용
    public bool IsOpen => canvasGroup.alpha == 1.0f;
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
        mixer.onClearItem += ClearMixerUI; // beforeSlot을 클릭했을 때
        mixer.onConfirmButtonClick += BlockInteractable;// enhancerUI창의 체크버튼 클맀했을 때
        onDarkForceValueChange += UpdateSuccessRate; // InputField의 Darkforce의 값이 바뀔때
        //mixer.onWaitforResult += WaitForResult; // warningBox의 체크버튼을 누를 때 
       // itemEnhancer.onWaitforResult += BlockInteractable;

        mixer.onSetItem += RefreshMixerUI;
        mixer.onSetItemCanceled += InitializeUIElements;

        mixer.onWaitforResult += WaitForResult;
        mixer.onWaitforResult += BlockInteractable;
        Mixing_Table = GameManager.Mixing_Table;
        mixer_Anim = mixer.Mixer_Anim;


        mixer_Anim.done_With_Success_Anim += EndSession_Success;//성공시 EndSession 실행 전 resultSlot의 데이터를 인벤토리에 넣어줘야함
        mixer_Anim.done_With_Fail_Anim += EndSession;
        Close();

        WarningBox warningBox = FindObjectOfType<WarningBox>();
        warningBox.onWarningBoxClose += OpenInteractable;
    }
    void EndSession_Success()
    {
        onEndSession_Success?.Invoke(Result_Slot.ItemData);
        EndSession();
    }
    void EndSession()
    {
        mixer.Return_To_Inventory = false;
        OpenInteractable();
        ClearMixerUI();
    }
    public void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ClearMixerUI();
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

    private void ClearMixerUI()
    {
        //이미지, 성공률 text
        InitializeUIElements();
        mixer.LeftSlotData = null;// 직접 수정하는 대신 Mixer의 프로퍼티를 null로 만들면 강화 시도 후 실패한 것이 아니라 올렸다가 취소한 것으로 간주해서 다시 인벤토리에 추가시킴
        mixer.MiddleSlotData = null;
        mixer.ResultSlot.ItemData = null;
        // mixer.ItemData = null;
    }

    private void InitializeUIElements()
    {
        amountSlider.maxValue = 0.0f;
        amountSlider.value = 0;
        amountText.text = "0";
        successRateText.text = $"{0}";
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
    void WaitForResult()
    {
        if (mixer.Mixing_Table.LevelUp(mixer.ResultSlot.ItemData, DarkForceCount, out bool critical))//
        {
            isCritical = critical;
            mixer.MixerState = ItemMixerState.Success;
            Debug.Log($"{isCritical} : 성공");
        }
        else
        {
            mixer.MixerState = ItemMixerState.Fail;
            Debug.Log("실패");
        }
    }


}
