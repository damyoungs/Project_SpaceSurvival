using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 배틀맵에서 플레이어의 이벤트핸들러내용을 정의할 컴포넌트  
/// 클릭했을때 이벤트별로 연결시켜주기
/// </summary>
public class BattleMap_Player_Controller : MonoBehaviour
{

    /// <summary>
    /// 레이가 타일에 충돌됬을때 체크할 레이어 값
    /// 에디터상에서 레이어를 셋팅해줘야한다.
    ///  LayerMask.GetMask();// 2진코드로 작성되있어서 값이 0,1,2,4,8,16 처럼 2의 배수로 순차적으로 저장되있다 
    ///  LayerMask.NameToLayer("");// 겟마스크와다르게 저장된순번 을가져온다 0,1,2,3,4,5,6,7 .... 
    ///  GameObject.Layer 는 순번이 저장되있다 
    ///  readOnly 로 미리 검색시도했더니 
    /// </summary>
    [SerializeField]
    int tileLayerIndex;

    [SerializeField]
    int uiLayerIndex;

    /// <summary>
    /// 플레이어가 턴인지 확인하기위해 가져오는 오브젝트
    /// </summary>
    PlayerTurnObject playerTurnObject;
    public PlayerTurnObject PlayerTurnObject 
    {
        get 
        {
            if (playerTurnObject == null) //데이터가 없으면 
            {
                //playerTurnObject = FindObjectOfType<PlayerTurnObject>(); //값이없으면 찾는로직 좀무겁다.
                //델리를 이용해 찾아본다 
                playerTurnObject = GetPlayerTurnObject?.Invoke(); //위에거 사용하기에는 부담이되서 바꿧다.
            }
            return playerTurnObject;
        }
    }
    /// <summary>
    /// 객체가 초기화 되는 타이밍이 틀려서 설정해둔 델리게이트
    /// </summary>
    public Func<PlayerTurnObject> GetPlayerTurnObject;
    
    /// <summary>
    /// 레이가 최대로 체크할 거리
    /// </summary>
    [SerializeField]
    float ray_Range = 15.0f;

    /// <summary>
    /// 이동가능한 타일 클릭시 신호를넘겨준다
    /// </summary>
    public Action<Tile> onMoveActive;
    
    /// <summary>
    /// 타일에 몬스터가 있고 클릭했을때 신호를 넘겨준다
    /// </summary>
    public Action<Tile> onClickMonster;
   
    /// <summary>
    /// 타일에 아이템이 있고 클릭햇을때 신호를 넘겨준다
    /// </summary>
    public Action<Tile> onClickItem;

    /// <summary>
    /// 캐릭터인 타일을 클릭했을때 신호를 넘겨준다
    /// </summary>
    public Action<Tile> onClickPlayer;

    /// <summary>
    /// 이벤트 제어 하기위한 델리게이트 나중에 메니저 만들어서 처리할때 필요 
    /// </summary>
    //public Action<Tile> onOffEventHandle;

    private void Awake()
    {
        tileLayerIndex = LayerMask.NameToLayer("Ground");
        uiLayerIndex = LayerMask.NameToLayer("UI");

       
    }
    private void Start()
    {
        InputSystemController.Instance.OnBattleMap_Player_UnitMove += OnMove;
    }
    /// <summary>
    /// 클릭했을때 레이를 쏴서 레이에 충돌한 객체들을 가져오고 
    /// 레이어로 나눠서 처리를 하는 로직 
    /// </summary>
    private void OnMove()
    {
        if (PlayerTurnObject == null) //플레이어가 현재 턴인경우만 실행하도록 체크
        {
            Debug.Log($"{playerTurnObject}플레이어가 셋팅 안되있거나 플레이어가 현재 턴이아닙니다.");
            return;
        }
        else if (!playerTurnObject.IsTurn)
        {
            Debug.Log($"턴아니라고 그만클릭해 {playerTurnObject.IsTurn}");
            return;
        }
        else if (SpaceSurvival_GameManager.Instance.IsUICheck) 
        {
            Debug.Log("UI 사용중입니당");
            return;
        }
            //Debug.Log($"인풋시스템에서는 클릭한 곳의 오브젝트까지는 못가져온다 그래서 레이로 쏴서 가져와야한다.");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());      // 화면에서 현재 마우스의 위치로 쏘는 빛
        Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.red, 1.0f);              // 디버그용 레이저

        RaycastHit[] hitObjets = Physics.RaycastAll(ray, ray_Range); //레이를 쏴서 충돌한 오브젝트 리스트를 받아온다.
        
        foreach (RaycastHit hit in hitObjets) // 내용이 있는경우 내용을 실행한다.
        {
            if (hit.collider.gameObject.layer == uiLayerIndex) 
            {
                Debug.Log($"{hit.collider.name}오브젝트는 UI다");
            }
            if (hit.collider.gameObject.layer == tileLayerIndex) //타일인지 체크하고 
            {
                OnTileClick(hit); //타일 클릭되있을때 로직을 실행 
                break; //한번클릭에 한번만 로직을 실행할수있게 브레이크를 잡는다.
            }
             //클릭이벤트 여기에 추가로연결 
        }

    }

    /// <summary>
    /// 타일에 설정된 값이 체크되있을경우 
    /// </summary>
    /// <param name="hitInfo"></param>
    private void OnTileClick(RaycastHit hitInfo) 
    {
        Tile targetTile = hitInfo.transform.GetComponent<Tile>();
        if (targetTile != null) //타일이 클릭 됬을경우 
        {
            switch (targetTile.ExistType) //타일 상태확인하고 
            {
                case Tile.TileExistType.None:
                    break;
                case Tile.TileExistType.Charcter:
                    Debug.Log($"이동불가 캐릭터: 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                    onClickPlayer?.Invoke(targetTile); 
                    break;
                case Tile.TileExistType.Monster:
                    onClickMonster?.Invoke(targetTile);
                    //몬스터 클릭시 몬스터에대한 정보가 나오던 뭔가 액션이필요
                    Debug.Log($"이동불가 몬스터: 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                    break;
                case Tile.TileExistType.Item:
                    onMoveActive?.Invoke(targetTile);//이동로직 실행
                    onClickItem?.Invoke(targetTile); //아이템있는곳을 클릭했을때 로직실행
                    // 아이템이 타일에있는경우 아이템 에대한 정보를 띄우던 뭔가을 액션 
                    break;
                case Tile.TileExistType.Prop:
                    Debug.Log($"이동불가 장애물 : 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                    break;
                case Tile.TileExistType.Move:
                    //Debug.Log(targetTile);
                    onMoveActive?.Invoke(targetTile);//이동로직 실행
                    Debug.Log($"이동가능 : 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                    break;
                default:
                    Debug.Log($"접근되면 안된다.");
                    break;
            }
        }
    }


}
  
