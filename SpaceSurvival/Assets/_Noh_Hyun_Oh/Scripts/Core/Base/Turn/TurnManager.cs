using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 정렬방법 오름차순(ASCENDING), 내림차순(DESCENDING)
/// </summary>
public enum SortType
{
    Ascending = 0, //오름차순
    Descending     //내림차순
}

/// <summary>
/// 턴 순번을 관리해줄 클래스
/// 
/// 1.링크드 리스크사용시 Find 를 하기위해선 객체가 생성이되야하는데 
///     MonoBehaviour 를 상속받은 클래스는 new 키워드로 생성 이안되고  AddComponent 로 만들어야한다.
///     MonoBehaviour 테스트데이터 만들때 new 로 생성했더니 값이 null 이 들어가서 LinkedList Find 함수가 제대로 위치를 찾지 못하였음
/// 
/// TurnBaseData 상속받은 객체에서 턴끝났을경우 그개체의 값이 수정되고 수정된이후 그값을 가지고 다시 재정렬을 한다.
/// 
/// 링크드리스크를 다시 배열 혹은 리스트로 바꾸는법
/// List<ISortBase> b =  turnObjectList.ToList<ISortBase>(); 리스트로 바꾸는방법
/// ISortBase[] a =  turnObjectList.ToArray<ISortBase>();    배열로 바꾸는방법 
/// 
/// </summary>
/// 


public class TurnManager : ChildComponentSingeton<TurnManager>
{
    /// <summary>
    /// 턴게이지 보여줄지 여부
    /// </summary>
    [SerializeField]
    private bool isViewGauge = false;
    public bool IsViewGauge => isViewGauge;
    /// <summary>
    /// 턴관리할 링크드 리스트
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList = null;

    /// <summary>
    /// 배틀 유닛의 유니크한 인덱스값을 셋팅하기위해 넘버링 할 변수
    /// </summary>
    int battleUnitIndex = -1;
    public int BattleIndex => battleUnitIndex;
    /// <summary>
    /// 화면에 턴진행도 보여줄 UI 위치
    /// </summary>
    Transform turnGaugeUI;



    /// <summary>
    /// 전체리스트의 정렬방식을 정한다. 값이 큰것부터 턴이시작되야되면 Descending 작은것부터 시작되야되면 Ascending 을 넣어주면된다
    /// true = 오름차순 (- ~ +) , false 내림차순 (+ ~ -)
    /// </summary>
    [SerializeField]
    SortType isAscending = SortType.Descending;


    /// <summary>
    /// 현재까지 진행된 턴의값
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    /// <summary>
    /// 턴시작의 최소값
    /// </summary>
    [SerializeField]
    [Range(0.1f,1.0f)]
    private float turnStartValue = 1.0f;

    /// <summary>
    /// Battle UI 최상단 위치
    /// </summary>
    Transform uiParent;

    /// <summary>
    /// 시간제한이 있을경우 제한턴의 값
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// 씬 이동시 배틀맵인지 체크하는 변수 로드 할때 변경해준다.
    /// 테스트용 변수
    /// </summary>
    bool isBattleMap = false;
    public bool IsBattleMap
    {
        get => isBattleMap;
        set => isBattleMap = value;
    }
    //UICamera[] battleInfoCamArray;

    private void Start()
    {
        turnGaugeUI = WindowList.Instance.TurnGaugeUI; //턴게이지 위치 찾기


        uiParent = WindowList.Instance.transform.GetChild(0); //Battle UI최상단 가져오기
    }


    /// <summary>
    /// 배틀 맵일경우 데이터 초기화가 이루어진뒤에 호출이 되야된다 .
    /// 캐릭터 데이터가 전부생성이된상태일때 인자값으로받을지 결정한다.
    /// </summary>
    public void InitTurnData() {
        turnIndex = 0; //턴값 초기화
        
        ITurnBaseData[] turnListTemp = GameObject.FindObjectsOfType<TurnBaseObject>(); //씬에있는 유닛들 긁어오기 
        
        if (turnListTemp == null || turnListTemp.Length == 0)
        {
            Debug.LogWarning("배틀맵 유닛이없어 임마");
            return;
        }

        Array.Sort(turnListTemp, SortComparison); //정렬

        turnObjectList = new LinkedList<ITurnBaseData>(turnListTemp);//링크드 리스트 초기화
         
      
        TurnStart();//턴시작
    }

    

