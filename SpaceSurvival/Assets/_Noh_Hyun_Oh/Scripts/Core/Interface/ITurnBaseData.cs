using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 턴 메니저에서 턴진행상황을 처리하기위해 사용하는 인터페이스 
/// 팀별 : 아군 n개 , 적군 n 개  가능  
/// 
/// </summary>
public interface ITurnBaseData
{
    GameObject gameObject { get; }
    /// <summary>
    /// 턴게이지 UI 캐싱용 프로퍼티
    /// </summary>
    TurnGaugeUnit GaugeUnit { get; }
    /// <summary>
    /// 컴포넌트에 정의된 함수를 연결
    /// 추적형 UI적용을위해 유닛의 좌표값을 알아야함으로 추가
    /// </summary>
    public Transform transform { get; }
    /// <summary>
    /// 전투씬에서 턴관리에 사용할 번호 
    /// </summary>
    public int UnitBattleIndex { get;}
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
    public Action<ITurnBaseData> TurnRemove { get; set; }

    /// <summary>
    /// 해당턴오브젝트에서 사용할 캐릭터 리스트
    /// </summary>
    public List<ICharcterBase> CharcterList { get; }

    /// <summary>
    /// 턴시작시 실행할 함수
    /// </summary>
    public void TurnStartAction();

    /// <summary>
    /// 턴유닛이 사라질때 초기화할 함수
    /// </summary>
    public void ResetData();
}
