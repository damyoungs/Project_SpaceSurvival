using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class EnemyTurnObject : TurnBaseObject
{
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
        if (UnitBattleIndex > -1) //배틀 인덱스가 셋팅되있으면 배틀맵임으로 배틀맵 일때만  
        {
            for (int i = 0; i < 3; i++)//캐릭터들 생성해서 셋팅 
            {
                GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL).gameObject;
                charcterList.Add(go.GetComponent<ICharcterBase>());
                go.name = $"Enemy_{i}";
                go.SetActive(true);
            }
        }
    }
}
