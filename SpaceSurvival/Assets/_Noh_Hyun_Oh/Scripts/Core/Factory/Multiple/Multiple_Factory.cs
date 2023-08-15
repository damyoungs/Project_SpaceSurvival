using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �⺻������ ������ �����Ǵ� ��ü ����
/// EnumList.MultipleFactoryObjectList �̰��� ��ü�߰��ɳ��� �����߰��Ͻø�˴ϴ�.
/// </summary>
public class Multiple_Factory : ChildComponentSingeton<Multiple_Factory>
{
    /// <summary>
    /// ����ȭ�鿡 ������ ������ƮǮ
    /// </summary>
    Pool_SaveData saveDataPool;

    /// <summary>
    /// ����ȭ�鸮��Ʈ ��ư�� 
    /// </summary>
    Pool_SavePageButton savePageButtonPool;

    /// <summary>
    /// �������Ȳ �ѷ��� Ǯ
    /// </summary>
    Pool_TurnGaugeUnit turnGaugeUnitPool;

    /// <summary>
    /// ������ UI ���� Ǯ
    /// </summary>
    Pool_TrackingBattleUI trackingBattleUIPool;

    /// <summary>
    /// ���� UI ���� Ǯ
    /// </summary>
    Pool_State statePool;

    /// <summary>
    /// �ϰ����� �� ������Ʈ ���� Ǯ
    /// </summary>
    Pool_BattleMapTurnUnit battleMapPlayerPool;

    /// <summary>
    /// �ϰ����� �� ������Ʈ ���� Ǯ
    /// </summary>
    Pool_BattleMapTurnUnit battleMapEnemyPool;

   

    Pool_PlayerUnit playerUnitPool;
    Pool_EnemyUnit enemyUnitPool;

    /// <summary>
    /// ���丮 ������ �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="scene">������ �����ʿ����</param>
    /// <param name="mode">������� �����ʿ����</param>
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
    /// ��ü �����ϱ�
    /// </summary>
    /// <param name="type">��ü����</param>
    /// <returns>������ ��ü</returns>
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


