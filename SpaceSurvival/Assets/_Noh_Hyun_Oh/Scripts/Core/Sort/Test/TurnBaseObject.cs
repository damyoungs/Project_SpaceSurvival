using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// �⺻ ���̽��� TrunBaseData
/// ���� ����� ������Ʈ ����.
/// </summary>
public class TurnBaseObject : MonoBehaviour, ITurnBaseData
{
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }
    /// <summary>
    /// ���̰���� ȸ���� �ൿ�°� 
    /// </summary>
    float turnAddValue = 0.50f;

    /// <summary>
    /// ȸ�����Ѽ�
    /// </summary>
    float maxTurnValue = 10.0f;

    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
    /// </summary>
    float turnWaitingValue = 1.0f;
    public float TurnWaitingValue {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
            if (turnWaitingValue < 0.0f) // ���� �ּҰ��� �����ְ�
            {
                turnWaitingValue = 0.0f; 
            } else if (turnWaitingValue > maxTurnValue) //�ִ�� ���ü��ִ°��� ��������  
            {
                turnWaitingValue = maxTurnValue;
            }
        }
    }

    /// <summary>
    /// ������ ���ذ� ������Ƽ
    /// </summary>
    public float TurnActionValue { 
            get => TurnWaitingValue; 
            set => TurnWaitingValue = value; 
    }

    /// <summary>
    /// �ϰ���� ȸ���� �ൿ�°�
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    /// <summary>
    /// ������� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }

    private void Awake()
    {
        turnAddValue = UnityEngine.Random.Range(0.0f ,0.1f); //������ø��� �����Ǵ� �ൿ�°� ���� ����
    }

    /// <summary>
    /// �� ���۽� ������ �Լ� 
    /// </summary>
    public void TurnStartAction()
    {
        /*
         ���⿡ ��ɱ��� PC�� �׼� �������� Ȱ��ȭ�Ͽ� �̵� ,���� , ��ų �� �׼��� ���Ҽ��ְ� �ٲٰ� 
         NPC �� ���⿡ �ڵ������� �����Ͽ� �������� TrunEndAction?.Invoke(this); �� �����Ͽ� �ϸ޴����� ������� �ѱ��.
         
         */
        if(TurnActionValue > 0.7f)TurnWaitingValue -= UnityEngine.Random.Range(0.3f, 0.7f);// �ൿ�� �Ҹ��� 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
