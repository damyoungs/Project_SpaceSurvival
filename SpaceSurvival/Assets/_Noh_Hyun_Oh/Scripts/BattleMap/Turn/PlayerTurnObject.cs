using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                bpc.onMoveActive = (tile) => {
                    go.CharcterMove(tile);
                };
                go.SetTile(SpaceSurvival_GameManager.Instance.BattleMapDoubleArray[0,0]);
            }
            
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //팀 상시 유아이 보여주기 
        }
        else // 외부에서 데이터가 들어왔을경우  이경우가 정상적인경우다  내가 데이서 셋팅안할것이기때문에...
        {
            foreach (ICharcterBase player in playerList)
            {
                charcterList.Add(player); //턴관리할 캐릭터로 셋팅
            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//팀 상시 유아이 보여주기 
        }
    }
}
