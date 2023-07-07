using System;
using UnityEngine;
/// <summary>
/// �⺻ ���̽��� TrunBaseData
/// ���� ����� ������Ʈ ����.
/// </summary>
public class TurnBaseObject : MonoBehaviour, ITurnBaseData
{
    /// <summary>
    /// ���̰���� ȸ���� �ൿ�°� 
    /// </summary>
    float turnAddValue = 100.0f;

    /// <summary>
    /// ȸ�����Ѽ�
    /// </summary>
    float maxTurnValue = 3000.0f;

    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
    /// </summary>
    float turnWaitingValue = 1000;
    public float TurnWaitingValue
    {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
            if (turnWaitingValue < 0.0f) // ���� �ּҰ��� �����ְ�
            {
                turnWaitingValue = 0.0f;
            }
            else if (turnWaitingValue > maxTurnValue) //�ִ�� ���ü��ִ°��� ��������  
            {
                turnWaitingValue = maxTurnValue;
            }
        }
    }

    /// <summary>
    /// ������ ���ذ� ������Ƽ
    /// </summary>
    public float TurnActionValue
    {
        get => turnWaitingValue;
        set => turnWaitingValue = value;
    }

    /// <summary>
    /// �ϰ���� ȸ���� �ൿ�°�
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    /// <summary>
    /// ������� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    private void Awake()
    {
        turnAddValue = UnityEngine.Random.Range(10.0f, 100.0f); //������ø��� �����Ǵ� �ൿ�°� ���� ����
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
        if (TurnActionValue > 700) TurnWaitingValue -= UnityEngine.Random.Range(300, 700);// �ൿ�� �Ҹ��� 
        Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
