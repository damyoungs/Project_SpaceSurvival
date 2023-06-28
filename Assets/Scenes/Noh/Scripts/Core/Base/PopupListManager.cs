using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupListManager : MonoBehaviour
{
    LinkedList<PopupWindowBase> popupActiveList;

    private void Awake()
    {
        popupActiveList = new LinkedList<PopupWindowBase>();
    }
    public void ResetViewIndex() {
        if (popupActiveList.Count > 0) {
            //for () { 
            ////링크드리스크에서 정렬된값을 가져와서 화면에도 정렬을 시킨다.
            //}
        }
    }
}
