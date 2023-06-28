using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOPopupWindow : MonoBehaviour
{
    private void OnEnable()
    {
        WindowList.Instance.ActivePopup = EnumList.PopupList.SAVELOADPOPUP;
    }
    private void OnDisable()
    {
        WindowList.Instance.ActivePopup = EnumList.PopupList.NONE;
        SaveLoadPopupWindow.Instance.NewIndex = -1; //값 초기화
        SaveLoadPopupWindow.Instance.OldIndex = -1; //카피 값 초기화
        SaveLoadPopupWindow.Instance.CopyCheck = false; //카피 값 초기화

    }
}
