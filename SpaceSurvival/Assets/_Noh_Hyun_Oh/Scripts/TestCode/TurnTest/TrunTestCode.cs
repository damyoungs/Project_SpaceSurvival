using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrunTestCode : TestBase
{
    TurnManager turnManager;
    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start()
    {
        turnManager = TurnManager.Instance;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        LinkedListNode<ITurnBaseData> node = turnManager.GetNode();

        node.Value.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f);

        node.Value.TurnEndAction(node.Value);
        Debug.Log($"{node.Value.UnitBattleIndex}��° ���� : �� :{node.Value.TurnActionValue}");
    }
    //�Ͻ��� 
    protected override void Test2(InputAction.CallbackContext context)
    {
        turnManager.ViewTurnList();
    }
    //Ȱ�µ� �Ҹ���� ���ĵȰ� ��� 
    protected override void Test3(InputAction.CallbackContext context)
    {
       
    }
}
