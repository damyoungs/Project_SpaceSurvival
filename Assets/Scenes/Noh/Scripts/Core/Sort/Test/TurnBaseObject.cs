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
    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
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
    /// ������ ���ذ�
    /// </summary>
    public double SortValue => turnWaitingValue;

}
