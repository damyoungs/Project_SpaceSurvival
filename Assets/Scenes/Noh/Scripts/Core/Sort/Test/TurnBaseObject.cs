using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
/// <summary>
/// ���� ����� ������Ʈ
/// ISortBase �� ������ �ʿ��ϱ⶧���� �߰�
/// </summary>
public class TurnBaseObject : MonoBehaviour, ISortBase
{
    double usingValue = 0;
    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
    /// </summary>
    float turnWaitingValue = 0;
    public float TurnWaitingValue {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
        }
    }

    /// <summary>
    /// ������ ���ذ�
    /// </summary>
    public float SortValue { get => turnWaitingValue; set => turnWaitingValue = value; }

    public double UseTurnValue => usingValue;


}
