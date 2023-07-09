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
                case EnumList.PopupList.SAVELOADPOPUP:
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
    PopupWindowBase invenWindow;
    public PopupWindowBase InvenWindow => invenWindow;

    /// <summary>
    /// ���� ���� ���� �ε� ���࿩�� ���� â ��������
    /// </summary>
    SaveLoadPopupWindow saveLoadPopupWindow;
    public SaveLoadPopupWindow IOPopupWindow => saveLoadPopupWindow;

    /// <summary>
    /// �����츮��Ʈ�� �׻�����ٴϴ°��̱⶧���� ���⿡�� �̺�Ʈó���� ����.
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
    }

    /// <summary>
    /// Ű�Է¹� ���콺 �Է�ó���� �߰�����
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        inputKeyEvent.Enable();
        inputKeyEvent.KeyBorad.System.performed += OnOffWindowOption; //Ű�Է½� �ɼ�â �¿��� 
        inputKeyEvent.KeyBorad.OptionKey.performed += OnOffWindowOption; // �����Ͱ� ����
        inputKeyEvent.KeyBorad.InvenKey.performed += OnOffInventory; // ���� �κ�â�� �ȸ��� 
        //inputKeyEvent.Mouse.MouseClick.performed += OnLeftClick; //ȭ�鿡�� Ŭ�������� ó���� �̺�Ʈ 
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
    /// �޴� â �¿��� 
    /// </summary>
    /// <param name="context">�Է�����</param>
    protected virtual void OnOffWindowOption(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading){ 
            if (context.performed)
            {
                mainWindow.gameObject.SetActive(!mainWindow.gameObject.activeSelf);//�ɼ������� ���� �ݰ� 
                
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
               invenWindow.gameObject.SetActive(!invenWindow.gameObject.activeSelf);  
            }

        }
    }
    /// <summary>
    /// ȭ��Ŭ���� ó�����̺�Ʈ
    /// �����̸����Ű��� Ŭ���ѿ�����Ʈ������ �����ü���� ó���� �����ϴ�.
    /// �ʿ������ ����
    /// </summary>
    /// <param name="context"></param>
    //protected virtual void OnLeftClick(InputAction.CallbackContext context)
    //{
    //    ���ε��̾ƴѰ�츸 �����Ѵ�. 
    //    if (!LoadingScean.IsLoading)
    //    {
    //        if (context.performed)
    //        {
    //            Debug.Log("Ŭ��������");

    //        }

    //    }
    //}

    /// <summary>
    /// �����ִ� â�� ���δݱ� .
    /// </summary>
    public void ResetPopupWindow() {


        for(int i = 0; i< transform.GetChild(0).childCount; i++) {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
}
