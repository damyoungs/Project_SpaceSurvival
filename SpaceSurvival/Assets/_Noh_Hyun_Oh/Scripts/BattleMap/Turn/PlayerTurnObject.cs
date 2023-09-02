using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 턴 
/// </summary>
public class PlayerTurnObject : TurnBaseObject 
{
   
    /// <summary>
    /// 테스트용 변수 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 1;

    [SerializeField]
    BattleMap_Player_Controller bpc;
    
    
    /// <summary>
    /// 캐릭터 데이터는 외부에서 셋팅하기때문에 해당 델리게이트 연결해줘야함
    /// </summary>
    public Func<ICharcterBase[]> initPlayer;


    
    /// <summary>
    /// 데이터 초기화 함수
    /// </summary>
    public override void InitData() 
    {
        bpc = FindObjectOfType<BattleMap_Player_Controller>();
        bpc.onClickMonster += OnClickUnit;
        TurnActionValue = 0.0f; //액션값 초기화 
        ICharcterBase[] playerList = initPlayer?.Invoke(); //외부에서 캐릭터배열이 들어왔는지 체크
        if (playerList == null || playerList.Length == 0) //캐릭터 초기화가 안되있으면 
        {
            //테스트 데이터 생성
            for (int i = 0; i < testPlayerLength; i++)//캐릭터들 생성해서 셋팅 
            {
                BattleMapPlayerBase go = (BattleMapPlayerBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL);
                charcterList.Add(go);
                go.name = $"Player_{i}";
                bpc.onMoveActive += (tile) => {
                    if (isTurn && go.isControll) //현재 턴인상태고 , 캐릭터의 컨트롤이 잡혀있을때만  
                    {
                        go.CharcterMove(tile); //이동한다.
                    }
                };
                go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster));
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
            }
            
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //팀 상시 유아이 보여주기 
        }
        else // 외부에서 데이터가 들어왔을경우  이경우가 정상적인경우다  내가 데이서 셋팅안할것이기때문에...
        {
            foreach (ICharcterBase player in playerList)
            {
                charcterList.Add(player); //턴관리할 캐릭터로 셋팅
                player.GetCurrentTile = () => SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster); //타일 셋팅연결
                player.transform.position = player.CurrentTile.transform.position;//셋팅된 타일위치로 이동시킨다.
            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//팀 상시 유아이 보여주기 
        }
    }
    public override void TurnStartAction()
    {
        isTurn = true;
        Debug.Log($"{name} 오브젝트는 턴이 시작되었다");
    }

    /// <summary>
    /// 턴일경우 클릭이벤트 처리 
    /// </summary>
    /// <param name="tile"></param>
    public void OnClickUnit(Tile tile)
    {
        if (isTurn) //턴일경우만 작동한다.
        {
            ///아군 을 클릭했을때 사용할수있다 . 지금은 몬스터도 같이있어서 안됨 
            foreach (ICharcterBase playerUnit in charcterList) //플레이어 뒤져서 
            {
                if (tile.width == playerUnit.CurrentTile.width &&
                    tile.length == playerUnit.CurrentTile.length) //클릭한 타일이 플레이어 유닛 위치면 
                {
                    currentUnit = playerUnit; //커런트 셋팅하고 빠져나간다
                    BattleMapPlayerBase unit = (BattleMapPlayerBase)playerUnit;
                    unit.isControll = true;
                    break;
                }
                else 
                {
                    currentUnit = playerUnit; //커런트 셋팅하고 빠져나간다
                    BattleMapPlayerBase unit = (BattleMapPlayerBase)playerUnit;
                    unit.isControll = false;
                }
            }

            Debug.Log($"{currentUnit} 이게 선택한유닛 ");
            if (currentUnit != null) //행동 유닛이 선택 되있을경우 실행  
            {
                
            }

        }
    }
}
