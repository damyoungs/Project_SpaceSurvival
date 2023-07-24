using EnumList;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 추적형 UI 기능 클래스 
/// 상태이상 과 체력 관련 UI 기능 들어있는 컴퍼넌트
/// </summary>
public class TrackingBattleUI : TrackingBattleUIObjectIsPool 
{
#if UNITY_EDITOR
    public bool isDebug = false; //디버그 표시해줄 체크박스 에디터에 추가하자
#endif

    /// <summary>
    /// 추적할 오브젝트 유닛
    /// </summary>
    [SerializeField]
    private ITurnBaseData player = null;
    public ITurnBaseData Player
    {
        get => player;
        set
        {
            player = value;
        }

    }

    /// <summary>
    /// 현재 보여주고있는 카메라 거리측정용으로 사용된다 
    /// </summary>
    [SerializeField]
    private Camera mainCamera;

    /// <summary>
    /// 카메라와의 거리 최소값 이거리 이하가 되면 안보여줘야된다.
    /// </summary>
    float minDirection = 3.0f;

    /// <summary>
    /// 카메라와의 거리 최대값 이거리 이상이 되면 안보여줘야된다
    /// </summary>
    float maxDirection = 20.0f;

    /// <summary>
    /// 카메라와 캐릭터간의 기본거리값 카메라가 타겟팅으로 잡고있는 캐릭터간의 거리 
    /// </summary>
    float defaultDir = 10.0f;

    /// <summary>
    /// 상태이상 한줄에 들어갈 갯수
    /// </summary>
    const int gridWidthLength = 4;

    /// <summary>
    /// 1기준으로 한줄에 들어가는 갯수를 나눈값
    /// 나눗셈을 한번만하기위한 꼼수 
    /// </summary>
    readonly float gridWidthScale = 1.0f / gridWidthLength;


    /// <summary>
    /// 상태이상이 들어갈 그리드레이아웃 그룹
    /// </summary>
    GridLayoutGroup glg;

    /// <summary>
    /// 위에보여줄 상태이상 위치값  
    /// </summary>
    RectTransform rtTop;

    /// <summary>
    /// 아래에 보여줄 체력및 스태미나 보여줄 위치값
    /// </summary>
    RectTransform rtBottom;

    /// <summary>
    /// 상태이상 위치값 담을 백터
    /// </summary>
    Vector2 topDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// 체력및 스태미나 위치값 담을 백터
    /// </summary>
    Vector2 bottomDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// 상태이상 크기조절용 백터
    /// </summary>
    Vector2 topDefaultDeltaSize = Vector2.zero;

    /// <summary>
    /// 체력 및 스태미나 크기조절용 백터
    /// </summary>
    Vector2 bottomDefaultSize = Vector2.zero;

    /// <summary>
    /// 상태이상의 크기변경을 위해 사용되는 백터
    /// </summary>
    Vector2 topGroupCellSize = Vector2.zero;

    /// <summary>
    /// 상태이상이 늘어날경우 그리드위치변경을위해 사용되는 백터
    /// </summary>
    Vector2 gridPos = Vector2.zero;

    /// <summary>
    /// 상태이상 UI 들어갈 그리드그룹 위치값
    /// </summary>
    Transform stateGroup;

    /// <summary>
    /// 상태이상이 걸려서 추가 되는 경우 호출
    /// </summary>
    public Action<StateType> statusAbnormalities;

    /// <summary>
    /// 지속시간이 지나거나 아이템을 사용해서 상태가 해제된 경우 호출
    /// </summary>
    public Action<IStateData> releaseStatus;

    /// <summary>
    /// 상태이상 최대갯수 
    /// </summary>
    int stateSize = 4;
    int StateSize {
        get => stateSize;
        set
        {
            if (value > stateSize) //크기가 기본사이즈보다크면 
            {
                stateSize *= 2; //일단 두배로 늘린다
                IStateData[] temp = states; //기존값들을 백업해둔다.
                states = new IStateData[stateSize]; // 늘린사이즈만큼 새로 배열 만든다. 
                for (int i = 0; i < temp.Length; i++)
                {
                     states[i] = temp[i]; //기존값들을 다시 담는다.
                }
            }
        }
        
    }

