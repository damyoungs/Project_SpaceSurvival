using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrunTestCode : TestBase
{
    TurnBaseObject[] list;
    //���ʱ�ȭ �� 1�� ���� npc�� �߰��� ����  
    protected override void Test1(InputAction.CallbackContext context)
    {
        TurnBaseObject b = FindObjectOfType<TurnBaseObject>();
        if (b== null) {
            TurnManager.Instance.InitTurnData();
        
        }
    }
    //�Ͻ��� 
    protected override void Test2(InputAction.CallbackContext context)
    {
        list = FindObjectsOfType<TurnBaseObject>();
        foreach (var obj in list)
        {
            if (obj.TurnEndAction != null) {
                obj.TurnEndAction?.Invoke(obj);
                break;
            }

        }
    }
    //Ȱ�µ� �Ҹ���� ���ĵȰ� ��� 
    protected override void Test3(InputAction.CallbackContext context)
    {
        TurnManager.Instance.ViewTurnList();
    }
}
