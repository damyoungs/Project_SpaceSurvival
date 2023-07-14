using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{

    [SerializeField]
  
    Text amount_Text;

    RectTransform rectTransform;
    RectTransform itemDescriptionTransform;
    TextMeshProUGUI itemDescription_Text;
    Animator anim;
    int popUpHash = Animator.StringToHash("PopUp");
    int itemCount;


    private ItemBase item;
    public ItemBase Item//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
    {
        get => item;
        set
        {
            item = value;
        }
    }
    public bool IsEmpty { get; set; } = true;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티
    public bool IsMoving { get; set; } = false; //이동중 description 팝업을 방지하기 위한 변수 
    
    public string CurrentItem { get;  set; }
    public int ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                UpdateAmountText(itemCount);
            }
        }
    }
    private void Awake()
    {
        amount_Text = transform.GetChild(1).GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
      
    }
    private void Start()
    {
        UpdateAmountText(0);
        GameManager.SlotManager.isMovingChange += IsMovingChange;
        itemDescription_Text = GameManager.Inventory.transform.GetChild(9).GetComponentInChildren<TextMeshProUGUI>();
        itemDescriptionTransform = GameManager.Inventory.transform.GetChild(9).GetComponent<RectTransform>();
        anim = GameManager.Inventory.GetComponent<Animator>();
    }

    private void IsMovingChange()
    {
        if (!IsMoving)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty && !IsMoving)
        {
            itemDescriptionTransform.position = rectTransform.position;
            SetDiscription(item);
            anim.SetBool(popUpHash, true);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        anim.SetBool(popUpHash, false);
        GameManager.SlotManager.OnSlotClicked(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool(popUpHash, false);
    }
    void SetDiscription(ItemBase item)
    {
        itemDescription_Text.text = $"{item.Name}\n{item.itemDescription}";
    }
    void UpdateAmountText(int amount)
    {
        if (amount <= 0)
        {
            amount_Text.text = "";
            return;
        }
        amount_Text.text = amount.ToString();
    }

 
}
