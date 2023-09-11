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
/// ����Ű �̺�Ʈ ��Ʈ���� �̳� ���� 
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                       // ó�����۽� �⺻�� : ������, Ÿ��Ʋ, �ε�â �� �Է��� �Ǹ�ȵǴ°��� ���� �÷��� 
    Use_BattleMap = 1,              // ��Ʋ�� ó�� ������Ȳ�� ���� �÷���
    Use_TownMap = 2,                // ���� ó�� ���������� ��Ȳ�ϰ�� ���� �÷���                   
    Use_OptionView = 4,             // �ɼ�â �������� ������
    Use_InvenView = 8,              // �κ�â �������� ������

    Cancel_InvenView = 247,         // �κ� â �����ϱ����� ������
    Cancel_BattleMap = 254,         // ��Ʋ�� �ɼ��� �����ϱ����� ������
    Cancel_TownMap = 253,           // ������ �ɼ��� �����ϱ����� ������
    Cancel_OptionView = 251,        // �ɼ� â�� �����ϱ����� ������

}
/// <summary>
/// ĵ������ �ش� Ŭ������ �߰��ǰ� ĵ������ ȭ����ü�������� Ű���.
/// MonoBehaviour �� �����Լ� On ���� �Լ����� ������������. ��ü������ ������Ʈ ���������� ��� üũ�ϰ��ִٰ��Ѵ�.
/// </summary>
public class InputSystemController : ChildComponentSingeton<InputSystemController>
{
    
    /// <summary>
    /// ��ü������ ��ǲ�ý����� ������ ���� 
    /// </summary>
    InputKeyMouse inputSystem;
    static public InputKeyMouse InputSystem => Instance.inputSystem;
    /// <summary>
    /// ���� ��Ʈ���� ������ �Ǵ��� �̳Ѱ�
    /// </summary>
    [SerializeField]
    HotKey_Use hotKey = HotKey_Use.None;
    public HotKey_Use HotKey => hotKey;
    // Player �׼Ǹ�  
    public Action<Vector2> OnPlayer_Move;
    public Action OnPlayer_Jump;
    public Action OnPlayer_ItemPickUp;
    public Action OnPlayer_Equip_Item;
    public Action OnPlayer_MoveMode_Change;
    public Action OnPlayer_Run;

    //UI_Inven �׼Ǹ�

    public Action OnUI_Inven_Click;
    public Action OnUI_Inven_Click_Cancel;
    public Action<InputAction.CallbackContext> OnUI_Inven_Shift; //per �����ѹ� cancel�ѹ� 
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

    //Camera �׼Ǹ�

    public Action OnCamera_RightRotate;
    public Action OnCamera_LeftRotate;

    //BattleMap_Player �׼Ǹ�

    public Action OnBattleMap_Player_UnitMove;

    //Input_Action_NoneGame �׼Ǹ�

    public Action OnInput_Action_NoneGame_Esc;
    public Action OnInput_Action_NoneGame_MouseClick;

    //UI_Options �׼Ǹ�

    public Action OnUI_Options_WindowOpen;
    public Action OnUI_Options_Esc;

    //UI_ModalPopup �׼Ǹ�
    public Action OnUI_ModalPopup_OnOff;


    protected override void Awake()
    {
        base.Awake();
        InputSystemActionSetting();

    }

    /// <summary>
    /// ��ǲ�ý��ۿ��� ���� �׼ǵ� ����ϴ� �Լ� 
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
    /// ��ǲ �ý��� �����ϴ� ���� 
    /// </summary>
    /// <param name="enable_Key">���� ������ ��Ȳ</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        //Debug.Log($"Enable �Էµ�Ű : {Convert.ToString((int)enable_Key,2)} , ����Ű : {Convert.ToString((int)hotKey,2)}");
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:                                  //��Ʋ�� ���Խ�
                //inputSystem.Player.Enable(); //Enable �Լ��� �׼��� ��ϵ��ְų� �̹�Ȱ��ȭ���¸� �ƹ��۵������ϰ� �����Ѵ�.  2021.3.25f1 ���� ����
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.BattleMap_Player.Enable();
                inputSystem.Camera.Enable();
                break;
            
            case HotKey_Use.Use_TownMap:                                    // ���� ���Խ�
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.Player.Enable(); 
                break;
            
            case HotKey_Use.Use_OptionView:                                 //�ɼ�â ��밡�� �ҽ�
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.UI_Options.Enable();
                inputSystem.UI_ModalPopup.Enable();
                break;

            case HotKey_Use.Use_InvenView:                                  //�κ� ��밡�� �ҽ�
                inputSystem.Input_Action_NoneGame.Disable();

                inputSystem.QuickSlot.Enable();
                inputSystem.UI_Inven.Enable();
                break;

            default:
                break;
        }
        hotKey |= enable_Key;
        //Debug.Log($"Enable ����� Ű : {Convert.ToString((int)hotKey, 2)}");
    }

    /// <summary>
    /// ��ǲ �ý��� ������� ����
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        //Debug.Log($"Disable �Է� Ű : {Convert.ToString((int)disable_Key,2)} , ���� Ű :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                       //�ε�,������,Ÿ��Ʋ 
                hotKey = HotKey_Use.None; //Ű�� �����ϰ� 

                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps; //�����ִ� �׼Ǹ� ����ã�Ƽ� 
                foreach (InputActionMap action in temp)
                {
                    action.Disable(); //�ݾƹ���
                }
                inputSystem.Input_Action_NoneGame.Enable(); //�⺻�� ����.
#if UNITY_EDITOR
                inputSystem.Test.Enable(); //�������鼭 �ٴݱ⶧���� �׽�Ʈ�� ����������.. �׷��� �������
#endif
                break;
            case HotKey_Use.Use_BattleMap:                                              //��Ʋ�ʿ��� �������  
                hotKey &= HotKey_Use.Cancel_BattleMap;

                inputSystem.BattleMap_Player.Disable();
                inputSystem.Camera.Disable();

                break;
            case HotKey_Use.Use_TownMap:                                                //�������� �������  
                hotKey &= HotKey_Use.Cancel_TownMap;

                inputSystem.Player.Disable();

                break;
            case HotKey_Use.Use_InvenView:                                              // �κ� �ݱ�
                hotKey &= HotKey_Use.Cancel_InvenView;

                //inputSystem.QuickSlot.Disable();
                inputSystem.UI_Inven.Disable();

                break;
            case HotKey_Use.Use_OptionView:                                             // �ɼ�â �ݱ� 
                hotKey &= HotKey_Use.Cancel_OptionView;

                inputSystem.UI_Options.Disable();
                inputSystem.UI_ModalPopup.Disable();

                break;
            default:
                Debug.Log($"���°��� : {disable_Key} , byte ������ enum ���� switch �ҽÿ� 0000 0101 �̷������� üũ�ϴµ� ���´� ���� �ɸ���.. ");
                break;
        }
        //Debug.Log($"Disable ����� Ű :{Convert.ToString((int)hotKey,2)}");
    }

}
