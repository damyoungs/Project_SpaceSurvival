using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolBar : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    Vector2 distance;
    Vector2 positionBeforeDrag;
    RectTransform canvasPosition;
    RectTransform Inventory;
    float offsetX = 630.0f;
    float offsetY = 130.0f;
    private void Awake()
    {
        canvasPosition = transform.parent.parent.GetComponent<RectTransform>();
        Inventory = transform.parent.GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        distance = Input.mousePosition - Inventory.position;
        positionBeforeDrag = Inventory.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Inventory.position = eventData.position - distance;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (Inventory.localPosition.x > offsetX || Inventory.localPosition.x < -offsetX || Inventory.localPosition.y > offsetY || Inventory.localPosition.y < -offsetY)
        {
            Inventory.position = positionBeforeDrag;
        }
    }

}
