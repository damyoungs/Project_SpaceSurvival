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
            ////��ũ�帮��ũ���� ���ĵȰ��� �����ͼ� ȭ�鿡�� ������ ��Ų��.
            //}
        }
    }
}
