using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enhancer_Slot_Before : Enhancer_Slot_Base, IPointerEnterHandler, IPointerMoveHandler,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        item_Enhancer.EnhancerState = EnhancerState.ClearItem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
   
    }

    public void OnPointerMove(PointerEventData eventData)
    {
      
    }
}
