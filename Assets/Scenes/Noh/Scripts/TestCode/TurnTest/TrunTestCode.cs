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
    //턴초기화 및 1턴 실행 npc면 중간에 멈춤  
    protected override void Test1(InputAction.CallbackContext context)
    {
        TurnBaseObject b = FindObjectOfType<TurnBaseObject>();
        if (b== null) {
            TurnManager.Instance.InitTurnData();
        
        }
    }
    //턴실행 
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
    //활력도 소모된후 정렬된값 출력 
    protected override void Test3(InputAction.CallbackContext context)
    {
        TurnManager.Instance.ViewTurnList();
    }
}
