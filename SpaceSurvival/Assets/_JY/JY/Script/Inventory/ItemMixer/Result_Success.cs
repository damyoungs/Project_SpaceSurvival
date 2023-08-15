using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Result_Success : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    public Action<ItemData> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(GameManager.Mixer.ResultSlot.ItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }

}
