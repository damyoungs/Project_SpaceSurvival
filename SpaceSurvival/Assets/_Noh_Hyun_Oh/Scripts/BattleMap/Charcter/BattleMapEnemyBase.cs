using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapEnemyBase : EnemyBase_PoolObj ,ICharcterBase
{
    /// <summary>
    /// 몬스터는 컨트롤할수없으니 형식만 맞춰두자
    /// </summary>
    public bool IsControll { get; set; }

    Enemy_Psionic psionicData;
    public Enemy_Psionic PsionicData => psionicData;

    Enemy_ enemy_;
    public Enemy_ Enemy => enemy_;


    public bool IsMoveCheck { get; }
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
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// 현재 자신의 위치의 타일
    /// </summary>
    public Tile currentTile;
    public Tile CurrentTile
    {
        get 
        {
            if (currentTile == null) 
            {
                currentTile = GetCurrentTile?.Invoke();
            }
            return currentTile;
        }
    }


    public Func<Tile> GetCurrentTile { get; set ; }

    /// <summary>
    /// 행동력 혹은 이동가능 거리
    /// </summary>
    protected float moveSize = 2.0f;
    public float MoveSize => moveSize;

    protected override void Awake()
    {
        base.Awake();
        psionicData = GetComponentInChildren<Enemy_Psionic>();
        psionicData.on_Enemy_Stamina_Change += (stmValue) =>
        {
            float currentMoveSize = stmValue > moveSize? moveSize : stmValue;
            //TurnManager.Instance.CurrentTurn.TurnActionValue = stmValue;
            TurnManager.Instance.CurrentTurn.TurnActionValue = 20;
            //if(stmValue < 1.0f)
            //{
            //    TurnManager.Instance.CurrentTurn.TurnEndAction();
            //}
        };
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        InitUI();
    }

    protected override void OnEnable()
    {
        if (battleUICanvas != null)  //캔버스 위치를 찾아놨으면
        {
            InitUI();//초기화
        }
    }
    /// <summary>
    /// 추적형 UI 초기화 함수 셋팅
    /// </summary>
    public void InitUI()
    {
        if (battleUI != null) //값이 있으면
        {
            battleUI.gameObject.SetActive(true); //활성화만 시킨다
        }
        else //추적형 UI가 셋팅안되있으면 셋팅한다
        {
            battleUI = (TrackingBattleUI)Multiple_Factory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // 제일처음 초기화할때 배틀 UI 셋팅하고 
            battleUI.gameObject.name = $"{name} _ Tracking"; //이름확인용
            battleUI.transform.SetParent(battleUICanvas);//풀은 캔버스 밑에없기때문에 배틀맵UI만 관리할 캔버스 위치 밑으로 이동시킨다.
            battleUI.gameObject.SetActive(true); //활성화 시킨다.
            battleUI.Player = transform.GetChild(0);     //UI 는 유닛과 1:1 매치가 되있어야 됨으로 담아둔다.
        }
    }

    /// <summary>
    /// 셋팅전의 값으로 돌리기
    /// 값을 초기화 시키고 풀로 돌리고 큐로 돌린다.
    /// </summary>
    public virtual void ResetData()
    {
        if (BattleUI != null) //배틀 UI가 셋팅되있으면 
        {
            BattleUI.ResetData();// 추적형 UI 초기화 
            BattleUI = null; // 비우기
        }
        Debug.Log("몬스터 초기화 ");
        currentTile.ExistType = Tile.TileExistType.None; // 속성 돌리고 
        
        currentTile = null; //타일 참조해제
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }

    public void EnemyAi(Tile PlayerTile)
    {
        Debug.Log($"{transform.name}턴 시작 행동력 : {psionicData.Stamina}\n좌표{CurrentTile.transform.position}, {currentTile.name}");


        
    }



    public void CharcterMove(Tile selectedTile)
    {

    }


}
