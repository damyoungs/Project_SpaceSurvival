using System;
using UnityEngine;
/// <summary>
/// ������ �ʿ��� Ŭ������ ���������̽��� ��ӹ޾Ƽ� SortValue ���� �����Ѵ�.
/// </summary>
public interface ITurnBaseData
{
    /// <summary>
    /// ���������� ����� ������ ��ȣ 
    /// ��õ ���Ĺ�� : PC ���� 0~������� �ִ�� 10���� 10���� 20���� 20������� ������
    /// </summary>
    public int UnitBattleIndex { get; set; }
    /// <summary>
    /// ������� �߰��� �ൿ�� 
    /// ���� �����϶� ���̸� �α����� �ʿ��Ѱ����� ���� �������� ���� 1000������ ���� ��Ű��ȴ�.
    /// </summary>
    public float TurnEndActionValue { get; }
    /// <summary>
    /// �ൿ�� ���ĵ� ���ذ�
    /// </summary>
    public float TurnActionValue { get; set; }

    /// <summary>
    /// �ϿϷ�� �˷��� ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    /// <summary>
    /// ������ �ൿ�߿� Ư�������� �������� �޴����� ��ȣ�� �ִ� ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnRemove { get; set;} 
    
    /// <summary>
    /// �Ͻ��۽� ������ �Լ�
    /// </summary>
    public void TurnStartAction();
}
