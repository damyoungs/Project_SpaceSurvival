
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : TestBase
{
    [SerializeField]
    SortComponent<TurnBaseObject>.SortProcessType sortProccessType;
    [SerializeField]
    SortComponent<TurnBaseObject>.SortType sortType;
    protected override void Test1(InputAction.CallbackContext context)
    {
        
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            tbo[i] = new TurnBaseObject();
            tbo[i].TurnWaitingValue = i * UnityEngine.Random.Range(-20, 20);
        }
        TestCheckValues(tbo);
        SortComponent<TurnBaseObject>.SorttingData(tbo, sortProccessType,sortType);
        TestCheckValues(tbo);

    }

    private void TestCheckValues(TurnBaseObject[] sortData)
    {
        for (int n = 0; n < sortData.Length; n++)
        {
            Debug.Log($"배열값 [{n}]번째 값:{sortData[n].TurnWaitingValue}");
        }
    }

}