    /// <summary>
    /// 턴시작할 오브젝트를 가져와서 시작함수를 호출한다.
    /// </summary>
    private void TurnStart() {
        turnIndex++; //턴시작마다 카운트 시킨다.

        ITurnBaseData currentTurn = GetCurrentTurnObject(); //처음 턴유닛을 찾아와서 


        if (turnStartValue < currentTurn.TurnActionValue) //턴진행 할수있는 값이 됬으면 턴진행
        {
            currentTurn.TurnEndAction = TurnEnd; //턴종료시 신호를 받을 델리게이트 셋팅하고
            /*
             * 적군움직일때 클릭이벤트를 제한 하거나 할때 기능추가 위치
             */


            currentTurn.TurnRemove = TurnListDeleteObj; //턴진행중 삭제될 유닛이 있으면 삭제함수를 연결시킨다.

            currentTurn.TurnStartAction();  //턴시작을 알린다
        }
        else  //아니면 턴을 종료해서 행동력값을 증가시킨다.
        {
            TurnEnd(currentTurn);
        }
        
    }

    /// <summary>
    /// 현재 턴을 진행해야할 오브젝트반환.
    /// </summary>
    /// <returns>리스트의 첫번째 목록의 값을 반환</returns>
    private ITurnBaseData GetCurrentTurnObject()
    {
        return turnObjectList.First.Value;   

    }

    /// <summary>
    /// 턴종료시 실행할 내용
    /// </summary>
    /// <param name="turnEndObj">턴종료한 유닛</param>
    private void TurnEnd(ITurnBaseData turnEndObj)
    {
        turnEndObj.TurnRemove = null;
        turnEndObj.TurnEndAction = null; // 턴종료가 끝낫으면 델리게이트를 초기화한다

        SetTurnValue();// 턴종료시마다 리스트의 유닛들의 행동력 값을 추가해주는 기능
        
        // TurnSorting(turnEndObj); // 값이 변경된 오브젝트의 정렬기능 실행 -- 턴종료마다 행동력증가폭이 같으면 해당함수가 실행되는의미가있다.

        //추가되는 행동력 값이 전부다르다는 전제하에 전체정렬을 재시도 
        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //값이변경이 됬음으로 전체 재정렬
        
        
        /*
         *  배틀맵에서 승리조건을 체웠을때 기능 추가해야되는 위치
         */


        TurnStart();
    }

    /// <summary>
    /// 턴이끝난뒤 턴리스트의 오브젝트들의 행동력을 추가시킨다.
    /// </summary>
    private void SetTurnValue() {

        foreach (ITurnBaseData node in turnObjectList) //턴이끝날때마다 
        {
            node.TurnActionValue += node.TurnEndActionValue;//리스트의 대기자들은 활동력을 추가 시킨다.
        }
    }


    /// <summary>
    /// 턴관리할 오브젝트가 사라질경우
    /// 턴진행리스트 에서 사라져야할 것들 제거
    /// </summary>
    /// <param name="deleteTurnObject">리스트에서 지울 턴</param>
    public void TurnListDeleteObj(ITurnBaseData deleteTurnObject)
    {
        if (deleteTurnObject.CharcterList.Count < 1) //턴오브젝트의 관리할유닛이 없는경우 
        {
            deleteTurnObject.ResetData();//턴오브젝트도 초기화 시키고 
            turnObjectList.Remove(deleteTurnObject);//리스트에서 삭제
        }
        else 
        {
            Debug.Log($"아직 유닛이 {deleteTurnObject.CharcterList.Count}명 남아있습니다 ");
        }
        
    }

