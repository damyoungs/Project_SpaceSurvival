using System;
using UnityEngine;
/// <summary>
/// 정렬이 필요한 클래스는 이인터페이스를 상속받아서 SortValue 값을 셋팅한다.
/// </summary>
public interface ITurnBaseData
{
    TrackingBattleUI BattleUI { get; set; }
    
    /// <summary>
    /// 컴포넌트에 정의된 함수를 연결
    /// 추적형 UI적용을위해 유닛의 좌표값을 알아야함으로 추가
    /// </summary>
    public Transform transform { get;}
    /// <summary>
    /// 전투씬에서 사용할 유닛의 번호 
    /// </summary>
    public int UnitBattleIndex { get; set; }
    /// <summary>
    /// 턴종료시 추가될 행동력 
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
