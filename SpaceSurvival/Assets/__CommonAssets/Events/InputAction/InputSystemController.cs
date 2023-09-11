using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// 단축키 이벤트 컨트롤할 이넘 설정 
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                       // 처음시작시 기본값 : 오프닝, 타이틀, 로딩창 등 입력이 되면안되는곳에 사용될 플래그 
    Use_BattleMap = 1,              // 배틀맵 처럼 전투상황에 사용될 플래그
    Use_TownMap = 2,                // 마을 처럼 전투가없는 상황일경우 사용될 플래그                   
    Use_OptionView = 4,             // 옵션창 열기위한 값셋팅
    Use_InvenView = 8,              // 인벤창 열기위한 값셋팅

    Cancel_InvenView = 247,         // 인벤 창 제외하기위한 값셋팅
    Cancel_BattleMap = 254,         // 배틀맵 옵션을 제외하기위한 값셋팅
    Cancel_TownMap = 253,           // 마을맵 옵션을 제외하기위한 값셋팅
    Cancel_OptionView = 251,        // 옵션 창을 제외하기위한 값셋팅

}
/// <summary>
/// 캔버스에 해당 클래스가 추가되고 캔버스는 화면전체영역으로 키운다.
/// MonoBehaviour 의 내장함수 On 관련 함수들은 막을수가없다. 자체적으로 업데이트 내부적으로 계속 체크하고있다고한다.
/// </summary>
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
    public HotKey_Use HotKey => hotKey;
    // Player 액션맵  
    public Action<Vector2> OnPlayer_Move;
    public Action OnPlayer_Jump;
    public Action OnPlayer_ItemPickUp;
    public Action OnPlayer_Equip_Item;
    public Action OnPlayer_MoveMode_Change;
    public Action OnPlayer_Run;

    //UI_Inven 액션맵

    public Action OnUI_Inven_Click;
    public Action OnUI_Inven_Click_Cancel;
    public Action<InputAction.CallbackContext> OnUI_Inven_Shift; //per 에서한번 cancel한번 
    public Action OnUI_Inven_EquipBox_Open;
    public Action OnUI_Inven_Inven_Open;
    public Action OnUI_Inven_MouseClickRight;

    //QuickSlot

    public Action OnQuickSlot_Popup;
    public Action OnQuickSlot_Shift;
    public Action OnQuickSlot_Eight;
    public Action OnQuickSlot_Nine;
    public Action OnQuickSlot_Zero;
    public Action OnQuickSlot_Ctrl;
    public Action OnQuickSlot_Alt;
    public Action OnQuickSlot_Space;
    public Action OnQuickSlot_Insert;

    //Camera 액션맵

    public Action OnCamera_RightRotate;
    public Action OnCamera_LeftRotate;

    //BattleMap_Player 액션맵

    public Action OnBattleMap_Player_UnitMove;

    //Input_Action_NoneGame 액션맵

    public Action OnInput_Action_NoneGame_Esc;
    public Action OnInput_Action_NoneGame_MouseClick;

    //UI_Options 액션맵

    public Action OnUI_Options_WindowOpen;
    public Action OnUI_Options_Esc;

    //UI_ModalPopup 액션맵
    public Action OnUI_ModalPopup_OnOff;


    protected override void Awake()
    {
        base.Awake();
        InputSystemActionSetting();

    }

    /// <summary>
    /// 인풋시스템에서 사용될 액션들 등록하는 함수 
    /// </summary>
    private void InputSystemActionSetting() 
    {
        inputSystem = new();

        inputSystem.Player.Move.performed += (context) => { OnPlayer_Move(context.ReadValue<Vector2>()); };
        inputSystem.Player.Jump.performed += (_) => { OnPlayer_Jump(); };
        inputSystem.Player.ItemPickUp.performed += (_) => { OnPlayer_ItemPickUp(); };
        inputSystem.Player.Equip_Item.performed += (_) => { OnPlayer_Equip_Item(); };
        inputSystem.Player.MoveMode_Change.performed += (_) => { OnPlayer_MoveMode_Change(); };
        inputSystem.Player.Run.performed += (_) => { OnPlayer_Run(); };


        inputSystem.UI_Inven.Click.performed += (_) => { OnUI_Inven_Click(); };
        inputSystem.UI_Inven.Click.canceled += (_) => { OnUI_Inven_Click_Cancel(); };
        inputSystem.UI_Inven.Shift.performed += (context) => { OnUI_Inven_Shift(context); };
        inputSystem.UI_Inven.Shift.canceled += (context) => { OnUI_Inven_Shift(context); };
        inputSystem.UI_Inven.EquipBox_Open.performed += (_) => { OnUI_Inven_EquipBox_Open(); };
        inputSystem.UI_Inven.Inven_Open.performed += (_) => { OnUI_Inven_Inven_Open(); };
        inputSystem.UI_Inven.MouseClickRight.performed += (_) => { OnUI_Inven_MouseClickRight(); };


        inputSystem.Camera.RightRotate.performed += (_) => { OnCamera_RightRotate(); };
        inputSystem.Camera.LeftRotate.performed += (_) => { OnCamera_LeftRotate(); };


        inputSystem.QuickSlot.PopUp.performed += (_) => { OnQuickSlot_Popup(); };
        inputSystem.QuickSlot.Shift.performed += (_) => { OnQuickSlot_Shift(); };
        inputSystem.QuickSlot.Eight.performed += (_) => { OnQuickSlot_Eight(); };
        inputSystem.QuickSlot.Nine.performed += (_) => { OnQuickSlot_Nine(); };
        inputSystem.QuickSlot.Zero.performed += (_) => { OnQuickSlot_Zero(); };
        inputSystem.QuickSlot.Ctrl.performed += (_) => { OnQuickSlot_Ctrl(); };
        inputSystem.QuickSlot.Alt.performed += (_) => { OnQuickSlot_Alt(); };
        inputSystem.QuickSlot.Space.performed += (_) => { OnQuickSlot_Space(); };
        inputSystem.QuickSlot.Insert.performed += (_) => { OnQuickSlot_Insert(); };


        inputSystem.BattleMap_Player.UnitMove.performed += (_) => { OnBattleMap_Player_UnitMove(); };


        inputSystem.Input_Action_NoneGame.Esc.performed += (_) => { OnInput_Action_NoneGame_Esc(); };
        inputSystem.Input_Action_NoneGame.MouseClick.performed += (_) => { OnInput_Action_NoneGame_MouseClick(); };


        inputSystem.UI_Options.Esc.performed += (_) => { OnUI_Options_Esc(); };
        inputSystem.UI_Options.WindowOpen.performed += (_) => { OnUI_Options_WindowOpen(); };


        inputSystem.UI_ModalPopup.OnOff.performed += (_) => { OnUI_ModalPopup_OnOff(); };
    }
    /// <summary>
    /// 인풋 시스템 연결하는 로직 
    /// </summary>
    /// <param name="enable_Key">현재 연결할 상황</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        //Debug.Log($"Enable 입력된키 : {Convert.ToString((int)enable_Key,2)} , 현재키 : {Convert.ToString((int)hotKey,2)}");
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:                                  //배틀맵 진입시
                //inputSystem.Player.Enable(); //Enable 함수는 액션이 등록되있거나 이미활성화상태면 아무작동도안하고 리턴한다.  2021.3.25f1 버전 기준
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.BattleMap_Player.Enable();
                inputSystem.Camera.Enable();
                break;
            
            case HotKey_Use.Use_TownMap:                                    // 마을 진입시
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.Player.Enable(); 
                break;
            
            case HotKey_Use.Use_OptionView:                                 //옵션창 사용가능 할시
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.UI_Options.Enable();
                inputSystem.UI_ModalPopup.Enable();
                break;

            case HotKey_Use.Use_InvenView:                                  //인벤 사용가능 할시
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.QuickSlot.Enable();
                inputSystem.UI_Inven.Enable();
                break;

            default:
                break;
        }
        hotKey |= enable_Key;
        //Debug.Log($"Enable 변경된 키 : {Convert.ToString((int)hotKey, 2)}");
    }

    /// <summary>
    /// 인풋 시스템 연결끊는 로직
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        //Debug.Log($"Disable 입력 키 : {Convert.ToString((int)disable_Key,2)} , 현재 키 :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                       //로딩,오프닝,타이틀 
                hotKey = HotKey_Use.None; //키값 수정하고 

                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps; //열려있는 액션맵 전부찾아서 
                foreach (InputActionMap action in temp)
                {
                    action.Disable(); //닫아버려
                }
                inputSystem.Input_Action_NoneGame.Enable(); //기본만 연다.
