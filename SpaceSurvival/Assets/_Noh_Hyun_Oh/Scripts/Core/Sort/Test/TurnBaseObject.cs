using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 기본 베이스는 TrunBaseData
/// 턴을 사용할 오브젝트 예제.
/// </summary>
public class TurnBaseObject : MonoBehaviour, ITurnBaseData
{
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }
    /// <summary>
    /// 턴이경과시 회복될 행동력값 
    /// </summary>
    float turnAddValue = 0.50f;

    /// <summary>
    /// 회복상한선
    /// </summary>
    float maxTurnValue = 10.0f;

    /// <summary>
    /// 현재 턴의 진행값 
    /// 이값으로 돌아올순번을 정한다.
    /// </summary>
    float turnWaitingValue = 1.0f;
    public float TurnWaitingValue {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
            if (turnWaitingValue < 0.0f) // 값의 최소값을 정해주고
            {
                turnWaitingValue = 0.0f; 
            } else if (turnWaitingValue > maxTurnValue) //최대로 들어올수있는값도 정해주자  
            {
                turnWaitingValue = maxTurnValue;
            }
        }
    }

    /// <summary>
    /// 정렬할 기준값 프로퍼티
    /// </summary>
    public float TurnActionValue { 
            get => TurnWaitingValue; 
            set => TurnWaitingValue = value; 
    }

    /// <summary>
    /// 턴경과시 회복될 행동력값
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    /// <summary>
    /// 턴종료시 실행할 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }

    private void Awake()
    {
        turnAddValue = UnityEngine.Random.Range(0.0f ,0.1f); //턴진행시마다 증가되는 행동력값 랜덤 설정
    }

    /// <summary>
    /// 턴 시작시 실행할 함수 
    /// </summary>
    public void TurnStartAction()
    {
        /*
         여기에 기능구현 PC면 액션 아이콘을 활성화하여 이동 ,공격 , 스킬 등 액션을 취할수있게 바꾸고 
         NPC 면 여기에 자동로직을 구성하여 마지막에 TrunEndAction?.Invoke(this); 를 실행하여 턴메니져로 제어권을 넘긴다.
         
         */
        if(TurnActionValue > 0.7f)TurnWaitingValue -= UnityEngine.Random.Range(0.3f, 0.7f);// 행동력 소모후 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
