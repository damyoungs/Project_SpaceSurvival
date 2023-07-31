using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_Enhancer_UI : MonoBehaviour
{
    public Button closeButton;
    public Button cancelButton;
    public Button plusButton;
    public Button minusButton;
    public Button confirmButton;

    public TMP_InputField amountText;
    public Slider amountSlider;

    public GameObject warningBox;
    
    public Enhancer_Slot_Before beforeSlot;
    public Enhancer_Slot_After afterSlot;

    private void Awake()
    {
        closeButton.onClick.AddListener(GameManager.Inventory.Change_EnhancerOpen_Property);
        cancelButton.onClick.AddListener(GameManager.Inventory.Change_EnhancerOpen_Property);
    }

}
