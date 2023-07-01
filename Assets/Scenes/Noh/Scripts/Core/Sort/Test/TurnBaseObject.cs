using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
/// <summary>
/// 턴을 사용할 오브젝트
/// ISortBase 는 정렬이 필요하기때문에 추가
/// </summary>
public class TurnBaseObject : MonoBehaviour, ISortBase
{
    /// <summary>
    /// 현재 턴의 진행값 
    /// 이값으로 돌아올순번을 정한다.
    /// </summary>
    double turnWaitingValue = 0;
    public double TurnWaitingValue {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
        }
    }

    /// <summary>
    /// 정렬할 기준값
    /// </summary>
    public double SortValue => turnWaitingValue;

}
