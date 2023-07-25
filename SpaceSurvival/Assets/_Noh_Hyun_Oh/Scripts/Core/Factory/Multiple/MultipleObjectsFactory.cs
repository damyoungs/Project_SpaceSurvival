using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �⺻������ ������ �����Ǵ� ��ü ����
/// EnumList.MultipleFactoryObjectList �̰��� ��ü�߰��ɳ��� �����߰��Ͻø�˴ϴ�.
/// </summary>
public class MultipleObjectsFactory : ChildComponentSingeton<MultipleObjectsFactory>
{
    /// <summary>
    /// ����ȭ�鿡 ������ ������ƮǮ
    /// </summary>
    SaveDataPool saveDataPool;

    /// <summary>
    /// ����ȭ�鸮��Ʈ ��ư�� 
    /// </summary>
    SavePageButtonPool savePageButtonPool;

    /// <summary>
    /// �κ��丮 �������� ������ ĭ 
    /// </summary>
    InventoryPool inventoryPool;

    /// <summary>
    /// �������Ȳ �ѷ��� Ǯ
    /// </summary>
    TurnUnitPool turnUnitPool;

    TrackingBattleUIPool trackingBattleUIPool;

    StatePool statePool;

    BattleMapUnitPool battleMapUnitPool;
    /// <summary>
    /// ���丮 ������ �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="scene">������ �����ʿ����</param>
    /// <param name="mode">������� �����ʿ����</param>
    protected override void Init(Scene scene, LoadSceneMode mode)
    {
        saveDataPool = GetComponentInChildren<SaveDataPool>(true);
        savePageButtonPool = GetComponentInChildren<SavePageButtonPool>(true);
        inventoryPool = GetComponentInChildren<InventoryPool>(true);
        turnUnitPool = GetComponentInChildren<TurnUnitPool>(true);
        trackingBattleUIPool = GetComponentInChildren<TrackingBattleUIPool>(true);
        statePool = GetComponentInChildren<StatePool>(true);
        battleMapUnitPool = GetComponentInChildren<BattleMapUnitPool>(true);
        saveDataPool.Initialize();
        savePageButtonPool.Initialize();
        inventoryPool.Initialize(); 
        turnUnitPool.Initialize();  
        trackingBattleUIPool.Initialize();
        statePool.Initialize();
        battleMapUnitPool.Initialize();
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
            case EnumList.MultipleFactoryObjectList.INVENTORY_POOL:
                obj = inventoryPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL:
                obj = turnUnitPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL:
                obj = trackingBattleUIPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.STATE_POOL:
                obj = statePool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.BATTLEMAP_UNIT_POOL:
                obj = battleMapUnitPool?.GetObject()?.gameObject;
                break;
            default:

                break;
        }
        return obj;
    }

}


