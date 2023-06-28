using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupWindowSizeButton : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    ///팝업 조절 할 부모 윈도우가져오기
    /// </summary>
    PopupWindowBase parentPopupWindow;

    private void Awake()
    {
        parentPopupWindow = transform.parent.parent.GetComponent<PopupWindowBase>();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentPopupWindow.ClickPosition = eventData.position; //부모윈도우의 값에 연산처리가있어서 값을 넘겼다.
        parentPopupWindow.IsWindowSizeChange = true; // 이것도마찬가지 사이즈조절이 되는경우만 하기위해 체크
        
    }

  
}
   
