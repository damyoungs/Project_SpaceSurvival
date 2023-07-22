using System;
using UnityEngine;
/// <summary>
/// ������ �ʿ��� Ŭ������ ���������̽��� ��ӹ޾Ƽ� SortValue ���� �����Ѵ�.
/// </summary>
public interface ITurnBaseData
{
    TrackingBattleUI BattleUI { get; set; }
    
    /// <summary>
    /// ������Ʈ�� ���ǵ� �Լ��� ����
    /// ������ UI���������� ������ ��ǥ���� �˾ƾ������� �߰�
    /// </summary>
    public Transform transform { get;}
    /// <summary>
    /// ���������� ����� ������ ��ȣ 
    /// </summary>
    public int UnitBattleIndex { get; set; }
    /// <summary>
    /// ������� �߰��� �ൿ�� 
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