    /// <summary>
    /// 현재 상태이상 상태 담아둘 배열
    /// </summary>
    private IStateData[] states;
    public IStateData[] States => states;

    /// <summary>
    /// 초기값들을 셋팅해둔다 나중에 거리에따른 사이즈조절에 사용할값
    /// </summary>
    private void Awake()
    {
        stateGroup = transform.GetChild(0);
        glg = stateGroup.GetComponent<GridLayoutGroup>();
        rtTop = stateGroup.GetComponent<RectTransform>();
        rtBottom = transform.GetChild(1).GetComponent<RectTransform>();

        //맨처음 맞춰놓은 사이즈(기본)값을 저장해둔다.
        topGroupCellSize = glg.cellSize;
        //상태이상
        topDefaultAnchoredPosition = rtTop.anchoredPosition;
        topDefaultDeltaSize = rtTop.sizeDelta;
        //체력 스태미나
        bottomDefaultAnchoredPosition = rtBottom.anchoredPosition;
        bottomDefaultSize = rtBottom.sizeDelta;


        statusAbnormalities += AddOfStatus; // 상태이상이 추가될때 호출될 함수등록

        states = new IStateData[stateSize]; // 상태이상의 배열크기를 잡아둔다.
        
        isPositionReset = false; //위치값 초기화 막기
    }

