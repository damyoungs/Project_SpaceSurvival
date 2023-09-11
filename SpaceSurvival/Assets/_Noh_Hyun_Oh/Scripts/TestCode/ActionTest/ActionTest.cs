using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionTest : TestBase
{
    [SerializeField]
    [Range(0.0f,1.0f)]
    float test = 0.0f;
    [SerializeField]
    float max = 0.0f;
    [SerializeField]
    float now = 0.0f;
    TrackingBattleUI testUI;
    Action action;
    event Action testAction
    {
        add => testAction += value;         //이벤트 함수는 += -= 로만 등록 된다고 한다. 
        remove => testAction -= value;      //인풋 시스템 이 이벤트함수를 사용해서 += -= 로만 사용가능하다.
    }
    InputKeyMouse keyTest;
    protected override void Awake()
    {
        base.Awake();   
        keyTest = new();
        keyTest.Test.Enable();
        keyTest.Player.Enable();
        testUI = FindObjectOfType<TrackingBattleUI>();
    }

    private void OnMouseDown()
    {
        //action += () => { Debug.Log("중복 등록 확인용"); };                    //중복으로 등록된다.
        //action += ActionTestFunc;                                              //중복으로 등록된다.
        //testAction += () => { Debug.Log("이벤트 액션 중복등록 확인용"); };     //중복 등록된다       
        //testAction += ActionEventTestFunc;                                     //중복 등록된다
        keyTest.Test.Test1.performed += inputTest;                               //중복 등록 안된다
        keyTest.Test.Test1.performed += (_) =>                                   //중복 등록 안된다
        {
            Debug.Log("인풋 시스템 중복 등록 테스트 람다식 ");
        };
        keyTest.Player.Move.performed += (_) => { Debug.Log($"{_} : 테스트"); };   // 람다함수든 맴버함수든 같은 함수면 등록은 한번만된다 
        keyTest.Player.Move.performed += (_) => { Debug.Log($"{_} : 뭐냐"); };     // 이건 이벤트시스템의 기본매커니즘 이라고한다.
                                                                                    // 해당내용 소스코드 찾아보려고했더니 접근이안된다 돈주고사랜다.. 
        //inputSystem.Player.Enable(); //Enable 함수는 enabled변수를 체크를 안한다.. 그래서 여기서 체크를 해줘서 초기화되있을때만실행  2021.3.25f1 버전 기준                                                            
        //inputSystem.Player.Disable(); //Disable 함수는 enable값(활성화상태확인용 프로퍼티)이 false 면 아무작동도안하고 리턴한다.  2021.3.25f1 버전 기준
    }


    private void OnMouseUp()
    {
        //action.Invoke();   
        //testAction.Invoke();
    }
    private void ActionTestFunc()
    {
        Debug.Log("중복이냐?");
    }
    private void ActionEventTestFunc()
    {
        Debug.Log("이벤트 중복이냐?");
    }
    private void inputTest(InputAction.CallbackContext context)
    {
        Debug.Log("인풋시스템 중복 등록 테스트 ");
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        testUI.hpGaugeSetting(now,max);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        testUI.stmGaugeSetting(now,max);

    }
}
