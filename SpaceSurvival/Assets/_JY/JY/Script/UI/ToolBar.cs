using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolBar : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    Vector2 distance;
    Vector2 positionBeforeDrag;
    RectTransform Inventory;
    float offsetX = 630.0f;
    float offsetY = 130.0f;
    private void Awake()
    {
        Inventory = transform.parent.GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        distance = Input.mousePosition - Inventory.position; //클릭하는순간의 eventPosition(마우스위치)와 인벤토리의 피봇간 거리 저장
        positionBeforeDrag = Inventory.position; //클릭하는순간의 Inventory 위치 저장
    }
    public void OnDrag(PointerEventData eventData) //
    {
        Inventory.position = eventData.position - distance; //이동
    }


    public void OnPointerUp(PointerEventData eventData)//로컬기준 범위밖으로 나갈시 처음 클릭했던 곳으로 이동
    {
        if (Inventory.localPosition.x > offsetX || Inventory.localPosition.x < -offsetX || Inventory.localPosition.y > offsetY || Inventory.localPosition.y < -offsetY)
        {
            Inventory.position = positionBeforeDrag;
        }
    }

}
