using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 클릭이벤트를 막기위해 전체화면으로 덮어버리고 단축키 이벤트를 끄고 esc이벤트만 추가한다.
/// </summary>
public class DefenceEvent: MonoBehaviour
{
    
    InputKeyMouse inputSystem ;
    SaveLoadPopupButton targetWindow;

    private void Awake()
    {
        inputSystem = new InputKeyMouse();//esc이벤트 추가할 컨트롤러
        int index = transform.parent.childCount - 1;
        targetWindow = transform.parent.GetChild(index).GetChild(5).GetComponent<SaveLoadPopupButton>();// esc눌렀을때 처리할 종료 process 실행클래스 
    }
    private void OnEnable()
    {
        WindowList.Instance.InputKeyEvent.Disable();// 단축키 비활성화
        inputSystem.Enable();
        inputSystem.KeyBorad.System.performed += Close;
    }


    private void OnDisable()
    {

        inputSystem.KeyBorad.System.performed -= Close;
        inputSystem.Disable();

        WindowList.Instance.InputKeyEvent.Enable(); //단축키 활성화
    }
    /// <summary>
    /// esc 눌렀을때 팝업창닫기로직실행
    /// </summary>
    private void Close(InputAction.CallbackContext context)
    {
        targetWindow.CancelButton();//닫기창누르는것과 같다 
    }
}
