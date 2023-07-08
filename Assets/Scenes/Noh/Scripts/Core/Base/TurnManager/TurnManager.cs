using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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
public class TurnManager : ChildComponentSingeton<TurnManager>
{
    /// <summary>
    /// 턴관리할 링크드 리스트
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList;


    /// <summary>
    /// 전체리스트의 정렬방식을 정한다. 값이 큰것부터 턴이시작되야되면 Descending 작은것부터 시작되야되면 Ascending 을 넣어주면된다
    /// true = 오름차순 (- ~ +) , false 내림차순 (+ ~ -)
    /// </summary>
    [SerializeField]
    SortComponent<ITurnBaseData>.SortType isAscending = SortComponent<ITurnBaseData>.SortType.Descending;


    /// <summary>
    /// 현재까지 진행된 턴의값
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    /// <summary>
    /// 시간제한이 있을경우 제한턴의 값
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// 씬 이동시 배틀맵인지 체크하는 변수 로드 할때 변경해준다.
    /// </summary>
    bool isBattleMap = false;
    public bool IsBattleMap
    {
        get => isBattleMap;
        set => isBattleMap = value;
    }

  
    /// <summary>
    /// 맵로딩마다 호출
    /// </summary>
    protected override void Init(Scene _, LoadSceneMode __)
    {
        //Debug.Log("로드완료");
        if(isBattleMap)//배틀 맵이면 
        { 
            InitTurnData(); //턴 진행 시작 
        }
    }


    /// <summary>
    /// 맵이동시 한번만 실행되야되고 
    /// 맵초기화 후 오브젝트 배치 끝난뒤
    /// 데이터 다생성되고 호출되야됨
    /// </summary>
    public void InitTurnData() {
        turnIndex = 0; //턴값 초기화
        //테스트 데이터 
        ITurnBaseData[] turnListTemp = TestGeneratorArraData();//테스트 데이터 생성
        Array.Sort(turnListTemp, SortComparison); //정렬

        turnObjectList = new LinkedList<ITurnBaseData>(turnListTemp);//초기화
        //여기까지 테스트 데이터생성 나중에 데이터 구조가 정해지면 파싱 작업 해야함.
        TurnStart();//턴시작
    }

    /// <summary>
    /// 턴시작할 오브젝트를 가져와서 시작함수를 호출한다.
    /// </summary>
    private void TurnStart() {
        turnIndex++; //몇턴인지 체크
        ITurnBaseData nowTurnUnit = GetCurrentTurnObject(); //처음 턴유닛을 찾아와서 
        nowTurnUnit.TurnEndAction = TurnEnd; //턴종료시 신호를 받을 델리게이트 셋팅하고

        /*
         * 적군움직일때 클릭이벤트를 제한 하거나 할때 기능추가 위치
         */
        nowTurnUnit.TurnStartAction();  //턴시작을 알린다
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
        turnEndObj.TurnEndAction = null; // 턴이꼬이는것을 방지하기위해 턴종료가 끝낫으면 델리게이터를 초기화한다

        SetTurnValue();// 턴종료시마다 리스트의 유닛들의 행동력 값을 추가해주는 기능
        
        TurnSorting(turnEndObj); // 값이 변경된 오브젝트의 정렬기능 실행

        //추가되는 값이 전부다르다는 전제하에 전체정렬을 재시도 
        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //값이변경이 됬음으로 전체 재정렬
       
        /*
         *  적군움직일때 클릭이벤트를 제한 하거나 할때 기능추가 위치
         *  배틀맵에서 승리조건을 체웠을때 기능 추가해야되는 위치
         */

        TurnStart();
    }
    

   

    /// <summary>
    /// 턴이 끝나면 순번을 재정렬 
    /// 리스트는 이미 정렬이된상태이기때문에 한쪽방향으로만 탐색하여 위치를교체한다.
    /// </summary>
    /// <param name="turnEndData">턴사용이 완료된 객체</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //비교할 노드 가져와서

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//비교할 다음노드를 미리가져온다.

        for (int i = 1; i < turnObjectList.Count; i++)//리스트 크기만큼 돌리고 
        {
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
    /// 몬스터가 죽어서 사라지거나 엔피씨가 사라지는경우 턴진행 상황에서도 사라져야함으로 리스트삭제
    /// 턴리스트에서 삭제.
    /// </summary>
    /// <param name="deleteObject">사라질 유닛</param>
    public void TurnListDeleteObj(ITurnBaseData deleteObject)
    {
        turnObjectList.Remove(deleteObject);
    }

    /// <summary>
    /// 몬스터가 리젠되거나 엔피씨가 중간에 추가될때 사용 
    /// 턴리스트에 추가될 값으로 재정렬 
    /// </summary>
    /// <param name="addObject">턴이 새롭게 추가된 객체</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        if (addObject == null) return; // 추가할값이없으면 리턴 
        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //첫번째 노드 가져와서

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

    /*======================================== 테스트용 ==================================================*/

    /// <summary>
    /// 테스트 데이터 생성
    /// </summary>
    /// <returns></returns>
    private TurnBaseObject[] TestGeneratorArraData()
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            GameObject go = new GameObject();
            go.name = $"{i} 번째 오브젝트";
            go.AddComponent<TurnBaseObject>();
            tbo[i] = go.GetComponent<TurnBaseObject>();
            //tbo[i] = new TurnBaseObject(); //컴퍼넌트상속받은 클래스는 new 를사용해서 생성을못하게 막아놨다 .
            //생성하려면 게임오브젝트를 만들고 AddComponent를 이용해야한다.
            //Debug.Log($"객체 널이냐? :{tbo[i]}");
            tbo[i].TurnWaitingValue = UnityEngine.Random.Range(0, 2000);

        }
        return tbo;
    }
    /// <summary>
    /// 턴리스트의 값확인용
    /// </summary>
    public void ViewTurnList() {
        foreach(var j in turnObjectList) {
            Debug.Log($"{j} 값 : {j.TurnActionValue}");
        }
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
        if (before.TurnActionValue < after.TurnActionValue)
        {
            return isAscending == SortComponent<ITurnBaseData>.SortType.Ascending ? -1 : 1;
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortComponent<ITurnBaseData>.SortType.Descending ? 1 : -1;
        }
        else
        {
            return 0;
        }
    }

}
