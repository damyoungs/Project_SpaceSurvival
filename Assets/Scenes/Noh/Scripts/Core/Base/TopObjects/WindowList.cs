using System.Collections;
using System.Collections.Generic;
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
                    defencePanel.SetActive(true);
                    break;
                case EnumList.PopupList.NONE:
                    defencePanel.SetActive(false);
                    break;
            }
            activePopup = value;
        } 
    }
    /// <summary>
    /// �˾�â�̶߸� ���潺 �г��� ��� ��ǲ�̺�Ʈ�� ���´�.
    /// </summary>
    GameObject defencePanel;

    /// <summary>
    /// Ű�Է� �̺�Ʈ
    /// </summary>
    InputKeyMouse inputKeyEvent;
    public InputKeyMouse InputKeyEvent => inputKeyEvent;
    /// <summary>
    /// ������ ������ �� �ɼǰ��� ������
    /// </summary>
    GameObject optionsWindow;
    public GameObject OptionsWindow => optionsWindow;
    /// <summary>
    /// �����츮��Ʈ�� �׻�����ٴϴ°��̱⶧���� ���⿡�� �̺�Ʈó���� ����.
    /// </summary>

    protected override void Awake()
    {
        base.Awake();
        inputKeyEvent = new InputKeyMouse();
        defencePanel = transform.GetComponentInChildren<DefenceEvent>(true).gameObject; //�̰Ǽ����� ���� �������̷��ߵǼ� ��ӹٲ��.
        optionsWindow = transform.GetChild(0).gameObject; //�ɼ�������� 0��°��
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
        inputKeyEvent.Mouse.MouseClick.performed += OnLeftClick; //ȭ�鿡�� Ŭ�������� ó���� �̺�Ʈ 

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
                optionsWindow.SetActive(!optionsWindow.activeSelf);//�ɼ������� ���� �ݰ� 
                
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
                Debug.Log("�κ�â����");

            }

        }
    }
    /// <summary>
    /// ȭ��Ŭ���� ó�����̺�Ʈ
    /// �����̸����Ű��� Ŭ���ѿ�����Ʈ������ �����ü���� ó���� �����ϴ�.
    /// �ʿ������ ����
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnLeftClick(InputAction.CallbackContext context)
    {
        //���ε��̾ƴѰ�츸 �����Ѵ�. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                //Debug.Log("Ŭ��������");

            }

        }
    }

}
