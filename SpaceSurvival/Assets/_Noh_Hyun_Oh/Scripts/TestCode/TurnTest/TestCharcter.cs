using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCharcter : TestBase, ITurnBaseData
{
    int index;
    public int UnitBattleIndex { get => index; set => index=value; }

    float actionAddValue = 0.5f;
    public float TurnEndActionValue => actionAddValue;

    float actionValue = 0.0f;
    public float TurnActionValue { get => actionValue; set => actionValue = value; }
    Action<ITurnBaseData> turnEnd;

    public Action<ITurnBaseData> TurnEndAction { get => turnEnd; set => turnEnd = value; }
    Action<ITurnBaseData> remove;
    public Action<ITurnBaseData> TurnRemove { get => remove; set => remove = value; }

    public TrackingBattleUI BattleUI 
    {
        get => battleUI;
        set => battleUI = value;
    } 
    

    TrackingBattleUI battleUI;

    TestCharcter target;
    protected override void Awake()
    {
        base.Awake();
        turnEnd += turnEnd;
    }

    private void OnEnable()
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL);
        battleUI = obj.GetComponent<TrackingBattleUI>();
        battleUI.Player = this;
        battleUI.gameObject.SetActive(true);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        TurnEnd();
    }
    public void TurnStartAction()
    {

    }
    private ITurnBaseData TurnEnd() 
    {
        return this;
    }


  

}
