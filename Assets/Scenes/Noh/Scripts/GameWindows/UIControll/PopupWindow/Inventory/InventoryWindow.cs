using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryWindow : PopupWindowBase 
{
    /// <summary>
    /// ���������� �� âũ�� �ּҰ� 
    /// </summary>
    //float minSizeWidth = 200.0f;
    
    /// <summary>
    /// ���������� �� âũ�� �ּҰ� 
    /// </summary>
    //float minSizeHeight = 200.0f;

    GameObject contentsObj;

    GameObject contentObj;
    protected override void Awake()
    {
        base.Awake();
        contentsObj = transform.GetChild(0).GetChild(0).gameObject; //������ ��ġã��
        contentObj = contentsObj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        //ȭ�� �ּ� �ִ� ������ ����
        minHeight = 600.0f;
        minWidth = 400.0f;
        maxHeight = 1000.0f;
        maxWidth = 800.0f;

        SetItemList(new Vector2(minWidth, minHeight));
    }
  
    protected override void SetItemList(Vector2 contentWindowSize)
    {

        int childCount = contentObj.transform.childCount; //�����ۼ����Ұ����ִ��� ī��Ʈ
        if (childCount == 0) return;
        Vector2 v2 = contentObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        int a = (int)(rectTransform.sizeDelta.x / v2.x);
        v2.x *= a;
        v2.y *= ((childCount / a )+1);

        contentObj.GetComponent<RectTransform>().sizeDelta = v2;
       //�ڵ��׸���� �ڵ����̾ƿ����� ó���ϰ� ���۳�Ʈ âũ�⸸ �������� ����¥���� ��ġ������ ��Ʈ ������Ʈ������ŭ �����ͼ� ó���ؾߵǴ�.



    }

    
}
