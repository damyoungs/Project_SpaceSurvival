using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 기본적으로 복수로 생성되는 객체 정의
/// EnumList.MultipleFactoryObjectList 이곳에 객체추가될내용 같이추가하시면됩니다.
/// </summary>
public class Multiple_Factory : ChildComponentSingeton<Multiple_Factory>
{
    /// <summary>
    /// 저장화면에 보여질 오브젝트풀
    /// </summary>
    Pool_SaveData saveDataPool;

    /// <summary>
    /// 저장화면리스트 버튼들 
    /// </summary>
    Pool_SavePageButton savePageButtonPool;

    /// <summary>
    /// 턴진행상황 뿌려줄 풀
    /// </summary>
    Pool_TurnGaugeUnit turnGaugeUnitPool;

    /// <summary>
    /// 추적형 UI 생성 풀
    /// </summary>
    Pool_TrackingBattleUI trackingBattleUIPool;

    /// <summary>
    /// 상태 UI 생성 풀
    /// </summary>
    Pool_State statePool;

    /// <summary>
    /// 턴관리될 용 오브젝트 생성 풀
    /// </summary>
    Pool_BattleMapTurnUnit battleMapPlayerPool;

    /// <summary>
    /// 턴관리될 용 오브젝트 생성 풀
    /// </summary>
    Pool_BattleMapTurnUnit battleMapEnemyPool;

   

    Pool_PlayerUnit playerUnitPool;
    Pool_EnemyUnit enemyUnitPool;

    /// <summary>
    /// 팩토리 생성시 초기화 함수
    /// </summary>
    /// <param name="scene">씬정보 딱히필요없음</param>
    /// <param name="mode">모드정보 딱히필요없음</param>
    protected override void Init(Scene scene, LoadSceneMode mode)
    {
        saveDataPool = GetComponentInChildren<Pool_SaveData>(true);
        savePageButtonPool = GetComponentInChildren<Pool_SavePageButton>(true);
        turnGaugeUnitPool = GetComponentInChildren<Pool_TurnGaugeUnit>(true);
        trackingBattleUIPool = GetComponentInChildren<Pool_TrackingBattleUI>(true);
        statePool = GetComponentInChildren<Pool_State>(true);
        playerUnitPool = GetComponentInChildren<Pool_PlayerUnit>(true);
        enemyUnitPool = GetComponentInChildren<Pool_EnemyUnit>(true);
        saveDataPool.Initialize();
        savePageButtonPool.Initialize();
        turnGaugeUnitPool.Initialize();  
        trackingBattleUIPool.Initialize();
        statePool.Initialize();
        playerUnitPool.Initialize();
        enemyUnitPool.Initialize();


        Pool_BattleMapTurnUnit[] battleTurns = GetComponentsInChildren<Pool_BattleMapTurnUnit>(true);
        
        battleMapPlayerPool = battleTurns[0];
        battleMapPlayerPool.Initialize();
        
        battleMapEnemyPool = battleTurns[1];
        battleMapEnemyPool.Initialize();

    }

    /// <summary>
    /// 객체 생성하기
    /// </summary>
    /// <param name="type">객체종류</param>
    /// <returns>생성된 객체</returns>
    public GameObject GetObject(EnumList.MultipleFactoryObjectList type)
    {
        GameObject obj = null;
        switch (type)
        {
            case EnumList.MultipleFactoryObjectList.SAVE_DATA_POOL:
                obj = saveDataPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.SAVE_PAGE_BUTTON_POOL:
                obj = savePageButtonPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL:
                obj = turnGaugeUnitPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL:
                obj = trackingBattleUIPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.STATE_POOL:
                obj = statePool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL:
                obj = battleMapPlayerPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL:
                obj = battleMapEnemyPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL:
                obj = playerUnitPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL:
                obj = enemyUnitPool?.GetObject()?.gameObject;
                break;
            default:

                break;
        }
        return obj;
    }

}


