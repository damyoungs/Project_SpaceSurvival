using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryWindow : PopupWindowBase 
{
    /// <summary>
    /// 아이템정렬 시 창크기 최소값 
    /// </summary>
    float minSizeWidth = 200.0f;
    
    /// <summary> 
    /// 아이템정렬 시 창크기 최소값 
    /// </summary>
    float minSizeHeight = 200.0f;

    GameObject contentsObj;

    GameObject contentObj;
    protected override void Awake()
    {
        base.Awake();
        contentsObj = transform.GetChild(0).GetChild(0).gameObject; //컨텐츠 위치찾기
        contentObj = contentsObj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        //화면 최소 최대 사이즈 셋팅
        minHeight = 600.0f;
        minWidth = 400.0f;
        maxHeight = 1000.0f;
        maxWidth = 800.0f;


        SetItemList(new Vector2(minWidth, minHeight));
    }
    float padding = 10.0f;
    protected override void SetItemList(Vector2 contentWindowSize)
    {

        int childCount = contentObj.transform.childCount; //아이템셋팅할것이있는지 카운트
        if (childCount == 0) return;
        Vector2 v2 = contentObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        //창크기 조절 대충..
        int a = (int)(rectTransform.sizeDelta.x / (v2.x + padding));
        v2.x = rectTransform.sizeDelta.x;
        v2.y = (v2.y+ padding) * ((childCount / a )+1);
        
        contentObj.GetComponent<RectTransform>().sizeDelta = v2;
       //자동그리기는 자동레이아웃으로 처리하고 컴퍼넌트 창크기만 조절하자 로직짜려면 골치아프다 렉트 오브젝트갯수만큼 가져와서 처리해야되니.



    }

    
}
