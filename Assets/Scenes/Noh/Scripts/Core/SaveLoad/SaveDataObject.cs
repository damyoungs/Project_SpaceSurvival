using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ����ȭ�鿡 ���̴� ���� ���� 
/// 
/// </summary>
public class SaveDataObject : SaveDataIsPool
{
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
            createTimeObj.text = createTime;
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
    TextMeshProUGUI saveFileNameObj; // �����̸�? 
    [SerializeField]
    TextMeshProUGUI charcterInfoObj; // ĳ�����̸� , ������ġ , �� , ���� ����?
    [SerializeField]
    TextMeshProUGUI createTimeObj;   // ����ð� �����ֱ�

    /// <summary>
    /// ������ �����Ǹ��� ���� ����
    /// </summary>
    private void Awake()
    {
        //Ǯ���� ó���� ���������� ���������ʰ� ��������
        isPositionReset = false;
        Button bt = gameObject.AddComponent<Button>();
        bt.onClick.AddListener( () => TestClick());
    }


    protected override void OnEnable()
    {
        base.OnEnable();

    }
    
    
    private void TestClick()
    {
        if (FileIndex > 0) {
            Debug.Log($"{fileIndex} ��° Ŭ��");
            Debug.Log($"{objectIndex} �̰Կ�����Ʈ����");
        }
        else{
            Debug.Log($"{objectIndex} �̰Կ�����Ʈ����");
        }

    }
}
