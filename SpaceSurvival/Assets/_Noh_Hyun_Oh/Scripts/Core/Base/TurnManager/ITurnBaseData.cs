using System;
using UnityEngine;
/// <summary>
/// 정렬이 필요한 클래스는 이인터페이스를 상속받아서 SortValue 값을 셋팅한다.
/// </summary>
public interface ITurnBaseData
{
    /// <summary>
    /// 전투씬에서 사용할 유닛의 번호 
    /// 추천 정렬방법 : PC 유닛 0~순서대로 최대로 10개면 10까지 20개면 20까지잡고 그이후
    /// </summary>
    public int UnitBattleIndex { get; set; }
    /// <summary>
    /// 턴종료시 추가될 행동력 
    /// 개별 턴제일때 차이를 두기위해 필요한값으로 팀별 턴제에는 값을 1000정도로 통일 시키면된다.
    /// </summary>
    public float TurnEndActionValue { get; }
    /// <summary>
    /// 행동력 정렬될 기준값
    /// </summary>
    public float TurnActionValue { get; set; }

    /// <summary>
    /// 턴완료시 알려줄 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    /// <summary>
    /// 유닛이 행동중에 특정유닛이 사라질경우 메니져에 신호를 주는 델리게이트
    /// </summary>
    public Action<ITurnBaseData> TurnRemove { get; set;} 
    
    /// <summary>
    /// 턴시작시 실행할 함수
    /// </summary>
    public void TurnStartAction();
}
