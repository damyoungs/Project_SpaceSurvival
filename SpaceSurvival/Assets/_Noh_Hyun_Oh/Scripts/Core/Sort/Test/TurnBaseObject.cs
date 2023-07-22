using System;
using UnityEngine;
/// <summary>
/// 기본 베이스는 ITurnBaseData 인터페이스
/// 턴을 사용할 유닛들의 기본 틀.
/// 배틀맵에서 턴을 사용하는 유닛들(PC,NPC)은 ITurnBaseData  인터페이스를 추가한 Base 클래스 만들고 그것을 상속받고 구현해줘야한다 .
/// 아니면 해당클래스를 상속받고 구현해주면된다 - 해당클래스상속시 ITurnBaseData 는 추가상속 해줘야한다.  해당클래스는 사용할수있도록 정리중이다.
/// </summary>
public class TurnBaseObject : BattleMapUnitIsPool, ITurnBaseData
{
    [SerializeField]
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }
    /// <summary>
    /// 턴이경과시 회복될 행동력값 
    /// </summary>
    float turnAddValue = 0.50f;
    public float TurnAddValue 
    {
        get => turnAddValue;
        protected set 
        {
            turnAddValue = value;
        }
    }
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
    public float TurnWaitingValue {
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
        }
    }

    /// <summary>
    /// 정렬할 기준값 프로퍼티
    /// </summary>
    public float TurnActionValue { 
            get => TurnWaitingValue; 
            set => TurnWaitingValue = value; 
    }

    /// <summary>
    /// 턴경과시 회복될 행동력값
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    /// <summary>
    /// 턴종료시 실행할 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    /// <summary>
    /// 턴진행시 적유닛을 제거하거나 할때 호출될 함수
    /// </summary>
    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }
    
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

    private void Awake()
    {
        //테스트
        turnAddValue = UnityEngine.Random.Range(0.0f ,0.1f); //턴진행시마다 증가되는 행동력값 랜덤 설정 -테스트
        TurnWaitingValue = UnityEngine.Random.Range(0.0f,8.0f); // -테스트값 설정


        //찾아오기
        battleUICanvas = FindObjectOfType<WindowList>().transform.GetChild(0).GetChild(0); // TrackingUI 담을 캔버스위치

    }

    /// <summary>
    /// awake 에서는 팩토리가 생성안될수도있어서 Start 에서 초기화가 가능하다.
    /// </summary>
    private void Start()
    {
        InitTrackingUI();//추적형 초기화 
    }
    protected override  void OnEnable()
    {
        base.OnEnable();
        if (BattleUI != null) //풀에서 항상 Visible 온 오프 하기때문에 체크가 필요하다 . 초기화타이밍인 Start 보다 빨리 발생하기때문에 필요한 널체크
        {
            InitTrackingUI();
        }
    }
    /// <summary>
    /// 추적형 UI 초기화 함수 셋팅
    /// </summary>
    public void InitTrackingUI()
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // 제일처음 초기화할때 배틀 UI 셋팅하고 
        BattleUI = obj.GetComponent<TrackingBattleUI>(); //UI 컴퍼넌트 찾아온다.
        BattleUI.gameObject.name = $"{this.name} _ {battleIndex}"; //인덱스는 항상바뀌기때문에 작성
        BattleUI.Player = this ;     //UI 는 유닛과 1:1 매치가 되있어야 됨으로 담아둔다.
        BattleUI.transform.SetParent(battleUICanvas);//풀은 캔버스 밑에없기때문에 배틀맵UI만 관리할 캔버스 위치 밑으로 이동시킨다.
        BattleUI.gameObject.SetActive(true); //활성화 시킨다.

    }
 
    /// <summary>
    /// 셋팅전의 값으로 돌리기
    /// 값을 초기화 시키고 풀로 돌리고 큐로 돌린다.
    /// </summary>
    public void InitValue() 
    {
        BattleUI.InitValue(); //배틀 UI초기화 시킨다.
        transform.SetParent(PoolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
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
        if(TurnActionValue > 1.0f)TurnWaitingValue -= UnityEngine.Random.Range(0.3f, 1.0f);// 행동력 소모후 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
