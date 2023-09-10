using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// 단축키 이벤트 컨트롤할 이넘 설정 
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                   // 처음시작시 기본값 : 오프닝, 타이틀, 로딩창 등 입력이 되면안되는곳에 사용될 플래그 
    Use_BattleMap = 1,              // 배틀맵 처럼 전투상황에 사용될 플래그
    Use_TownMap = 2,                // 마을 처럼 전투가없는 상황일경우 사용될 플래그                   
    Use_OptionView = 4,             // 옵션창이 열렸을때 체크할 플래그  - 설정이나 저장시 인벤창 같이 내부 설정이 변경될만한게 열리면안되니 체크필요
    Cancel_BattleMap = 254,
    Cancel_TownMap = 253,
    Cancel_OptionView = 251,

}

public class InputSystemController : ChildComponentSingeton<InputSystemController>
{

    /// <summary>
    /// 전체적으로 인풋시스템을 관리할 변수 
    /// </summary>
    InputKeyMouse inputSystem;
    static public InputKeyMouse InputSystem => Instance.inputSystem;
    /// <summary>
    /// 현재 컨트롤할 내용을 판단할 이넘값
    /// </summary>
    [SerializeField]
    HotKey_Use hotKey = HotKey_Use.None;

    [SerializeField]
    HotKey_Use testKey;
    /// <summary>
    /// 인풋 시스템 연결하는 로직 
    /// </summary>
    /// <param name="enable_Key">현재 연결할 상황</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        Debug.Log($"Enable 입력된키 : {Convert.ToString((int)enable_Key,2)} , 현재키 : {Convert.ToString((int)hotKey,2)}");
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:
                inputSystem.Player.Enable(); //Enable 함수는 액션이 등록되있거나 이미활성화상태면 아무작동도안하고 리턴한다.  2021.3.25f1 버전 기준
                break;
            case HotKey_Use.Use_TownMap:
                break;
            case HotKey_Use.Use_OptionView:
                break;
            default:
                break;
        }
        hotKey |= enable_Key;
        Debug.Log($"Enable 변경된 키 : {Convert.ToString((int)hotKey, 2)}");
    }

    /// <summary>
    /// 인풋 시스템 연결끊는 로직
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        Debug.Log($"Disable 입력 키 : {Convert.ToString((int)disable_Key,2)} , 현재 키 :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                       //초기화
                hotKey = HotKey_Use.None;
                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps; //열려있는 액션맵 전부찾아서 
                foreach (InputActionMap action in temp) 
                {
                    action.Disable(); //닫아버려
                                      //그리고 기본 입력 을 열기 
                    inputSystem.Test.Enable(); //테스트 하기위한 기본 은열어둔다.
                }
                break;
            case HotKey_Use.Use_BattleMap:                                              //배틀맵일때 
                hotKey &= HotKey_Use.Cancel_BattleMap;

                break;
            case HotKey_Use.Use_TownMap:                                                //마을일때 
                hotKey &= HotKey_Use.Cancel_TownMap;

                break;
            case HotKey_Use.Use_OptionView:                                             // 옵션창 열렸을때 
                hotKey &= HotKey_Use.Cancel_OptionView;

                break;
            case HotKey_Use.Use_TownMap | HotKey_Use.Use_OptionView:                    // 마을맵이고 옵션창 열렸을때 
                hotKey &= HotKey_Use.Cancel_TownMap;
                hotKey &= HotKey_Use.Cancel_OptionView;

                break;
            case HotKey_Use.Use_BattleMap | HotKey_Use.Use_OptionView:                  // 배틀맵이고 옵션창 열렸을때 
                hotKey &= HotKey_Use.Cancel_BattleMap;
                hotKey &= HotKey_Use.Cancel_OptionView;
                
                break;
            default:
                Debug.Log("딱맞는 케이스만 걸리네..");
                break;
        }
        Debug.Log($"Disable 변경된 키 :{Convert.ToString((int)hotKey,2)}");
    }

    protected override void Awake()
    {
        base.Awake();
        inputSystem = new();
        inputSystem.Test.Enable();
        inputSystem.Test.Test1.performed += Test1;
        inputSystem.Test.Test2.performed += Test2;
    }
    public void PlayerControlOpen()
    {
        inputSystem.Player.Enable();
    }
    public void PlayerControlClose()
    {
        inputSystem.Player.Disable();
    }

    private void Test1(InputAction.CallbackContext context)
    {
        //Debug.Log("1번");
        //EnableHotKey(testKey);
    }

    private void Test2(InputAction.CallbackContext context)
    {
        //Debug.Log("2번");
        //DisableHotKey(testKey);
    }

    private void Test(InputAction.CallbackContext context)
    {
        Debug.Log("인풋 시스템 열려있다 클릭이야");
    }


}
