using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePageButtonIsPool : ObjectIsPool
{
    /// <summary>
    /// 화면에 보여줄 페이징번호 
    /// </summary>
    int pageIndex = -1;
    public int PageIndex { 
        get => pageIndex;
        set { 
            pageIndex = value;
            text.text = $"{pageIndex}";
        }
    }
    /// <summary>
    /// 처리할 클래스 가져오기
    /// </summary>
    SaveDataSort proccessClass;

    /// <summary>
    /// 화면에 보여줄 텍스트위치 가져오기
    /// </summary>
    TextMeshProUGUI text;

    /// <summary>
    /// 오브젝트찾기
    /// </summary>
    private void Awake()
    {
        isPositionReset = false; //활성화시 로컬포지션 로테이션 초기화를하지않는다.
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        proccessClass = transform.parent.parent.parent.GetComponent<SaveDataSort>();
    }

    /// <summary>
    /// 페이지 버튼 클릭 이벤트
    /// </summary>
    public void OnPageDownButton()
    {
        if (pageIndex > -1)
        {
            proccessClass.SetPageList(pageIndex-1); //화면에는 1부터 시작이기때문에 배열처리하기위해 -1
        }
    }
}
