using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    Text item_Discription;
    Text amount_Text;
    Image item_Image;
    int itemCount;
    public bool IsEmpty { get; set; } = true;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티
    
    public ItemBase CurrentItem { get;  set; }
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
        item_Discription = transform.GetChild(2).GetComponent<Text>();
        amount_Text = transform.GetChild(1).GetComponent<Text>();
        item_Image = transform.GetChild(0).GetComponent<Image>();
    }
    private void Start()
    {
        UpdateAmountText(0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        ItemCount += 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");

    }
    void SetDiscription()
    {
        item_Discription.text = "맛있어 보인다!";
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
