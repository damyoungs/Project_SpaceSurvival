using TMPro;
using UnityEngine;

/// <summary>
/// ����ȭ�鿡 ���̴� ���� ���� 
/// �̿�����Ʈ Ŭ���� �����ε����� �Ѱܾ��Ѵ�.
/// </summary>
public class SaveGameObject : SaveData_PoolObj
{

    /// <summary>
    /// Ŭ�������� ������Ʈ �ε���
    /// </summary>
    [SerializeField]
    private int objectIndex = -1;
    public int ObjectIndex { 
        get => objectIndex; 
        set => objectIndex = value; 
    }
    /// <summary>
    /// �������� �ε�����
    /// -1���̸� �ʱ����
    /// </summary>
    [SerializeField]
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
    TextMeshProUGUI saveFileNumber; // �����̸�? 
    TextMeshProUGUI sceanNameObject; // ĳ�����̸� , ������ġ , �� , ���� ����?
    TextMeshProUGUI createTimeObj;   // ����ð� �����ֱ�
    TextMeshProUGUI charcterLevelObj; // �����̸�? 
    TextMeshProUGUI charcterMoneyObj; // ĳ�����̸� , ������ġ , �� , ���� ����?
    TextMeshProUGUI etcObj;   // ����ð� �����ֱ�



    SaveLoadPopupWindow proccessManager;
    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(0);
        saveFileNumber      = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        sceanNameObject     = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        createTimeObj       = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        charcterLevelObj    = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        charcterMoneyObj    = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        etcObj              = child.GetComponent<TextMeshProUGUI>();
    }
    protected override void OnEnable()
    {
        
    }
    private void Start()
    {
        proccessManager = WindowList.Instance.IOPopupWindow; //����ȭ�� ó���ϴ�Ŭ������������
    }

    public void InFocusObject() 
    {
        if (proccessManager.NewIndex != fileIndex) //���� ������Ʈ Ŭ���ߴ��� üũ
        {
            if (proccessManager.NewIndex > -1 && proccessManager.CopyCheck) // ī�ǹ�ư�� Ŭ���߰� �����Ͱ� ���°��� Ŭ�����ߴ��� üũ
            {

                proccessManager.OldIndex = proccessManager.NewIndex; // ī���� ������ ��ȣ ����
                proccessManager.NewIndex = fileIndex; //ī�ǵ� ������ ��ȣ ����
                proccessManager.OpenPopupAction(EnumList.SaveLoadButtonList.COPY); //ī�ǽ���

            }
            else
            {
                proccessManager.NewIndex = fileIndex; //����Ŭ�������� �ٽ� ���� 
            }
        }
        
    }

}