    /// <summary>
    /// 활성화시 셋팅하기
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        InitTracking();
    }
   
    /// <summary>
    /// 배틀맵 시작시 초기화할 값들 셋팅 
    /// </summary>
    private void InitTracking() 
    {
        mainCamera = FindObjectOfType<Camera>(); //카메라 찾아서 셋팅한다. - 시네머신으로 카메라 전환시 해당값을 교체하는 로직을 추가가 필요
        //mainCamera = Camera.main; //유니티에서 제공해주는걸 사용해보자 awake에서는 못가져온다. OnEnable 처음호출에서도못가져온다.
        StopAllCoroutines();//기존추적하던게있으면 멈추고 
        StartCoroutine(StartTracking()); //새로추척
    }
    /// <summary>
    /// 추적 시작
    /// </summary>
    IEnumerator StartTracking() 
    {
        while (true)
        {
            SetTrackingUI(); //캐릭터가 움직일때마다 UI 크기및 위치를 변경시켜줘야한다. 
            yield return null;

        }
    }
    /// <summary>
    /// 카메라와 플레이어간의 거리를 재서 추적형 UI 크기를 조절시키는 함수
    /// </summary>
    private void SetTrackingUI()
    {
        if (Player != null) //플레이어가 존재하면
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(Player.transform.position); //플레이어 스크린좌표를 읽어온다.
            transform.position = playerPosition; //주적할 오브젝트의 위치를 쫒아간다.
#if UNITY_EDITOR
            if (isDebug) 
            {
                Debug.Log(playerPosition);
            }
#endif
            //UI보이는 거리 구간을 확인
            if (playerPosition.z < maxDirection && playerPosition.z > minDirection) //해당범위안에서만 보여준다. z기준으로만잡고있지만 추가해주면된다.
            {
                //기준으로 가까워지면 배율이 증가하고 멀어지면 배율이 감소하는 연산을 하고싶은데 곱하기로 하려면 if문이 엄청들어간다 
                float scale = defaultDir / playerPosition.z; //배율을 뽑기위해 곱하기연산으로 돌리고싶었지만 방법을 못찾앗다.
                                                             //초기값에서 배율로 곱해서 사이즈정한다.
                                                             ////배율별로 사이즈조절
                rtBottom.anchoredPosition = bottomDefaultAnchoredPosition * scale;
                rtBottom.sizeDelta = bottomDefaultSize * scale;

                ////상태이상
                glg.cellSize = topGroupCellSize * scale; //셀사이즈를 조절하고 
                gridPos = topDefaultAnchoredPosition * scale; //상태이상기본위치를 설정한뒤 
                //상태이상이 한줄에서 두줄 , 두줄에서 세줄 같이 변경될때를 위해 계산을한다.
                gridPos.y += (glg.cellSize.y * //셀사이즈만큼 간격을 추가한다 
                                (int)((transform.GetChild(0).childCount - 1) * gridWidthScale)); //한줄이 넘어가면 

                rtTop.anchoredPosition = gridPos; //수정된 값을 적용       
                
                rtTop.sizeDelta = topDefaultDeltaSize * scale; //상태이상의 크기를 조절

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true); //화면에 보여준다.
                }

            }
            else //벗어나면 
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); //안보여준다.
                }
            }
        }
    }
   
    /// <summary>
    /// 한턴당 상태이상의 진행도를 감소시키는 함수  (수정하는함수)
    /// 턴메니져에서 일괄실행  - 테스트 진행중
    /// </summary>
    public void TrunActionStateChange() 
    {
        for (int i=0; i<  states.Length; i++) 
        {
            if (states[i] == null) continue; //빈값이면 다음으로 
            states[i].CurrentDuration += states[i].ReducedDuration; //값이 존재하면 상태이상 갱신
            if (states[i].CurrentDuration < 0.0f) // 상태이상 지속시간이 끝났으면 
            {
                releaseStatus?.Invoke(states[i]);//상태해제됬다고 신호를 보낸다.
                states[i] = null; //배열에서 삭제
            } 
        }

    }

    /// <summary>
    /// 상태변화 발생(추가)시 UI 추가 
    /// 유닛쪽에서 호출 하는 함수 
    /// </summary>
    /// <param name="stateData">상태이상 의 타입</param>
    public void AddOfStatus(EnumList.StateType type)
    {
        IStateData stateData = SettingStateUI(type); //풀에서 객체가져와서 UI셋팅
        AddStateArray(stateData); //데이터셋팅
        
    }

    /// <summary>
    /// 현재 진행중인 상태이상 데이터 셋팅
    /// 배열에 추가하고 관리한다.
    /// </summary>
    /// <param name="addData">상태이상의 정보</param>
    private void AddStateArray(IStateData addData) 
    {
        for(int i=0; i< StateSize; i++)//전체검색해보고 
        {
            if (states[i] == null) //빈곳이있는경우 
            {
                states[i] = addData;//추가하고
                return;//빠져나간다.
            }
        }
        StateSize++; //상태리스트 꽉차있으면 배열 사이즈늘리고 
        AddStateArray(addData); // 함수를 다시호출해서 배열에 추가시킨다.
    }

    /// <summary>
    /// 상태이상이 발동시 호출될 함수 
    /// 상태이상별로 UI처리관련 내용을 추가할예정
    /// </summary>
    /// <param name="type">추가된 상태이상타입</param>
    /// <returns>상태이상의 정보를 생성해서 반환</returns>
    private IStateData SettingStateUI(EnumList.StateType type)
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.STATE_POOL); //풀에서 꺼내고
        StateObjectIsPool poolObj = obj.GetComponent<StateObjectIsPool>(); //컴포넌트 내용읽어와서 
        poolObj.transform.SetParent(stateGroup);// 부모 셋팅하고 

        ///밑에는 이미지 셋팅 
        switch (type)
        {
            case EnumList.StateType.ElectricShock:
                //이미지셋팅
                break;
            case EnumList.StateType.Poison:

                break;
            case EnumList.StateType.Freeze:

                break;
            case EnumList.StateType.Burns:

                break;
            case EnumList.StateType.Fear:

                break;
            default:
                break;
        }
        return poolObj;
    }

    /// <summary>
    /// 초기화 함수
    /// 셋팅값 초기화및 풀로 돌리고걸려있는 상태이상 도 전부 초기화 
    /// 최종적으로 큐에 추가하고 풀로 돌려버린다.
    /// </summary>
    public  void ResetData() 
    {
        //델리게이트 초기화 
        statusAbnormalities = null;
        releaseStatus = null;
        //거리재기위한 카메라와 기준점이될 플레이어 참조를 해제 
        mainCamera = null;
        Player = null;

        for (int i = 0; i < StateSize; i++) //상태이상 내용을 전부
        {
            if (states[i] != null) //값이 들어 있는것들 찾아서
            {
                states[i].ResetData(); //내부값들 초기화및 풀로 돌리기작업 하고
                states[i] = null; // 빈값으로 셋팅
            }
        }
       
        transform.SetParent(PoolTransform);//풀로 돌린다
        gameObject.SetActive(false); //큐로 돌리고 
        //초기화 마지막에 풀로 돌린다.

        
    }


    


}
