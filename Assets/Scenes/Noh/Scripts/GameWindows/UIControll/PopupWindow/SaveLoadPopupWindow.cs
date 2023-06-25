using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �˾� ó���� Ŭ����
/// </summary>
public class SaveLoadPopupWindow : Singleton<SaveLoadPopupWindow>
{
    GameObject saveButton;
    GameObject loadButton;
    GameObject copyButton;
    GameObject deleteButton;
    TextMeshProUGUI windowText;

    
    public Action<int,bool> focusInChangeFunction;

    /// <summary>
    /// false �� ī��Ȱ��ȭ �ȵȰ� true �� Ȱ��ȭ
    /// </summary>
    bool copyCheck = false;
    public bool CopyCheck {
        get => copyCheck;
        set { 
            copyCheck = value;
            if (copyCheck)//ī�ǹ�ưŬ����
            {
                oldIndex = newIndex;// ī�Ǵ������ÿ� �������ð��� ������ �׸��̵ȴ�.
                focusInChangeFunction?.Invoke(newIndex, copyCheck); // ī�Ǵ������� ��Ŀ�̼���
            }
            else // ī�ǹ�ư ������ 
            {
                focusInChangeFunction?.Invoke(oldIndex, copyCheck);// ī�Ǿȴ�������
            }
        } 
    }
    /// <summary>
    /// ī������ ������ ������ �ε����� -1�̸� �ʱⰪ 
    /// </summary>
    int oldIndex = -1;
    public int OldIndex {
        get => oldIndex; 
        set 
        {
            if (copyCheck) //ī�Ƿ����϶��� ���� �����Ѵ� 
            {
                oldIndex = value;
            }
        }
    }
    /// <summary>
    /// ���� ���� ���� �ε� ���� ���Ǵ� �ε�����
    /// </summary>
    int newIndex = -2;
    public int NewIndex { 
        get => newIndex;
        set
        {
            newIndex = value;
            focusInChangeFunction?.Invoke(newIndex, copyCheck);//�ٲﰪ ����
        }
    }

    /// <summary>
    /// ���� �ε� ī�� ���� ��ư�������� üũ �Һ���
    /// </summary>
    EnumList.SaveLoadButtonList buttonType;
    public EnumList.SaveLoadButtonList ButtonType {
        get => buttonType;
        set => buttonType = value;
    }

    /// <summary>
    /// ������Ʈ ã��
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        saveButton = transform.GetChild(1).GetChild(1).gameObject;
        loadButton = transform.GetChild(1).GetChild(2).gameObject;
        copyButton = transform.GetChild(1).GetChild(3).gameObject;
        deleteButton = transform.GetChild(1).GetChild(4).gameObject;
        windowText = transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>();
    }
    /// <summary>
    /// ó������ 
    /// </summary>
    /// <param name="type">���ư�������� �׿��´� �˾�â Ȱ��ȭ</param>
    public void OpenPopupAction(EnumList.SaveLoadButtonList type)
    {
        if (newIndex > -1) { 
            buttonType = type;
            switch (type) { 
                case EnumList.SaveLoadButtonList.SAVE:
                    saveButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.LOAD:
                    loadButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.COPY:
                    copyButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.DELETE:
                    deleteButton.SetActive(true);
                    break;
            }
            windowText.text = $"{type} �Ͻðڽ��ϱ� ?";
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }


   
}
