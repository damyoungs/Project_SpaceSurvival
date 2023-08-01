using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_Enhancer_UI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public Button closeButton;
    public Button cancelButton;
    public Button plusButton;
    public Button minusButton;
    public Button confirmButton;

    public TMP_InputField amountText;
    public Slider amountSlider;

    public GameObject warningBox;
    
    Enhancer_Slot_Before beforeSlot;
    Enhancer_Slot_After afterSlot;

    

    public Enhancer_Slot_Before BeforeSlot => beforeSlot;
    public Enhancer_Slot_After AfterSlot => afterSlot;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        beforeSlot = GetComponentInChildren<Enhancer_Slot_Before>();
        afterSlot = GetComponentInChildren<Enhancer_Slot_After>();

        closeButton.onClick.AddListener(GameManager.Inventory.Change_EnhancerOpen_Property);
        cancelButton.onClick.AddListener(GameManager.Inventory.Change_EnhancerOpen_Property);
    }
    private void Start()
    {
        GameManager.Inventory.onEnHancerOpen += Open;
        GameManager.Inventory.onEnHancerClose += Close;
        GameManager.SlotManager.setEnhanceItem += (_) => RefreshEnhancerUI();
    }
    void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    void RefreshEnhancerUI()
    {
       // amountSlider.value = GameManager.Inst.player.
    }
}
