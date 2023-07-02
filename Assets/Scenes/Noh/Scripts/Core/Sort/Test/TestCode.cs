
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : TestBase
{
    /// <summary>
    /// ���ľ˰��� ����
    /// </summary>
    [SerializeField]
    SortComponent<TurnBaseObject>.SortProcessType sortProccessType;
    /// <summary>
    /// �������� �������� Ÿ��
    /// </summary>
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
            Debug.Log($"�迭�� [{n}]��° ��:{sortData[n].TurnWaitingValue}");
        }
    }
    private void TestCheckValues(List<TurnBaseObject> sortData)
    {
        for (int n = 0; n < sortData.Count; n++)
        {
            Debug.Log($"�迭�� [{n}]��° ��:{sortData[n].TurnWaitingValue}");
        }
    }
    private TurnBaseObject[] GeneratorArraData()
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<TurnBaseObject>();
            tbo[i] = go.GetComponent<TurnBaseObject>();
            //tbo[i] = new TurnBaseObject(); //���۳�Ʈ��ӹ��� Ŭ������ new ������ؼ� ���������ϰ� ���Ƴ��� .
            //�����Ϸ��� ���ӿ�����Ʈ�� ����� AddComponent�� �̿��ؾ��Ѵ�.
            //Debug.Log($"��ü ���̳�? :{tbo[i]}");
            tbo[i].TurnWaitingValue = UnityEngine.Random.Range(-200, 200);

        }
        return tbo;
    }
    //================ array �� list ���� �⺻������� ��� �׽�Ʈ=====================//
    /// <summary>
    /// ���ı���
    /// �������� = true  �������� = false
    /// </summary>
    public bool isAsc = false;
    protected override void Test3(InputAction.CallbackContext context)
    {
        List<TurnBaseObject> tboList = new List<TurnBaseObject>(GeneratorArraData());
        TestCheckValues(tboList);
        tboList.Sort(SortComparison);//����Ʈ ����
        TestCheckValues(tboList);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        TurnBaseObject[] tbo = GeneratorArraData();
        TestCheckValues(tbo);
        Array.Sort(tbo, SortComparison); // ��� ����
        TestCheckValues(tbo);


    }
    /// <summary>
    /// ����Ʈ�� Array �� �⺻���ı���� �̿��� ���� 
    /// ���ڰ��� �ڷ����� ��������Ѵ� ��ȯ���� int -1 0 1
    /// </summary>
    /// <param name="before">���ǰ�</param>
    /// <param name="after">���ǰ�</param>
    /// <returns>�� ���</returns>
    private int SortComparison(TurnBaseObject before, TurnBaseObject after)
    {
        if (before.SortValue < after.SortValue)
        {
            return isAsc ? -1 : 1;
        }
        else if (before.SortValue > after.SortValue)
        {
            return isAsc ? 1 : -1;
        }
        else
        {
            return 0;
        }
    }
}