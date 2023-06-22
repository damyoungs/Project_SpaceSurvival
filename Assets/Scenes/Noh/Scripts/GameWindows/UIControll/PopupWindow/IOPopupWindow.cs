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
        SaveLoadPopupWindow.Instance.NewIndex = -1; //�� �ʱ�ȭ
        SaveLoadPopupWindow.Instance.OldIndex = -1; //ī�� �� �ʱ�ȭ
        SaveLoadPopupWindow.Instance.CopyCheck = false; //ī�� �� �ʱ�ȭ

    }
}
