using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 풀에서 생성될때 자동으로 데이터까지 초기화하도록 고쳤다.
/// </summary>
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
            realIndex = value - 1;
            text.text = $"{pageIndex}";
        }
    }
    /// <summary>
    /// 페이지버튼 누를때마다 -1연산이 필요해서 기냥 변수로뺏다.
    /// </summary>
    int realIndex = -1; 
    
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
        if (realIndex > -1)
        {
            proccessClass.SetPageList(realIndex); 
        }
    }
}
