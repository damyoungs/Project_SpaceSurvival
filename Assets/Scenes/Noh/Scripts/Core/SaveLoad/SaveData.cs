using EnumList;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ����ȭ�鿡 ���̴� ���� ���� 
/// �ٵ�� ��
/// </summary>
public class SaveData : ObjectIsPool
{
    ///// <summary>
    ///// ���ϸ�  
    ///// </summary>
    //private string fileName;
    //public string FileName { 
    //    get => fileName; 
    //    set => fileName = value;
    //}
    /// <summary>
    /// �������� �ε�����
    /// </summary>
    private int fileIndex = 0;
    public int FileIndex { 
        get => fileIndex;
        set {
            fileIndex = value;
            saveFileNameObj.text += fileIndex.ToString("D3");
        }
    }
    /// <summary>
    /// ����ȭ�鿡 ���� �������� ������¥
    /// </summary>
    private string createTime = "��õ�̽ʻ�����������Ͼ�ȩ�ýʺ�";
    public string CreateTime { 
        get => createTime;
        set { 
            createTime = value;
            createTimeObj.text = createTime.ToString();
        } 
    }
    /// <summary>
    /// ����ȭ�鿡 ���� ĳ�����̸�
    /// </summary>
    private string charcterName = "�׽���";
    public string CharcterName { 
        get => charcterName;
        set { 
            charcterName = value;
            charcterInfoObj.text += sceanName;
        }
    }

    /// <summary>
    /// ����ȭ�鿡���� ĳ���� �����ݾ�
    /// </summary>
    private double money = 3333333333333333333;
    public double Money { 
        get => money;
        set { 
            money = value;
            charcterInfoObj.text += $"   {money}";
        }
    }
    /// <summary>
    /// ����ȭ�鿡 ���� ������
    /// </summary>
    private string sceanName = "������?";
    public string SceanName {
        get => sceanName;
        set { 
            sceanName = value;
            saveFileNameObj.text += $"   {sceanName}";
        }
    }
    /// <summary>
    /// ������Ʈ�ؿ� �ؽ�Ʈ ������Ʈ�� 
    /// </summary>
    [SerializeField]
    TextMeshProUGUI saveFileNameObj;
    [SerializeField]
    TextMeshProUGUI charcterInfoObj;
    [SerializeField]
    TextMeshProUGUI createTimeObj;

    /// <summary>
    /// ������ �����Ǹ��� ���� ����
    /// </summary>
    private void Awake()
    {
        //���������� ���������ʰ� ��������
        isPositionReset = false;
    }
}
