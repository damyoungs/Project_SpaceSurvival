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
        distance = Input.mousePosition - Inventory.position; //Ŭ���ϴ¼����� eventPosition(���콺��ġ)�� �κ��丮�� �Ǻ��� �Ÿ� ����
        positionBeforeDrag = Inventory.position; //Ŭ���ϴ¼����� Inventory ��ġ ����
    }
    public void OnDrag(PointerEventData eventData) //
    {
        Inventory.position = eventData.position - distance; //�̵�
    }


    public void OnPointerUp(PointerEventData eventData)//���ñ��� ���������� ������ ó�� Ŭ���ߴ� ������ �̵�
    {
        if (Inventory.localPosition.x > offsetX || Inventory.localPosition.x < -offsetX || Inventory.localPosition.y > offsetY || Inventory.localPosition.y < -offsetY)
        {
            Inventory.position = positionBeforeDrag;
        }
    }

}
