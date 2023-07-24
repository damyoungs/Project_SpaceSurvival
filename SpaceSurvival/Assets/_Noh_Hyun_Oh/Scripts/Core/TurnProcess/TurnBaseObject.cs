using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// 기본 베이스는 ITurnBaseData 인터페이스
/// 턴을 사용할 유닛들의 기본 틀.
/// 배틀맵에서 턴을 사용하는 유닛들(PC,NPC)은 ITurnBaseData  인터페이스를 추가한 Base 클래스 만들고 그것을 상속받고 구현해줘야한다 .
/// 아니면 해당클래스를 상속받고 구현해주면된다 - 해당클래스상속시 ITurnBaseData 는 추가상속 해줘야한다.  해당클래스는 사용할수있도록 정리중이다.
/// 
/// SetParent 사용시 주의사항 
///     - 이동될 객체가 활성화 상태에서 이동될 transform을 기준으로 상위객체들이 비활성화 되있는경우 자동으로 OnDisable 이 호출된다 .
///     - 활성화 객체 -> 비활성화 부모  반대로 비활성화 객체 -> 활성화 부모 각각 싱태변화가 자동으로 이루어져 OnEnable과 OnDisable 함수가 호출된다.
/// </summary>
public class TurnBaseObject : BattleMapUnitIsPool, ITurnBaseData
{
    /// <summary>
    /// 배틀유닛이 유니크한 키값으로 설정하려고한다. - 미셋팅
    /// </summary>
    [SerializeField]
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }

    /// <summary>
    /// 턴이경과시 회복될 행동력값 
    /// </summary>
    [SerializeField]
    float turnAddValue = 0.50f;
    public float TurnAddValue 
    {
        get => turnAddValue;
        set 
        {
            turnAddValue = value;
        }
    }

    [SerializeField]
    /// <summary>
    /// 행동력 최대치 (회복상한선)
    /// </summary>
    float maxTurnValue = 1.5f;
    public float MaxTurnValue 
    {
        get => maxTurnValue;
        protected set 
        {
            maxTurnValue = value;
        }
    }

    /// <summary>
    /// 현재 턴의 진행값 
    /// 이값으로 돌아올순번을 정한다.
    /// </summary>
    float turnWaitingValue = 1.0f;
    public float TurnActionValue
    {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
            if (turnWaitingValue < 0.0f) // 값의 최소값을 정해주고
            {
                turnWaitingValue = 0.0f; 
            } else if (turnWaitingValue > maxTurnValue) //최대로 들어올수있는값도 정해주자  
            {
                turnWaitingValue = maxTurnValue;
            }
            if (gaugeUnit != null)  // 턴게이지가 셋팅되있으면 
            {
                gaugeUnit.ProgressValue = value; //턴게이지값도 수정 

            }
        }
    }

  
    /// <summary>
    /// 턴경과시 회복될 행동력값
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    Action<ITurnBaseData> turnEndAction;
    /// <summary>
    /// 턴종료시 실행할 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get=> turnEndAction; set =>  turnEndAction = value; }

    /// <summary>
    /// 턴진행시 적유닛을 제거하거나 할때 호출될 함수
    /// </summary>
    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }

    [SerializeField]
    /// <summary>
    /// 추적형 UI 
    /// </summary>
    private TrackingBattleUI battleUI = null;
    public TrackingBattleUI BattleUI
    {
        get => battleUI;
        set => battleUI = value;

    }

    /// <summary>
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform battleUICanvas;
    [SerializeField]
    /// <summary>
    /// 턴게이지 UI 
    /// </summary>
    private GaugeUnit gaugeUnit = null;
    public GaugeUnit GaugeUnit 
    {
        get => gaugeUnit;
        set 
        {
            gaugeUnit = value;
        }
    }
    /// <summary>
    /// 턴 UI가 담길 캔버스 위치
    /// </summary>
    Transform turnGaugeCanvas;

    private void Awake()
    {
        Transform windowList = FindObjectOfType<WindowList>().transform; //싱글톤이 생성안되있는상태라 검색해서 가져온다.
        battleUICanvas = windowList.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        turnGaugeCanvas = windowList.GetChild(0).GetChild(1).GetChild(0);// 턴 게이지 담을 캔버스위치
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitUI(); 
    }

    /// <summary>
    /// 추적형 UI 초기화 함수 셋팅
    /// </summary>
    public void InitUI()
    {
        if (battleIndex < 0) return; //인덱스는 배틀맵에서 잡기때문에 인덱스 셋팅되면 UI셋팅한다.
        if (BattleUI == null) //추적형 UI가 셋팅안되있으면 
        {//셋팅 
            GameObject obj = MultipleObjectsFactory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // 제일처음 초기화할때 배틀 UI 셋팅하고 
            obj.gameObject.name = $"{this.name} _ Tracking _ {battleIndex}"; //이름확인용
            obj.transform.SetParent(battleUICanvas);//풀은 캔버스 밑에없기때문에 배틀맵UI만 관리할 캔버스 위치 밑으로 이동시킨다.
            obj.gameObject.SetActive(true); //활성화 시킨다.
            BattleUI = obj.GetComponent<TrackingBattleUI>(); //UI 컴퍼넌트 찾아온다.
            BattleUI.Player = this ;     //UI 는 유닛과 1:1 매치가 되있어야 됨으로 담아둔다.
        }
        if (GaugeUnit == null) //턴 게이지UI 가 셋팅 안되있으면
        {//셋팅
            GameObject obj = MultipleObjectsFactory.Instance.
               GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// 풀에서 데이터 가져온다.
            obj.gameObject.name = $"{this.name} _ TurnGauge _{battleIndex}"; //이름확인용
            obj.transform.SetParent(turnGaugeCanvas); //부모위치 설정 
            obj.gameObject.SetActive(true); //활성화 시킨다.
            GaugeUnit = obj.GetComponent<GaugeUnit>(); //턴 유닛에 게이지 유닛 캐싱 
            GaugeUnit.ProgressValue = TurnActionValue; //초기값 셋팅
        }
    }

    /// <summary>
    /// 셋팅전의 값으로 돌리기
    /// 값을 초기화 시키고 풀로 돌리고 큐로 돌린다.
    /// </summary>
    public void ResetData()
    {
        //배틀 추적 UI는 팀별로 나눌경우도 추가가 필요 - 기능 제대로돌아가면 추가
        if (BattleUI != null) 
        {
            BattleUI.ResetData();// 추적형 UI 초기화 
            BattleUI = null; // 비우기
        }
        if (GaugeUnit != null) 
        {
            GaugeUnit.ResetData();// 턴 게이지 UI  초기화 
            GaugeUnit = null; // 비우기
        }
        turnEndAction = null;
        turnRemove = null;
        //턴 오브젝트 초기화
        transform.SetParent(PoolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
        UnitBattleIndex = -1;//인덱스 초기화
    }

    /// <summary>
    /// 턴 시작시 실행할 함수 
    /// </summary>
    public void TurnStartAction()
    {
        /*
         여기에 기능구현 PC면 액션 아이콘을 활성화하여 이동 ,공격 , 스킬 등 액션을 취할수있게 바꾸고 
         NPC 면 여기에 자동로직을 구성하여 마지막에 TrunEndAction?.Invoke(this); 를 실행하여 턴메니져로 제어권을 넘긴다.
         
         */
        if(TurnActionValue > 1.0f)TurnActionValue -= UnityEngine.Random.Range(0.3f, 1.0f);// 행동력 소모후 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
