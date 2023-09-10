using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapPlayerBase : PlayerBase_PoolObj, ICharcterBase
{
    /// <summary>
    /// 현재 캐릭이 컨트롤할수있는상태인지 체크
    /// </summary>
    bool isControll = false;
    public bool IsControll
    {
        get => isControll;
        set => isControll = value;
    }

    /// <summary>
    /// 캐릭터 데이터 연동용 변수
    /// </summary>
    Player_ charcterData;
    public Player_ CharcterData => charcterData;
    /// <summary>
    /// 스태미나 최대치 
    /// </summary>
    const float charcterStaminaMaxValue = 10.0f;


    /// <summary>
    /// 이동버그가 존재해서 체크하는 변수
    /// </summary>
    bool isMoveCheck = false;
    public bool IsMoveCheck => isMoveCheck;

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
    /// 현재 내위치에있는 타일
    /// </summary>
    Tile currentTile;
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

    public Func<Tile> GetCurrentTile { get; set; }
    
    /// <summary>
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// 행동력 혹은 이동 거리
    /// </summary>
    protected float moveSize = 5.0f;
    public float MoveSize 
    {
        get => moveSize;
        set => moveSize = value;
    }


    /// <summary>
    /// 좌측상단에있는 캐릭터 상태창
    /// </summary>
    UICamera viewPlayerCamera;


    protected override void Awake()
    {
        base.Awake();
        charcterData = GetComponentInChildren<Player_>();
        charcterData.on_Player_Stamina_Change += (stmValue) => {
            
            TurnManager.Instance.CurrentTurn.TurnActionValue = stmValue;
            moveSize = stmValue;
            if (battleUI != null) 
            {
                BattleUI.stmGaugeSetting(stmValue, charcterStaminaMaxValue); //소모된 행동력 표시
            }
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(currentTile, moveSize);//이동범위표시해주기 
            if (stmValue < 1.0f) //최소행동값? 보다 낮으면 
            {
                TurnManager.Instance.CurrentTurn.TurnEndAction();//턴종료 
            }
        };
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        InitUI();//맨처음 
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
        charcterMove = CharcterMoveCoroutine(null,0.0f);
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
        if (viewPlayerCamera == null)  //카메라 셋팅안되있으면 
        {
            viewPlayerCamera = EtcObjects.Instance.TeamCharcterView;// EtcObject 에 미리 만들어둔 게임오브젝트 가져오기 큐로 관리중이다 
            Transform cameraTarget = transform.GetChild(0); //캐릭터위치
            viewPlayerCamera.TargetObject = cameraTarget.GetChild(cameraTarget.childCount-1); //캐릭터안에 맨밑에 카메라 타겟을 만들어둬야쫒아다닌다.
            viewPlayerCamera.gameObject.SetActive(true); //셋팅끝낫으면 활성화시키기
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
        if (viewPlayerCamera != null)
        {
            viewPlayerCamera.TargetObject = null; //타겟 지우고
            viewPlayerCamera.gameObject.SetActive(false); // 비활성화 시키고 내부적으로 큐로 돌린다.
            viewPlayerCamera = null; //참조 지우기
        }
        currentTile.ExistType = Tile.TileExistType.None; // 속성 돌리고 
        currentTile = null; //타일 참조해제
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }


    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
    /// <summary>
    /// 승근씨 AStar 로직 조금수정한내용 가져와서 사용 하고 
    /// 데이터있으면 이동시킨다.
    /// </summary>
    /// <param name="currentTile"></param>
    public void CharcterMove(Tile currentTile)
    {
        if (!isMoveCheck) //이동중간에 끊길시 타일버그가 발생해 체크하는로직추가 
        {
            List<Tile> path = Cho_BattleMap_AStar.PathFind(
                                                            SpaceSurvival_GameManager.Instance.BattleMap,
                                                            SpaceSurvival_GameManager.Instance.MapSizeX,
                                                            SpaceSurvival_GameManager.Instance.MapSizeY,
                                                            this.currentTile,
                                                            currentTile
                                                            );
            //StopAllCoroutines();
            //StartCoroutine(CharcterMove(path));
            StopCoroutine(charcterMove);
            charcterMove = CharcterMoveCoroutine(path, currentTile.MoveCheckG);
            StartCoroutine(charcterMove);
           
        }
        
    }

    [SerializeField]
    Animator unitAnimator;
    int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float rotateSpeed = 10.0f;
    IEnumerator charcterMove;
    /// <summary>
    /// 승근씨가 짜둔 길찾기 가져오기
    /// 
    /// 이동버그 존재함 
    /// - 어떠한 상황에서 발생하는지는 파악이안되나 타일의 값이 charcter 로 셋팅이안되는 상황이 발생 
    ///   이동시 해당로직에서 데이터를 바꾸고있기때문에 여기인거같은데 정확하게 파악을 못하고있음.
    /// </summary>
    /// <param name="path">A스타 최단거리 타일리스트</param>
    /// <param name="useTurnValue">이동했을때 소모될 값</param>
    /// <returns></returns>
    IEnumerator CharcterMoveCoroutine(List<Tile> path , float useTurnValue)
    {
        isMoveCheck = true; //이동 중인지 체크하기 
        Vector3 targetPos = currentTile.transform.position;
        unitAnimator.SetBool(isWalkingHash, true); //이동애니메이션 재생 시작
        foreach (Tile tile in path)  // 길이있는경우 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //새로운 위치잡고 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //해당방향 바라보고 
            this.currentTile.ExistType = Tile.TileExistType.Move;// 기존위치 이동가능하게 바꾸고  
            //Debug.Log($"{this.currentTile.Index}타일 오브젝트 이동중에 타일 데이터일단 move로변경");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 변경되야된다 charcter 로 ");
            tile.ExistType = Tile.TileExistType.Charcter; //이동한위치 못가게 바꾼다.
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 charcter 변경되었다.");
            
            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //이동시작
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);

        charcterData.Stamina -= this.currentTile.MoveCheckG; //최종이동한 거리만큼 스태미나를 깍는다.

        isMoveCheck = false; //이동끝낫는지 체크
    }

}
