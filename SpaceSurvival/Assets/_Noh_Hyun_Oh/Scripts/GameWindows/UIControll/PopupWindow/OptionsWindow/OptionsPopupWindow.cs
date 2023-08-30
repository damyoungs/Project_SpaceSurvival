using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsPopupWindow : PopupWindowBase, IPopupSortWindow, IPointerClickHandler
{
    Action<IPopupSortWindow> popupEventHandle;
    public Action<IPopupSortWindow> PopupSorting { set => popupEventHandle += value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        popupEventHandle(this);
    }
}
