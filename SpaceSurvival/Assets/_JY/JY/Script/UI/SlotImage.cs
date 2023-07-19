using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotImage : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler
{
    RectTransform inventoryRectTransform;
    Vector2 localMousePosition;
    bool firstClick = false;
    private void Start()
    {
        inventoryRectTransform = GameManager.Inventory.GetComponent<RectTransform>();
    }
    void SwitchOnFirstClick()
    {
        firstClick = !firstClick;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchOnFirstClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     
    }
}
