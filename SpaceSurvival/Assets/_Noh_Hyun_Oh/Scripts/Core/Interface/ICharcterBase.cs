using System;
using UnityEngine;
/// <summary>
/// 배틀 맵에서 유닛들이 가지고 있어야할 인터페이스 필요하면 추가예정
/// </summary>
public interface ICharcterBase 
{
    public Transform transform { get; }
    /// <summary>
    /// 추적형 UI 캐싱용 프로퍼티
    /// </summary>
    TrackingBattleUI BattleUI { get; set; }

    /// <summary>
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform BattleUICanvas { get;  }

    /// <summary>
    /// 현재 캐릭터가 있는 타일 
    /// </summary>
    Tile CurrentTile { get; }

   
    /// <summary>
    /// 턴유닛이 사라질때 초기화할 함수
    /// </summary>
    public void ResetData();
}
