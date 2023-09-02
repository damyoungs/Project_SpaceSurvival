using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapPlayerBase : PlayerBase_PoolObj , ICharcterBase
{
    /// <summary>
    /// 현재 캐릭이 컨트롤할수있는상태인지 체크
    /// </summary>
    public bool isControll = false;

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
    public float MoveSize => moveSize;


    /// <summary>
    /// 좌측상단에있는 캐릭터 상태창
    /// </summary>
    UICamera viewPlayerCamera;


    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        InitUI();//맨처음 
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
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
        this.currentTile = null; //타일정보 지우기
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }


    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
    public void CharcterMove(Tile currentTile)
    {
        List<Tile> path = Cho_BattleMap_AStarDouble.PathFind(
                                                        SpaceSurvival_GameManager.Instance.BattleMapDoubleArray,
                                                        this.currentTile,
                                                        currentTile
                                                        );
        StopAllCoroutines();
        StartCoroutine(CharcterMove(path));
    }

    [SerializeField]
    Animator unitAnimator;
    int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float rotateSpeed = 10.0f;
    /// <summary>
    /// 승근씨가 짜둔 길찾기 가져오기
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator CharcterMove(List<Tile> path)
    {
        Vector3 targetPos = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, true);
        foreach (Tile tile in path) 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position;
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            
            while ((targetPos - transform.position).sqrMagnitude > 0.04f) 
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
            
            this.currentTile.ExistType = Tile.TileExistType.Move;// 기존위치 이동가능하게 바꾸고  
            this.currentTile = tile;
            this.currentTile.ExistType = Tile.TileExistType.Monster; //이동한위치 못가게 바꾼다.
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);
    }
}
