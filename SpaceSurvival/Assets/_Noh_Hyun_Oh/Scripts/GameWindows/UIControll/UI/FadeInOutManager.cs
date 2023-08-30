using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 씬 페이드인 페이드 아웃 기능 담긴 컴퍼넌트
/// </summary>
public class FadeInOutManager  :TestBase
{
    /// <summary>
    /// 화면 가로크기 구해두기
    /// </summary>
    readonly float screenWidth = Screen.width;
    /// <summary>
    /// 화면 세로크기 구해두기
    /// </summary>
    readonly float screenHeight = Screen.height;

    /// <summary>
    /// UI 이동 속도
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5.0f;

    /// <summary>
    /// 이동할 위치임시로 저장할 백터
    /// </summary>
    private Vector2 tempMin;
    private Vector2 tempMax;

    /// <summary>
    /// 진행도 
    /// </summary>
    private float timeElapsed = 0.0f;

    private bool isMove = false;

    RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        rt = GetComponent<RectTransform>();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        TurnStartView(rt, 0);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        TurnEndView(rt, screenWidth);
    }
    public void TurnStartView(RectTransform rt , float arriveValue = 0.0f) 
    {
        StopAllCoroutines();
        StartCoroutine(MoveToLeftFromRight(rt, arriveValue));
    }

    public void TurnEndView(RectTransform rt , float arriveValue = 0.0f) 
    {
        StopAllCoroutines();
        StartCoroutine( MoveToRightFromLeft(rt, arriveValue)); 
    }
    /// <summary>
    /// 윈도우 창 크기만큼 왼쪽에서 오른쪽으로 움직이게 하는로직
    /// </summary>
    /// <param name="arriveValue">움직일 수치값 값입력없으면 기존위치로 돌아간다. </param>
    IEnumerator MoveToLeftFromRight(RectTransform rt , float arriveValue = 0.0f) 
    {
        isMove = true;
        arriveValue = -arriveValue; //도착위치값은 오른쪽이면 -로 셋팅

        timeElapsed = 0.0f; //시간누적용 변수값 초기화
        while (true)
        {
            timeElapsed += Time.deltaTime * moveSpeed; // 속도만큼 시간누적
            
            SetLeftRightValue(rt,
                         rt.offsetMin.x - timeElapsed //왼쪽에서 오른쪽으로 가기위한 계산값
                        );
            yield return null;
            if (rt.offsetMin.x <= arriveValue) //도착지점
            {
                SetLeftRightValue(rt, arriveValue); //속도가빨라지면 벗어남으로 도착지점으로 마무리
                break;
            }
        }
        isMove = false;
    }
    /// <summary>
    /// 윈도우 창 크기만큼 오른쪽에서 왼쪽으로 움직이게하는로직
    /// </summary>
    /// <param name="arriveValue">움직일 수치값 값입력없으면 기존위치로 돌아간다. </param>
    IEnumerator MoveToRightFromLeft(RectTransform rt,  float arriveValue = 0.0f)
    {

        timeElapsed = 0.0f;
        while (true)
        {
            timeElapsed += Time.deltaTime * moveSpeed;
            SetLeftRightValue(rt,
                rt.offsetMin.x + timeElapsed //오른쪽에서 왼쪽으로 가기위한 계산값
                );
            yield return null;
            if (rt.offsetMin.x >= arriveValue) //도착지점
            {
                SetLeftRightValue(rt, arriveValue); //속도가빠르면 벗어남으로 위치값 고정
                break;
            }
        }
    }

    /// <summary>
    /// UI 크기를 유지한채 
    /// 왼쪽, 오른쪽으로 이동시키는 함수 
    /// </summary>
    /// <param name="value">이동할 위치값</param>
    ///<param name="rt">이동할 오브젝트</param>
    private void SetLeftRightValue(RectTransform rt , float value)
    {
        tempMin = rt.offsetMin; //Left
        tempMax = rt.offsetMax; //Right
        tempMin.x = value; //Left 값은 양수값 그대로 들어간다
        tempMax.x = value; //Right 값은 양수값 넣으면 음수값으로 변환되어 들어간다.
        rt.offsetMin = tempMin; //Left
        rt.offsetMax = tempMax; //Right

    }

    /// <summary>
    ///  UI 크기를 유지한채 위아래로 이동시키는 함수
    ///<param name="value">위치값</param>
    ///<param name="rt">이동할 오브젝트</param>
    /// </summary>
    private void SetTopBottomValue(RectTransform rt, float value)
    {
        tempMin = rt.offsetMin; //bottom
        tempMax = rt.offsetMax; //top
        tempMin.y = value; //bottom 값은 양수값 그대로 들어간다
        tempMax.y = value; //top 값은 양수값 넣으면 음수값으로 변환되어 들어간다.
        rt.offsetMin = tempMin; //bottom
        rt.offsetMax = tempMax; //top
    }

}