#if UNITY_EDITOR
                inputSystem.Test.Enable(); //포문돌면서 다닫기때문에 테스트도 닫혀버린다.. 그러니 열어두자
#endif
                break;
            case HotKey_Use.Use_BattleMap:                                              //배틀맵에서 벗어낫을때  
                hotKey &= HotKey_Use.Cancel_BattleMap;

                inputSystem.BattleMap_Player.Disable();
                inputSystem.Camera.Disable();

                break;
            case HotKey_Use.Use_TownMap:                                                //마을에서 벗어낫을때  
                hotKey &= HotKey_Use.Cancel_TownMap;

                inputSystem.Player.Disable();

                break;
            case HotKey_Use.Use_InvenView:                                              // 인벤 닫기
                hotKey &= HotKey_Use.Cancel_InvenView;

                //inputSystem.QuickSlot.Disable();
                inputSystem.UI_Inven.Disable();

                break;
            case HotKey_Use.Use_OptionView:                                             // 옵션창 닫기 
                hotKey &= HotKey_Use.Cancel_OptionView;

                inputSystem.UI_Options.Disable();
                inputSystem.UI_ModalPopup.Disable();

                break;
            default:
                Debug.Log($"들어온값이 : {disable_Key} , byte 형식의 enum 으로 switch 할시에 0000 0101 이런값으로 체크하는데 딱맞는 값만 걸리네.. ");
                break;
        }
        //Debug.Log($"Disable 변경된 키 :{Convert.ToString((int)hotKey,2)}");
    }

}