    /// <summary>
    /// 턴관리할 오브젝트가 추가될경우 
    /// 턴리스트에 추가될 값으로 재정렬 
    /// </summary>
    /// <param name="addObject">턴이 새롭게 추가된 객체</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //첫번째 노드 가져와서
        if (addObject == null) return; // 추가할값이없으면 리턴 

        for (int i = 0; i < turnObjectList.Count; i++)//리스트 한번돌정도로 포문을돌리고
        {
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(addObject, checkNode.Value, isAscending))//인자로받은 노드값이 리스트의 노드값과비교를해서 정렬기준을정한다. 
            {
                turnObjectList.AddBefore(checkNode, addObject); // 그앞단에 추가를해버린다.
                break;//추가했으면 빠져나간다.
            }
            else if (i == turnObjectList.Count - 1) //마지막까지 비교값이 정해지지않았으면  
            {
                turnObjectList.AddLast(addObject); //맨마지막에 추가를한다. 
                break;//추가했으면 빠져나간다.
            }
            checkNode = checkNode.Next; //값이 비교되지 않으면 다음 노드를 찾는다.
        }
    }

    /// <summary>
    /// 턴관련 데이터 초기화 
    /// </summary>
    public void ResetBattleData() 
    {
        if (turnObjectList == null) return; // 턴리스트가 존재하지 않으면 빠져나가기

        foreach (ITurnBaseData node in turnObjectList)  // 턴데이터 돌면서
        {
            node.ResetData(); //턴유닛의 데이터 초기화 
        }
        turnObjectList.Clear();//리스트 비우기 

        //UI 끄기
        for (int i = 1; i < uiParent.childCount - 1; i++)
        {
            for (int j = 0; j < uiParent.GetChild(i).childCount; j++)
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false); //액션버튼 끄기 
            }
        }
        turnGaugeUI.gameObject.SetActive(false); //턴게이지 끄기

    }

    /// <summary>
    /// 리스트와 Array 의 기본정렬기능을 이용한 정렬 
    /// 인자값의 자료형은 맞춰줘야한다 반환값은 int -1 0 1
    /// </summary>
    /// <param name="before">앞의값</param>
    /// <param name="after">뒤의값</param>
    /// <returns>비교 결과</returns>
    private int SortComparison(ITurnBaseData before, ITurnBaseData after)
    {
        if (before.TurnActionValue < after.TurnActionValue)  //두값을 비교 
        {
            return isAscending == SortType.Ascending ? -1 : 1;  //오름 차순이면 -1 내림차순이면 1 
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortType.Descending ? 1 : -1; //내림차순이면 1 오름 차순이면 -1
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// 제한 : 순차적으로 정렬된 상태여야 하고 이상황이 아닌상태에서 사용시 인덱스가 꼬일수있다.
    /// 리스트는 이미 정렬이된상태이기때문에 한쪽방향으로만 탐색하여 위치를교체한다. 
    /// 해당함수는 정렬이 다되있는 리스트에서 하나의 인덱스값이 변했을때 변한인덱스 를 정렬위치에 집어넣는기능이다 
    /// </summary>
    /// <param name="turnEndData">턴사용이 완료된 객체</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //비교할 노드 가져와서

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//비교할 다음노드를 미리가져온다.
        for (int i = 0; i < turnObjectList.Count; i++)//리스트 크기만큼 돌리고 
        {
            Debug.Log(nextNode);
            if (nextNode == null)
            {
                //Debug.Log($"{turnEndData.transform.name} 가 턴끝난뒤 정렬시  : 비교값 :{nextNode} :: 포문횟수 : {i} ::  링크드리스트 전체크기 :{turnObjectList.Count}");
                return;
            }
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(checkNode.Value, nextNode.Value, isAscending)) // 값비교 오름차순이냐 내림차순이냐에따라 달라진다.
            {                                       //교체가 필요할경우 
                turnObjectList.Remove(checkNode);   //일단 노드지우고
                                                    //안지우면 에러남 InvalidOperationException: The LinkedList node already belongs to a LinkedList.
                                                    //노드안에 이미 같은게있다고 추가할수없다고함.
                turnObjectList.AddBefore(nextNode, checkNode); //노드의 앞에 추가
                break;//위치가 변동이 있으면 빠져나간다 .
            }
            //교체가 필요없는경우 
            nextNode = nextNode.Next;   //다음노드를 찾는다
        }

    }

    /*======================================== 테스트용 ==================================================*/


    /// <summary>
    /// 턴리스트의 값확인용
    /// </summary>
    public void ViewTurnList() {
        foreach(ITurnBaseData j in turnObjectList) {
            Debug.Log($"{j} 값 : {j.TurnActionValue}");
        }
        Debug.Log(turnObjectList.Count);
    }
    /// <summary>
    /// 현재 진행중인 턴유닛 찾아오기 
    /// </summary>
    /// <returns></returns>
    public ITurnBaseData GetNode() {
        ITurnBaseData isTurnNode = null;
        foreach (ITurnBaseData node in turnObjectList) 
        {
            if (node.TurnEndAction != null) //현재 턴진행중이면 endAction 이 등록되있다 
            {
                Debug.Log(node.transform.name);
               isTurnNode = node;//진행중인 노드 담아서 
            }
        }
        return isTurnNode;//반환
    }
    /// <summary>
    /// 테스트용 랜덤한 유닛 가져오기 
    /// </summary>
    /// <returns></returns>
    public ITurnBaseData RandomGetNode() 
    {
        ITurnBaseData isRandNode = null;
        int randomIndex = UnityEngine.Random.Range(0,turnObjectList.Count); //리스트의 랜덤한 인덱스 값 가져오기 
        LinkedListNode<ITurnBaseData> node = turnObjectList.First;
        for (int i = 0; i < turnObjectList.Count; i++)
        {
            if (randomIndex == i) 
            {
                isRandNode = node.Value;
                break;
            }
            node = node.Next;
        }
        return isRandNode;
    }
    public ITurnBaseData GetPlayerCharcter() 
    {
        return player;
    }
    ITurnBaseData player;
    [SerializeField]
    int initSize = 1;
    /// <summary>
    /// 테스트 데이터 만드는용
    /// 기능: 맵에 플레이어 , 엔피씨 , 몬스터  와 같은 턴을사용하는 유닛이 생성된후에 그 리스트로 턴관리해줄 베이스를 생성 
    ///       팀원 1~3명 은 UI용 카메라를 추가해준다. 
    ///       
    /// </summary>
    public void TestInit() 
    {
        if (IsViewGauge) 
        {
            turnGaugeUI.gameObject.SetActive(true);
        }
        for (int i = 1; i < uiParent.childCount - 1; i++)
        {

            for (int j = 0; j < uiParent.GetChild(i).childCount; j++)
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }
        ITurnBaseData tbo;
        for (int i = 0; i < initSize; i++)
        {
            GameObject obj;
            battleUnitIndex = i;// 일단 체크용 변수 입력
            if (i == 0)
            {
                obj= Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL); //
                tbo = obj.GetComponent<PlayerTurnObject>(); //찾고new PlayerTurnObject();
                obj.name = $"PL_Charcter  _{i}";
                player = tbo;
            }
            else 
            {
                obj= Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //가져오고
                tbo = obj.GetComponent<EnemyTurnObject>();
                obj.name = $"ENEMY_Charcter _{i}";
            }
            Debug.Log(tbo);
            InitTurnObject(i,obj, tbo);
        }

    }
    private void InitTurnObject(int index ,GameObject obj, ITurnBaseData tbo) 
    {
       
        tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -테스트값 설정
        obj.transform.position = new Vector3(
                                        UnityEngine.Random.Range(-10.0f, 10.0f),
                                        0.0f,
                                        UnityEngine.Random.Range(-10.0f, 0.0f)
                                        );//랜덤위치로 뿌리고
    }
   

}
