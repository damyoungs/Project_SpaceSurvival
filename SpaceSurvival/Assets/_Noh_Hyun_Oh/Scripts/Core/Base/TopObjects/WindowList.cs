using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// â���� �ֻ��� ������Ʈ�� ���
/// �������� �ܺ��Է��̺�Ʈó���� ���⼭ �ϸ� �����Ͱ���.
/// �ε�â���� ó���ȵǰ� �̺�Ʈ�Լ����� if (!LoadingScean.IsLoading){ } �ȿ��� �� ����ǰ� ������ּ���
/// WindowList , DataFactory  ������Ʈ�� ��Ȱ��ȭ�� ��Ű��������. ���ο�����Ʈ�� ��Ű���� �����ۼ���Ź�帳�ϴ�.
/// </summary>
public class WindowList : Singleton<WindowList> {

    /// <summary>
    /// Ȱ��ȭ �� �˾����ִ°�� defencePanel ����
    /// </summary>
    EnumList.PopupList activePopup;
    public EnumList.PopupList ActivePopup{
        get =>activePopup;
        set 
        {
            switch (value) {
                case EnumList.PopupList.SAVE_LOAD_POPUP:
                    defencePanel.gameObject.SetActive(true);
                    break;
                case EnumList.PopupList.NONE:
                    defencePanel.gameObject.SetActive(false);
                    break;
            }
            activePopup = value;
        } 
    }
    /// <summary>
    /// �˾�â�̶߸� ���潺 �г��� ��� ��ǲ�̺�Ʈ�� ���´�.
    /// </summary>
    DefenceEvent defencePanel;

    /// <summary>
    /// Ű�Է� �̺�Ʈ
    /// </summary>
    InputKeyMouse inputKeyEvent;
    public InputKeyMouse InputKeyEvent => inputKeyEvent;
    
    /// <summary>
    /// ������ ������ �� �ɼǰ��� ������
    /// </summary>
    SaveWindowManager mainWindow;
    public SaveWindowManager MainWindow => mainWindow;

    /// <summary>
    /// �κ��丮 ������Ʈ
    /// </summary>
    InventoryWindow invenWindow;
    public InventoryWindow InvenWindow => invenWindow;

    /// <summary>
    /// ���� ���� ���� �ε� ���࿩�� ���� â ��������
    /// </summary>
    SaveLoadPopupWindow saveLoadPopupWindow;
    public SaveLoadPopupWindow IOPopupWindow => saveLoadPopupWindow;

    /// <summary>
    /// �׽�Ʈ�� �ɼ� �˾�������
    /// </summary>
    OptionsPopupWindow optionsPopupWindow;
    public OptionsPopupWindow OptionsPopupWindow => optionsPopupWindow;


    /// <summary>
    /// �˾� â �������� �Ŵ���
    /// </summary>
    PopupSortManager popupManager;
    public PopupSortManager PopupSortManager => popupManager;
    
    /// <summary>
    /// �� �޴���  
    /// </summary>
    TurnManager turnManager;
    public TurnManager TurnManager => turnManager;

    /// <summary>
    /// �� ������ UI ��ġ
    /// </summary>
    Transform turnGaugeUI;
    public Transform TurnGaugeUI => turnGaugeUI;

    /// <summary>
    /// �����츮��Ʈ�� �׻������ٴϴ°��̱⶧���� ���⿡�� �̺�Ʈó���� ����.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        inputKeyEvent = new InputKeyMouse();
        //������Ʈ ���� ��ӹٲ��� �����̴��� GetComponentInChildren<Type>(true) ���� ã�ƵѶ���.. �Ź� �̰Ŷ��� ��ġ�����.
        defencePanel = transform.GetComponentInChildren<DefenceEvent>(true); 
        invenWindow = transform.GetComponentInChildren<InventoryWindow>(true);
        saveLoadPopupWindow = transform.GetComponentInChildren<SaveLoadPopupWindow>(true);
        mainWindow = transform.GetComponentInChildren<SaveWindowManager>(true);
        popupManager = transform.GetComponentInChildren<PopupSortManager>(true);
        optionsPopupWindow = transform.GetComponentInChildren<OptionsPopupWindow>(true);
        turnGaugeUI = transform.GetChild(0).GetChild(1);
        turnManager = FindObjectOfType<DataFactory>().transform.GetComponentInChildren<TurnManager>(true);
    }
    private void Start()
    {
        mainWindow.Oninitialize();
    }
    /// <summary>
    /// Ű�Է¹� ���콺 �Է�ó���� �߰�����
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        inputKeyEvent.Enable();
        inputKeyEvent.KeyBoard.System.performed += OffPopupWindow; // esc �Է½� ������� â�ݱ� 
        inputKeyEvent.KeyBoard.OptionKey.performed += OnOffWindowOption; // �ɼ�â �� ���̺�â �����ݱ�
        inputKeyEvent.KeyBoard.InvenKey.performed += OnOffInventory; // �κ�â �׽�Ʈ�� 
        InputKeyEvent.KeyBoard.StateKey.performed += OnOffStateWindow;// ����â �׽�Ʈ��
    }



    /// <summary>
    /// ��Ȱ��ȭ ������ ��������ɶ����Ǳ⶧���� �̺�Ʈ �����Լ� ó�����ص��ȴ�.
    /// �ٸ� �̱��� ������ ���������Ȱ��� Destroy�ɶ� ��Ȱ��ȭ�� �����ϱ⶧���� �����������ִ�. - Ȯ�οϷ�
    /// ���̵��� OnEnable�Լ��� ��ȣ������ʴ´� (Ȯ�οϷ�)
    /// </summary>
    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    inputKeyEvent.Mouse.MouseClick.performed -= OnLeftClick;
    //    inputKeyEvent.KeyBorad.OptionKey.performed -= OnOffWindowOption;
    //    inputKeyEvent.KeyBorad.InvenKey.performed -= OnOffInventory;
    //    inputKeyEvent.KeyBorad.System.performed -= OnOffWindowOption;
    //    inputKeyEvent.Disable();
    //}



    /// <summary>
    /// �˾�â ��ư�� ��������� ��������� ������ ��������� ������.
    /// <param name="target">���� �˾�â ��ü</param>
    /// </summary>
    public void popupOnOff(IPopupSortWindow target) {
        if (target.gameObject.activeSelf)//â�� ����������
        {
            popupManager.PopupClose(target) ;//�ݰ�
        }
        else //�ƴѰ�쿣
        { 
            popupManager.PopupOpen(target); //����
        }
    }

    /// <summary>
    /// �޴� â �¿��� 
    /// </summary>
    /// <param name="context">�Է�����</param>
    private void OnOffWindowOption(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading){ 
            if (context.performed)
            {
                popupOnOff(mainWindow);
            }
        }
    }

    /// <summary>
    /// �κ� â �¿���
    /// </summary>
    /// <param name="context"></param>
    private void OnOffInventory(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupOnOff(invenWindow);
            }

        }
    }

    /// <summary>
    /// ���� â �¿���
    /// </summary>
    /// <param name="context"></param>
    private void OnOffStateWindow(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupOnOff(optionsPopupWindow);
            }

        }
    }

    private void OffPopupWindow(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupManager.PopupClose();
            }

        }
    }
}
