
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class TestCode : TestBase
{
    [SerializeField]
    SortComponent<TurnBaseObject>.SortProccessType sortProccessType; // 정렬 알고리즘 종류
    [SerializeField]
    SortComponent<TurnBaseObject>.SortType sortType; // 오름차순 내림차순 

    private void TestCheckValues(TurnBaseObject[] sortData)
    {
        for (int n = 0; n < sortData.Length; n++)
        {
            Debug.Log($"배열값 [{n}]번째 값:{sortData[n].TurnWaitingValue}");
        }
    }
    private void TestCheckValues(List<TurnBaseObject> sortData)
    {
        for (int n = 0; n < sortData.Count; n++)
        {
            Debug.Log($"배열값 [{n}]번째 값:{sortData[n].TurnWaitingValue}");
        }
    }
    private void TestCheckValues(LinkedList<TurnBaseObject> sortData)
    {
        int n = 0;
        foreach (TurnBaseObject t in sortData)
        {
            Debug.Log($"링크드리스크  [{n}]번째 값:{t.TurnWaitingValue}");
            n++;
        }
    }
    private TurnBaseObject[] GeneratorArraData()
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            tbo[i] = new TurnBaseObject();
            //컴퍼넌트상속받은 클래스는 new 를사용해서 생성을 하면 null값이 들어간다. 그래서 LinkedList<TurnBaseObject> 의 Find() 함수를 사용못한다.
            //GameObject go = new GameObject();
            //go.AddComponent<TurnBaseObject>();
            //tbo[i] = go.GetComponent<TurnBaseObject>();
            //사용하려면 게임오브젝트를 만들고 AddComponent를 이용해야한다.
            //이러면 하이라키창에 게임오브젝트가 쭈욱생성되는게 문제일거같았지만
            //어짜피 캐릭터나 몬스터같은 게임오브젝트있는것들을 받아오기때문에 링크드 리스크 사용하는건 문제 없을거같다. 


            //Debug.Log($"객체 널이냐? :{tbo[i]}");
            tbo[i].TurnWaitingValue = UnityEngine.Random.Range(-200, 200);

        }
        return tbo;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {

        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            tbo[i] = new TurnBaseObject();
            tbo[i].TurnWaitingValue = i * UnityEngine.Random.Range(-20, 20);
        }
        TestCheckValues(tbo);
        SortComponent<TurnBaseObject>.SorttingData(tbo, sortProccessType, sortType);//정렬알고리즘정의해둔 클래스의 함수호출
        TestCheckValues(tbo);

    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            tbo[i] = new TurnBaseObject();
            tbo[i].TurnWaitingValue = i * UnityEngine.Random.Range(-20, 20);
        }
        List<TurnBaseObject> ltbo = new List<TurnBaseObject>(tbo);
        TestCheckValues(ltbo);
        SortComponent<TurnBaseObject>.SorttingData(ltbo, sortProccessType, sortType);
        TestCheckValues(ltbo);

    }


   
 



    //================ array 와 list 정렬 기본기능제공=====================//
    /// <summary>
    /// 정렬기준
    /// 오름차순 = true  내림차순 = false
    /// </summary>
    public bool isAsc = false;
    protected override void Test3(InputAction.CallbackContext context)
    {
        List<TurnBaseObject> tboList = new List<TurnBaseObject>(GeneratorArraData());
        TestCheckValues(tboList);
        tboList.Sort(SortComparison);//리스트 정렬
        TestCheckValues(tboList);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        TurnBaseObject[] tbo = GeneratorArraData();
        TestCheckValues(tbo);
        Array.Sort(tbo,SortComparison); // 어레이 정렬
        TestCheckValues(tbo);


    }
    /// <summary>
    /// 리스트와 Array 의 기본정렬기능을 이용한 정렬 
    /// 인자값의 자료형은 맞춰줘야하고 반환값은 int -1 0 1 으로 맞춰줘야한다.
    /// </summary>
    /// <param name="before">앞의값</param>
    /// <param name="after">뒤의값</param>
    /// <returns>비교 결과</returns>
    private int SortComparison(TurnBaseObject before , TurnBaseObject after) {
        if (before.SortValue < after.SortValue) 
        {
            return isAsc ?  -1 : 1; 
        }
        else if (before.SortValue > after.SortValue)
        {
            return isAsc ?  1 : -1;
        }
        else 
        {
            return 0;
        }
    }
}