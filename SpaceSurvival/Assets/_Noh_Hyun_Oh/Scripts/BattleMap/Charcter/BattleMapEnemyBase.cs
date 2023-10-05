using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapEnemyBase : Base_PoolObj ,ICharcterBase 
{
    
    
    
    /// <summary>
    /// 몬스터는 컨트롤할수없으니 형식만 맞춰두자
    /// </summary>
    public bool IsControll { get; set; }


    Enemy_ enemyData;
    public Enemy_ EnemyData => enemyData;

    public virtual bool IsMoveCheck { get; }

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

    public Action<BattleMapEnemyBase> onDie;

    /// <summary>
    /// 행동력 혹은 이동가능 거리
    /// </summary>
    [SerializeField]
    protected float moveSize = 4.0f;
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    /// <summary>
    /// 행동끝났으면 신호보낼 델리게이트
    /// </summary>
    public Action onActionEndCheck;

    protected override void Awake()
    {
        base.Awake();

        enemyData = GetComponentInChildren<Enemy_>();
        enemyData.on_Enemy_Stamina_Change += (stmValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.stmGaugeSetting(stmValue, enemyData.MaxStamina); //소모된 행동력 표시
            }
        };
        enemyData.on_Enemy_HP_Change += (hpValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, enemyData.MaxHp); //소모된 행동력 표시
            }
            if (enemyData.HP < 0)
            {
                GameManager.PlayerStatus.GetExp((uint)enemyData.EnemyExp);
                ResetData();
                onDie?.Invoke(this);
            }
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
    public void ResetData()
    {
        if (BattleUI != null) //배틀 UI가 셋팅되있으면 
        {
            BattleUI.ResetData();// 추적형 UI 초기화 
            BattleUI = null; // 비우기
        }
        //Debug.Log($"{currentTile.width},{currentTile.length} ,{currentTile.ExistType}  몬스터 초기화 ");
        currentTile.ExistType = Tile.TileExistType.None; // 속성 돌리고 
        
        currentTile = null; //타일 참조해제
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }


  



  
    /// <summary>
    /// 공격범위안에 있으면 공격하는 함수 
    /// </summary>
    private void IsAttackAction()
    {
        Tile attackTile = Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, enemyData.AttackRange);
        //Debug.Log(SpaceSurvival_GameManager.Instance.PlayerTeam[0]);

        if (attackTile != null)
        {
            Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        }
    }


    public void Attack_Enemy(IBattle target)
    {
        target.Defence(enemyData.AttackPower);
    }

    public void Defence(float damage, bool isCritical = false)
    {
        float finalDamage = Mathf.Max(0, damage - enemyData.DefencePower);
        GameManager.EffectPool.GetObject(finalDamage, transform, isCritical);
        enemyData.HP -= finalDamage;
    }


    void EnemyAi()
    {
        Debug.Log($"{transform.name}턴 시작 - [체력:{enemyData.HP}] / [행동력:{enemyData.Stamina}] / [타입:{enemyData.mType}]\n[좌표:{CurrentTile.transform.position}] / [{currentTile.name}]");

    }

    public void CharcterMove(Tile selectedTile)
    {
        List<Tile> path = Cho_BattleMap_Enemy_AStar.PathFind(
                                                           SpaceSurvival_GameManager.Instance.BattleMap,
                                                           SpaceSurvival_GameManager.Instance.MapSizeX,
                                                           SpaceSurvival_GameManager.Instance.MapSizeY,
                                                           this.currentTile,
                                                           selectedTile,
                                                           moveSize
                                                           );
        StopAllCoroutines();
        StartCoroutine(EnemyMove(path));
    }
    //[SerializeField]
    //Animator unitAnimator;
    //int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    IEnumerator EnemyMove(List<Tile> path)
    {
        Vector3 targetPos = currentTile.transform.position; //길이없는경우 현재 타일위치 고정
        //unitAnimator.SetBool(isWalkingHash, true); //이동애니메이션 재생 시작

        foreach (Tile tile in path) //몬스터 중복 방지 용으로 타일값 미리셋팅해서 체크하자 
        {
            tile.ExistType = Tile.TileExistType.Monster;//이것이 실행됬으면 밑에로직은 무조건 실행되야 정상동작된다.
        }

        foreach (Tile tile in path)  // 길이있는경우 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //새로운 위치잡고 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //해당방향 바라보고 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}타일 오브젝트 이동중에 타일 데이터일단 move로변경");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 변경되야된다 charcter 로 ");

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //이동시작
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);



        IsAttackAction(); //공격 범위안에있는지 체크해서 공격하기

        onActionEndCheck?.Invoke(); //행동끝났으면 신호보내기
    }

    public void EnemyAi(Tile PlayerTile)
    {
        CharcterMove(PlayerTile);
    }


}
