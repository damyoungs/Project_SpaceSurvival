
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// WindowsList 
/// </summary>
public class GameOptionsBase : SingletonBase<GameOptionsBase>
{
    /// <summary>
    /// ��ǲ�ý��ۿ�����Ŭ����
    /// </summary>
    KeyMouseInputSystem inputActions;

    /// <summary>
    /// ������ ������
    /// </summary>
    GameObject optionsWindow;



    protected virtual void Awake()
    {
        inputActions = new KeyMouseInputSystem();
        optionsWindow = transform.GetChild(0).gameObject;
    }
    protected virtual void OnEnable()
    {
        inputActions.Enable();
        inputActions.KeyBorad.System.performed += OnOffWindowOption;
        inputActions.KeyBorad.InvenKey.performed += OnOffInventory;
        inputActions.KeyBorad.OptionKey.performed += OnOffWindowOption;
        inputActions.Mouse.MouseClick.performed += OnLeftClick;
    }

    protected virtual void OnDisable()
    {
        inputActions.Mouse.MouseClick.performed -= OnLeftClick;
        inputActions.KeyBorad.OptionKey.performed -= OnOffWindowOption;
        inputActions.KeyBorad.InvenKey.performed -= OnOffInventory;
        inputActions.KeyBorad.System.performed -= OnOffWindowOption;
        inputActions.Disable();
    }


    /// <summary>
    /// �޴� â �¿��� 
    /// </summary>
    /// <param name="context">�Է�����</param>
    protected virtual void OnOffWindowOption(InputAction.CallbackContext context)
    {
        if (context.performed) {
            optionsWindow.SetActive(!optionsWindow.activeSelf);
        }
    }

    /// <summary>
    /// �κ� â �¿���
    /// </summary>
    /// <param name="context"></param>
    private void OnOffInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("�κ�â����");

        }
    }

    protected virtual void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed) {
        }
    }

    
}
