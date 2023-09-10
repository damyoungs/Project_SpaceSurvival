using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// ����Ű �̺�Ʈ ��Ʈ���� �̳� ���� 
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                   // ó�����۽� �⺻�� : ������, Ÿ��Ʋ, �ε�â �� �Է��� �Ǹ�ȵǴ°��� ���� �÷��� 
    Use_BattleMap = 1,              // ��Ʋ�� ó�� ������Ȳ�� ���� �÷���
    Use_TownMap = 2,                // ���� ó�� ���������� ��Ȳ�ϰ�� ���� �÷���                   
    Use_OptionView = 4,             // �ɼ�â�� �������� üũ�� �÷���  - �����̳� ����� �κ�â ���� ���� ������ ����ɸ��Ѱ� ������ȵǴ� üũ�ʿ�
    Cancel_BattleMap = 254,
    Cancel_TownMap = 253,
    Cancel_OptionView = 251,

}

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

    [SerializeField]
    HotKey_Use testKey;
    /// <summary>
    /// ��ǲ �ý��� �����ϴ� ���� 
    /// </summary>
    /// <param name="enable_Key">���� ������ ��Ȳ</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        Debug.Log($"Enable �Էµ�Ű : {Convert.ToString((int)enable_Key,2)} , ����Ű : {Convert.ToString((int)hotKey,2)}");
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:
                inputSystem.Player.Enable(); //Enable �Լ��� �׼��� ��ϵ��ְų� �̹�Ȱ��ȭ���¸� �ƹ��۵������ϰ� �����Ѵ�.  2021.3.25f1 ���� ����
                break;
            case HotKey_Use.Use_TownMap:
                break;
            case HotKey_Use.Use_OptionView:
                break;
            default:
                break;
        }
        hotKey |= enable_Key;
        Debug.Log($"Enable ����� Ű : {Convert.ToString((int)hotKey, 2)}");
    }

    /// <summary>
    /// ��ǲ �ý��� ������� ����
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        Debug.Log($"Disable �Է� Ű : {Convert.ToString((int)disable_Key,2)} , ���� Ű :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                       //�ʱ�ȭ
                hotKey = HotKey_Use.None;
                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps; //�����ִ� �׼Ǹ� ����ã�Ƽ� 
                foreach (InputActionMap action in temp) 
                {
                    action.Disable(); //�ݾƹ���
                                      //�׸��� �⺻ �Է� �� ���� 
                    inputSystem.Test.Enable(); //�׽�Ʈ �ϱ����� �⺻ ������д�.
                }
                break;
            case HotKey_Use.Use_BattleMap:                                              //��Ʋ���϶� 
                hotKey &= HotKey_Use.Cancel_BattleMap;

                break;
            case HotKey_Use.Use_TownMap:                                                //�����϶� 
                hotKey &= HotKey_Use.Cancel_TownMap;

                break;
            case HotKey_Use.Use_OptionView:                                             // �ɼ�â �������� 
                hotKey &= HotKey_Use.Cancel_OptionView;

                break;
            case HotKey_Use.Use_TownMap | HotKey_Use.Use_OptionView:                    // �������̰� �ɼ�â �������� 
                hotKey &= HotKey_Use.Cancel_TownMap;
                hotKey &= HotKey_Use.Cancel_OptionView;

                break;
            case HotKey_Use.Use_BattleMap | HotKey_Use.Use_OptionView:                  // ��Ʋ���̰� �ɼ�â �������� 
                hotKey &= HotKey_Use.Cancel_BattleMap;
                hotKey &= HotKey_Use.Cancel_OptionView;
                
                break;
            default:
                Debug.Log("���´� ���̽��� �ɸ���..");
                break;
        }
        Debug.Log($"Disable ����� Ű :{Convert.ToString((int)hotKey,2)}");
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
        //Debug.Log("1��");
        //EnableHotKey(testKey);
    }

    private void Test2(InputAction.CallbackContext context)
    {
        //Debug.Log("2��");
        //DisableHotKey(testKey);
    }

    private void Test(InputAction.CallbackContext context)
    {
        Debug.Log("��ǲ �ý��� �����ִ� Ŭ���̾�");
    }


}
