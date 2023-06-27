using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupWindowSizeButton : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    ///�˾� ���� �� �θ� �����찡������
    /// </summary>
    PopupWindowBase parentPopupWindow;

    private void Awake()
    {
        parentPopupWindow = transform.parent.parent.GetComponent<PopupWindowBase>();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentPopupWindow.ClickPosition = eventData.position; //�θ��������� ���� ����ó�����־ ���� �Ѱ��.
        parentPopupWindow.IsWindowSizeChange = true; // �̰͵��������� ������������ �Ǵ°�츸 �ϱ����� üũ
        
    }

  
}
   
