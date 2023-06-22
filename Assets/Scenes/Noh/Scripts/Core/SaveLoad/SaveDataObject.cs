using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// ����ȭ�鿡 ���̴� ���� ���� 
/// �̿�����Ʈ Ŭ���� �����ε����� �Ѱܾ��Ѵ�.
/// </summary>
public class SaveDataObject : SaveDataIsPool
{
    //Ŭ�������� �� ���É����� üũ�Һ���
    bool isFocusing = false;
    /// <summary>
    /// Ŭ�������� ������Ʈ �ε���
    /// </summary>
    private int objectIndex = -1;
    public int ObjectIndex { 
        get => objectIndex; 
        set => objectIndex = value; 
    }
    /// <summary>
    /// �������� �ε�����
    /// -1���̸� �ʱ����
    /// </summary>
    private int fileIndex = -1;
    public int FileIndex { 
        get => fileIndex;
        set {
            fileIndex = value;
            if (fileIndex > -1)
            {
                saveFileNumber.text = $"No.{fileIndex.ToString("D3")}";
            }
            else
            {
                saveFileNumber.text = $"No.{objectIndex.ToString("D3")}" ;
            }
        }
    }
    /// <summary>
    /// ����ȭ�鿡 ���� �������� ������¥
    /// </summary>
    private string createTime ;
    public string CreateTime { 
        get => createTime;
        set { 
            createTime = value;
            createTimeObj.text = $"{createTime}";
        } 
    }
    /// <summary>
    /// ����ȭ�鿡 ���� ĳ�����̸�
    /// </summary>
    private int charcterLevel = -1;
    public int CharcterLevel { 
        get => charcterLevel;
        set { 
            charcterLevel = value;
            charcterLevelObj.text = $"Lv.{charcterLevel}";
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
            charcterMoneyObj.text = $"$ {money}";
        }
    }
    /// <summary>
    /// ����ȭ�鿡 ���� ������
    /// </summary>
    private EnumList.SceanName sceanName;
    public EnumList.SceanName SceanName {
        get => sceanName;
        set { 
            sceanName = value;
            sceanNameObject.text = $" Map :{sceanName}";
        }
    }
    /// <summary>
    /// ������Ʈ�ؿ� �ؽ�Ʈ ������Ʈ�� 
    /// </summary>
    [SerializeField]
    TextMeshProUGUI saveFileNumber; // �����̸�? 
    [SerializeField]
    TextMeshProUGUI sceanNameObject; // ĳ�����̸� , ������ġ , �� , ���� ����?
    [SerializeField]
    TextMeshProUGUI createTimeObj;   // ����ð� �����ֱ�
    [SerializeField]
    TextMeshProUGUI charcterLevelObj; // �����̸�? 
    [SerializeField]
    TextMeshProUGUI charcterMoneyObj; // ĳ�����̸� , ������ġ , �� , ���� ����?
    [SerializeField]
    TextMeshProUGUI etcObj;   // ����ð� �����ֱ�
    [SerializeField]
    GameObject isFocus;

    InputKeyMouse inputSystem;

    /// <summary>
    /// ������ �����Ǹ��� ���� ����
    /// </summary>
    private void Awake()
    {
        inputSystem = new InputKeyMouse();
        //Ǯ���� ó���� ���������� ���������ʰ� ��������
        isPositionReset = false;
    }

   
    public void InFocusObject() {
        if (SaveLoadPopupWindow.Instance.NewIndex > -1 && SaveLoadPopupWindow.Instance.CopyCheck)
        {
            SaveLoadPopupWindow.Instance.OldIndex = SaveLoadPopupWindow.Instance.NewIndex;
            SaveLoadPopupWindow.Instance.NewIndex = fileIndex;
            SaveLoadPopupWindow.Instance.OpenPopupAction(EnumList.SaveLoadButtonList.COPY);
        }
        else { 
            SaveLoadPopupWindow.Instance.NewIndex = fileIndex;
        }
        isFocusing = !isFocusing;
        Debug.Log(isFocusing);
    }
    
}
