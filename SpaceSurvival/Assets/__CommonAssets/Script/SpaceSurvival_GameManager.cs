using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임에서 필요한 데이터 및 공통된 기능을 담을 메니저 클래스 
/// </summary>
public class SpaceSurvival_GameManager : Singleton<SpaceSurvival_GameManager>
{
    /// <summary>
    /// 배틀맵 시작시 셋팅할 맵의 타일 변수 
    /// </summary>
    Tile[] battleMap;
    public Tile[] BattleMap 
    {
        get 
        {
            //if (battleMap == null) //배틀맵의 값이없으면 
            //{
            //    battleMap = GetBattleMapTilesData?.Invoke(); //델리게이트 요청해 값을 받아오도록한다.
            //    //배틀맵이 아닌경우 델리게이트가 값을 셋팅못하니 null 이 셋팅 될수도있다.
            //}
            battleMap ??= GetBattleMapTilesData?.Invoke(); //위의 주석과 같은 내용이라고 한다 . (복합형)
            return battleMap;

        }
    }
    /// <summary>
    /// 배틀맵 시작시 셋팅할 맵의 타일 변수 
    /// </summary>
    Tile[,] battleMapDoubleArray;
    public Tile[,] BattleMapDoubleArray
    {
        get
        {
            //if (battleMapDoubleArray == null) //배틀맵의 값이없으면 
            //{
            //    battleMapDoubleArray = GetBattleMapTilesDataDoubleArray?.Invoke(); //델리게이트 요청해 값을 받아오도록한다.
            //    //배틀맵이 아닌경우 델리게이트가 값을 셋팅못하니 null 이 셋팅 될수도있다.
            //}
            battleMapDoubleArray ??= GetBattleMapTilesDataDoubleArray?.Invoke(); //위의 주석과 같은 내용이라고 한다 . (복합형)
            Debug.Log(battleMapDoubleArray);
            Debug.Log(battleMapDoubleArray.Length);
            return battleMapDoubleArray;

        }
    }


    /// <summary>
    /// 플레이어의 팀원 목록을 저장해둔다.
    /// </summary>
    ICharcterBase[] playerTeam;
    public ICharcterBase[] PlayerTeam 
    {
        get 
        {
            //if (playerTeam == null) //팀목록이 없으면 
            //{
            //    playerTeam = GetPlayerTeam?.Invoke(); // 델리를 요청해서 받아온다
            //}
            playerTeam ??= GetPlayerTeam?.Invoke(); // 위의 주석 내용과 같음(복합형)
            return playerTeam;
        }
    }
    /// <summary>
    /// 플레이어의 팀원 목록을 받아오기위한 Func 델리게이트 
    /// 이건 아직 데이터새성이 어떻게될지모르니 일단 틀만 잡아놓고 나중에 수정.
    /// </summary>
    public Func<ICharcterBase[]> GetPlayerTeam;

    /// <summary>
    /// 배틀맵 데이터를 받아오기위한 Func 델리게이트
    /// </summary>
    public Func<Tile[]> GetBattleMapTilesData;

    /// <summary>
    /// 배틀맵 데이터를 받아오기위한 Func 델리게이트
    /// </summary>
    public Func<Tile[,]> GetBattleMapTilesDataDoubleArray;

    /*
     인벤토리는 하나만 사용할경우 여기에 추가가 필요하다 .
     */

    /// <summary>
    /// 이동범위 표시하는 컴포넌트 가져온다.
    /// </summary>
    MoveRange moveRange;
    public MoveRange MoveRange 
    {
        get 
        {
            if (moveRange == null) 
            {
                moveRange = GetMoveRangeComp?.Invoke();
            }
            return moveRange;
        }
    
    }
    /// <summary>
    /// 이동 범위표시하는 로직 받아오기위한 델리게이트
    /// </summary>
    public Func<MoveRange> GetMoveRangeComp;
 
    
}
