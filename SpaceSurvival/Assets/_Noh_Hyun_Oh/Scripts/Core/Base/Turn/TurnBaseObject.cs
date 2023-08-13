using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// 기본 베이스는 ITurnBaseData 인터페이스
/// 턴을 사용할 유닛들의 기본 틀.
/// 배틀맵에서 턴을 사용하는 유닛들(PC,NPC)을  팀으로 구분해서 관리한다.
/// 
/// SetParent 사용시 주의사항 
///     - 이동될 객체가 활성화 상태에서 이동될 transform을 기준으로 상위객체들이 비활성화 되있는경우 자동으로 OnDisable 이 호출된다 .
///     - 활성화 객체 -> 비활성화 부모  반대로 비활성화 객체 -> 활성화 부모 각각 상태변화가 자동으로 이루어져 OnEnable과 OnDisable 함수가 호출된다.
/// </summary>
public class TurnBaseObject : BattleMapTurnUnit_PoolObj, ITurnBaseData
{
    /// <summary>
    /// 맵에 생성된 턴을 사용할 오브젝트의 인덱스  매니저에서 해당 오브젝트 생성할때 인덱스를 줄것이다.
    /// </summary>
    [SerializeField]
    protected int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }


    /// <summary>
    /// 턴이경과시 회복될 행동력값 
    /// </summary>
    [SerializeField]
    protected float turnAddValue = 0.50f;
    public float TurnEndActionValue => turnAddValue;

    /// <summary>
    /// 행동력 최대치 (회복상한선)
    /// </summary>
    [SerializeField]
    protected float maxTurnValue = 1.5f;

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
    /// 턴종료시 실행할 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set ; }


    /// <summary>
    /// 배틀맵에서 특정 적그룹 의 관리할 유닛수가 없다면 턴 진행도 안되야함으로 처리할 로직 연결 
    /// </summary>
    public Action<ITurnBaseData> TurnRemove { get; set; }


    /// <summary>
    /// 현재 해당 턴이 관리할 유닛들
    /// </summary>
    protected List<ICharcterBase> charcterList;
    public List<ICharcterBase> CharcterList => charcterList;

    /// <summary>
    /// 턴게이지 UI 
    /// </summary>
    TurnGaugeUnit gaugeUnit = null;
    public TurnGaugeUnit GaugeUnit => gaugeUnit;
   
    /// <summary>
    /// 턴 UI가 담길 캔버스 위치
    /// </summary>
    Transform turnGaugeCanvas;

    
    protected override void Awake()
    {
        base.Awake();
        turnGaugeCanvas = FindObjectOfType<WindowList>().transform.
                            GetChild(0).
                            GetChild(WindowList.Instance.transform.GetChild(0).childCount - 1).
                            GetChild(0); // 턴 게이지 담을 캔버스위치
        charcterList = new(); //리스트 초기화 
    }
    /// <summary>
    /// 활성화시 턴게이지 셋팅하고 활성화 
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        //생성타임이 안맞아서 체크하는 수밖에..
        if (gaugeUnit != null) //값셋팅 되있으면 
        {
            gaugeUnit.gameObject.SetActive(true); //키고.
        }
        else if (turnGaugeCanvas != null)// 캔버스 셋팅 혹은 게이지 되있으면  
        {
            TurnGaugeInit();//게이지 셋팅
        }
    }

    /// <summary>
    /// 셋팅전의 값으로 돌리기
    /// 값을 초기화 시키고 풀로 돌리고 큐로 돌린다.
    /// 리셋은 OnDisable 함수안에 실행시키면 안된다 . SetParent 가 에러를 발생할것이다.
    /// </summary>
    public virtual void ResetData()
    {
        name = "";
        gaugeUnit.ResetData();// 턴 게이지 UI  초기화 
        gaugeUnit = null; // 턴 게이지 UI 비우기
        TurnEndAction = null; //턴종료 액션도 비우고
        TurnRemove = null; //해당턴유닛에서 삭제되는 유닛들도 비우자.
        UnitBattleIndex = -1;//인덱스 초기화
        foreach (ICharcterBase charcter in charcterList)//캐릭터 셋팅되있으면 
        {
            charcter.ResetData(); //데이터 초기화 실행시키고 
        }
        charcterList.Clear(); //팀인원 초기화 
        gameObject.SetActive(false); // 큐를 돌린다.
    }

    /// <summary>
    /// 턴 시작시 실행할 함수 base에있는건 테스트용이니 오버로드 할때 안써도된다.
    /// </summary>
    public virtual void TurnStartAction()
    {
        /*
         해당 팀이 턴시작시 처리할 로직 화면에 UI보여준다든가 카메라 이동시킨다든가  등등
         
         */
        if(TurnActionValue > 1.0f)TurnActionValue -= UnityEngine.Random.Range(0.3f, 1.0f);// 행동력 소모후 테스트 용 
    }



    /// <summary>
    /// 사용안한다고했지만 기능만들어놔서 일단 유지는 시킨다. 
    /// UI하단 턴게이지에 사용될 UI 오브젝트 셋팅하기
    /// </summary>
    private void TurnGaugeInit()
    {
        GameObject obj = MultipleObjectsFactory.Instance.
           GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// 풀에서 데이터 가져온다.
        obj.gameObject.name = $"{this.name} _ TurnGauge "; //오브젝트에 이름 넣어둔다 제대로됬는지 확인하기쉽게.
        obj.transform.SetParent(turnGaugeCanvas); //부모위치 설정 
        obj.gameObject.SetActive(true); //활성화 시킨다.
        gaugeUnit = obj.GetComponent<TurnGaugeUnit>(); //턴 유닛에 게이지 유닛 캐싱 
        gaugeUnit.ProgressValue = TurnActionValue; //초기값 셋팅
    }
}
