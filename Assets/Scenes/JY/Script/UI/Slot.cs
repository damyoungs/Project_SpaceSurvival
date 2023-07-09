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
    int itemCount;

    TextMeshProUGUI itemDescription_Text;
    Animator anim;

    private ItemBase item;
    public ItemBase Item//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
    {
        get => item;
        set
        {
            item = value;
            SetDiscription(value);
        }
    }
    public bool IsEmpty { get; set; } = true;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티
    
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
        itemDescription_Text = GetComponentInChildren<TextMeshProUGUI>();
        amount_Text = transform.GetChild(1).GetComponent<Text>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        UpdateAmountText(0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            anim.SetBool("PopUp", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("PopUp", false);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.SlotManager.OnSlotClicked(this);
    }
}
