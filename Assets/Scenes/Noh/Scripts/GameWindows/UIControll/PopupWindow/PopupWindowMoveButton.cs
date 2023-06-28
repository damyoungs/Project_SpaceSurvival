using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 팝업창 이동과 화면밖으로 벗어나지 않게 처리하는 부분
/// </summary>
public class PopupWindowMoveButton : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    /// <summary>
    /// 화면 넓이 가져오기
    /// </summary>
    float windowWidth = Screen.width;

    /// <summary>
    /// 화면 높이 가져오기
    /// </summary>
    float windowHeight = Screen.height;

    /// <summary>
    /// 이동할 윈도우창 위치의 렉트트랜스폼을 가져온다
    /// </summary>
    private RectTransform parentWindow;

    /// <summary>
    /// 드래그시작위치를담을 백터
    /// </summary>
    private Vector2 startPosition;
    /// <summary>
    /// 드래그시작시 이벤트위치값을 담을 백터
    /// </summary>
    private Vector2 movePosition;

    private Vector2 moveOffSet;
    private void Awake()
    {
        parentWindow = transform.parent.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 드래그시작할때 한번만발동 
    /// </summary>
    /// <param name="eventData">이벤트위치값정보</param>
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)//인터페이스두개를사용하기때문에 명시적으로 작성
    {
            startPosition = parentWindow.anchoredPosition;//드래그시작할때 위치값저장
            movePosition = eventData.position; //드래그시작할때의 이동처리할 포지션값 저장
    }

    /// <summary>
    /// 드래그 진행중 일때 화면밖으로 벗어났는지 체크하고 안벗어났으면 이동시킨다
    /// </summary>
    /// <param name="eventData">이벤트위치정보값</param>
    void IDragHandler.OnDrag(PointerEventData eventData) //인터페이스두개를사용하기때문에 명시적으로 작성
    {
        
        
        //이동한 드래그만큼 값을 더해준다.
        //시작지점 에서 이동한거리(이동한값에서 처음값을뺀값)을 더한다.
        if (CheckOutOfWindow(eventData)) { //창밖으로 벗어나지 않았는지 체크 
            parentWindow.anchoredPosition = moveOffSet;  //벗어나지 않았으면 이동시킨다.
        }
    }

    /// <summary>
    /// 현재화면에서 벗어나지 않았는지 체크한다
    /// </summary>
    /// <returns>화면에서 벗어났으면 false</returns>
    bool CheckOutOfWindow(PointerEventData eventData)
    {

        moveOffSet = startPosition + (eventData.position - movePosition);
        Debug.Log($"moveOffSet{moveOffSet}");
        //왼쪽과 위 체크
        if (moveOffSet.x < 0 || moveOffSet.y > 0) { 
            return false;
        }
        

        //Horizontal
        //오른쪽 체크
        float x = moveOffSet.x + parentWindow.rect.width; //오른쪽으로벗어나는것을체크하기위해 창크기랑좌표랑 합친다
        if (x > windowWidth) { //합친값이 창보다 크면 벗어낫으니 아웃!
            //moveOffSet.x = windowWidth;
            return false; 
        }



        //아래체크
        float y = moveOffSet.y - parentWindow.rect.height; // 아래방향이기때문에 더하는게아니라 빼준다. moveOffSet.y는 항상 음수이다.
        if (-y > windowHeight) {//아래방향이기때문에 연산값에 -를해서 양수로 바꿔준다.
            //moveOffSet.y = windowHeight;
            return false;
        }

        return true;
    }
}