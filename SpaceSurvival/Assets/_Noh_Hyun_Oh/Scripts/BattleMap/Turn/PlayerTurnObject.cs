using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 턴 
/// </summary>
public class PlayerTurnObject : TurnBaseObject
{
    public Func<ICharcterBase[]> initPlayer;
    UICamera cam;
    protected override void Awake()
    {
        base.Awake();
        onEnable_InitData += () =>
        {
            battleIndex = -1;
            if (TurnManager.Instance != null)
            {
                //활성화 하기전에 값셋팅을 할 람다 함수
                //이러면 팩토리에서 get 할때 비활성화에서 -> 초기화 -> 활성화 가 가능하다 
                battleIndex = TurnManager.Instance.BattleIndex;
            }
        };
        //최소한으로 초기화해줘야 할맴버들
        turnAddValue = 0.06f;                       // 한턴당 회복될 수치
        maxTurnValue = 1.8f;                        // 최대로 회복될 수치
        TurnActionValue = 0.0f;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (battleIndex > -1) //배틀 인덱스가 셋팅되있으면 배틀맵임으로 배틀맵 일때만  
        {
            ICharcterBase[] playerList = initPlayer?.Invoke(); //외부에서 캐릭터배열이 들어왔는지 체크
            if (playerList == null || playerList.Length == 0) //캐릭터 초기화가 안되있으면  
            {
                //테스트 데이터 생성
                for (int i = 0; i < 3; i++)//캐릭터들 생성해서 셋팅 
                {
                    GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL).gameObject;
                    charcterList.Add(go.GetComponent<ICharcterBase>());
                    go.name = $"Player_{i}";
                    go.SetActive(true);
                }
            }
            else // 외부에서 데이터가 들어왔을경우  
            {
                foreach (ICharcterBase player in playerList)
                {
                    charcterList.Add(player); //턴관리할 캐릭터로 셋팅
                }
            }
        }
    }

}
